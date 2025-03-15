using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraCharts;
using HighLandsCoffee_Manager.DTO;
using Microsoft.AnalysisServices.AdomdClient;

namespace HighLandsCoffee_Manager.DAO
{
    public class DAO_PhanTich
    {
        private AdomdConnection con;
        private KetNoiSQL dt = new KetNoiSQL();

        #region Kết nối DB
        private void OpenConnection()
        {
            if (con == null)
                con = dt.conDB_CuBe();

            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        private void CloseConnection()
        {
            if (con != null && con.State == ConnectionState.Open)
                con.Close();
        }
        #endregion

        #region Lấy Danh Sách Chủ Đề
        public List<string> LayDanhSachChuDe()
        {
            return new List<string>
            {
                "Chi Nhánh",
                "Khu Vực",
                "Khách Hàng",
                "Tuổi",
                "Giới tính",
                "Đối tượng Khách hàng",
                "Sản phẩm",
                "Danh mục"
            };
        }
        #endregion

        #region Ánh Xạ Chủ Đề và Measures
        private string GetChuDe(string chuDe)
        {
            switch (chuDe)
            {
                case "Chi Nhánh": return "[Dim Chi Nhanh].[Ten Chi Nhanh]";
                case "Khu Vực": return "[Dim Chi Nhanh].[Ten Khu Vuc]";
                case "Khách Hàng": return "[Dim Khach Hang].[Ten KH]";
                case "Tuổi": return "[Dim Khach Hang].[Tuoi]";
                case "Giới tính": return "[Dim Khach Hang].[Gioi Tinh]";
                case "Đối tượng Khách hàng": return "[Dim Khach Hang].[Ten Loai KH]";
                case "Sản phẩm": return "[Dim San Pham].[Ten San Pham]";
                case "Danh mục": return "[Dim San Pham].[Ten Danh Muc]";
                default: return "[Dim Chi Nhanh].[Ten Chi Nhanh]";
            }
        }

        private readonly Dictionary<string, string> columnMapping = new Dictionary<string, string>
        {
            { "[Dim Chi Nhanh].[Ten Chi Nhanh].[Ten Chi Nhanh].[MEMBER_CAPTION]", "Tên Chi Nhánh" },
            { "[Dim Chi Nhanh].[Ten Khu Vuc].[Ten Khu Vuc].[MEMBER_CAPTION]", "Tên Khu Vực" },
            { "[Dim Khach Hang].[Ten KH].[Ten KH].[MEMBER_CAPTION]", "Tên Khách Hàng" },
            { "[Dim Khach Hang].[Tuoi].[Tuoi].[MEMBER_CAPTION]", "Tuổi" },
            { "[Dim Khach Hang].[Gioi Tinh].[Gioi Tinh].[MEMBER_CAPTION]", "Giới Tính" },
            { "[Dim Khach Hang].[Ten Loai KH].[Ten Loai KH].[MEMBER_CAPTION]", "Đối Tượng Khách Hàng" },
            { "[Dim San Pham].[Ten San Pham].[Ten San Pham].[MEMBER_CAPTION]", "Tên Sản Phẩm" },
            { "[Dim San Pham].[Ten Danh Muc].[Ten Danh Muc].[MEMBER_CAPTION]", "Tên Danh Mục" },
            { "[Dim Thoi Gian].[Thang].[Thang].[MEMBER_CAPTION]", "Tháng" },
            { "[Dim Thoi Gian].[Quy].[Quy].[MEMBER_CAPTION]", "Quý" },
            { "[Dim Thoi Gian].[Nam].[Nam].[MEMBER_CAPTION]", "Năm" },
            { "[Measures].[Doanh Thu]", "Doanh Thu (VNĐ)" },
            { "[Measures].[Chiet Khau]", "Chiết Khấu (VNĐ)" },
            { "[Measures].[So Luong Ban]", "Số Lượng Bán (SP)" },
            { "[Measures].[Loi Nhuan]", "Lợi Nhuận (VNĐ)" }
        };

        public string GetColumnDisplayName(string columnName)
        {
            return columnMapping.ContainsKey(columnName) ? columnMapping[columnName] : columnName;
        }
        #endregion

        #region Tạo và Thực thi Truy Vấn MDX
        public string TaoCauTruyVanTongQuat(
            List<string> danhSachMeasures,
            string chuDe,
            List<string> danhSachThoiGian,
            DateTime? ngayBD,
            DateTime? ngayKT
        )
        {
            if (danhSachMeasures == null || !danhSachMeasures.Any())
                throw new Exception("Vui lòng chọn ít nhất một Measure.");

            string measures = string.Join(", ", danhSachMeasures.Select(m => $"[Measures].[{m}]"));
            string dimensionChuDe = GetChuDe(chuDe);

            string timeDimensions = string.Empty;
            if (danhSachThoiGian != null && danhSachThoiGian.Any())
            {
                // Tạo chuỗi timeDimensions, mỗi Dim Thoi Gian đều có .MEMBERS
                timeDimensions = string.Join(" * ", danhSachThoiGian.Select(t => $"[Dim Thoi Gian].[{t}].MEMBERS"));
            }

            // Kiểm tra nếu có timeDimensions thì sử dụng nó, không thì sử dụng dimensionChuDe.MEMBERS
            string rowDimensions = string.IsNullOrEmpty(timeDimensions)
                ? $"{dimensionChuDe}.MEMBERS"
                : $"({dimensionChuDe}.MEMBERS * {timeDimensions})";

            // Xử lý câu WHERE để lọc theo ngày nếu có
            string whereClause = string.Empty;
            if (ngayBD.HasValue && ngayKT.HasValue)
            {
                whereClause = $"WHERE ([Dim Thoi Gian].[Ngay].&[{ngayBD.Value:yyyy-MM-dd}]:[Dim Thoi Gian].[Ngay].&[{ngayKT.Value:yyyy-MM-dd}])";
            }
            return $@"
                    SELECT 
                        NON EMPTY {{ {measures} }} ON COLUMNS,
                        NON EMPTY {{ {rowDimensions} }} 
                        ON ROWS
                    FROM [DDS Highlands Coffee]
                    {whereClause}
                    CELL PROPERTIES VALUE, FORMATTED_VALUE";
        }

        public DataTable ThucThiTruyVanMDX(string mdxQuery, bool formatForGrid = false)
        {
            try
            {
                OpenConnection();
                using (var cmd = new AdomdCommand(mdxQuery, con))
                {
                    using (var adapter = new AdomdDataAdapter(cmd))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count == 0)
                            throw new Exception("Không có dữ liệu trả về từ truy vấn.");

                        foreach (DataColumn column in dataTable.Columns)
                        {
                            column.ColumnName = GetColumnDisplayName(column.ColumnName);
                        }

                        // Định dạng dữ liệu nếu cần
                        if (formatForGrid)
                        {
                            dataTable = DinhDangGiaTri(dataTable);
                        }

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi truy vấn MDX: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion

        #region Định Dạng Giá Trị
        public DataTable DinhDangGiaTri(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName.Contains("(VNĐ)"))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row[column.ColumnName] = $"{row[column.ColumnName]:N0} VNĐ";
                    }
                }
                else if (column.ColumnName.Contains("(SP)"))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row[column.ColumnName] = $"{row[column.ColumnName]} SP";
                    }
                }
            }
            return dataTable;
        }
        #endregion

        #region Hiển Thị Biểu đồ
        public class ChartDisplay
        {
            private ChartControl chartControl;

            public ChartDisplay(ChartControl chartControl)
            {
                this.chartControl = chartControl;
            }

            public void HienThiBieuDo(DataTable data)
            {
                if (data == null || data.Rows.Count == 0) return;

                chartControl.Series.Clear();

                foreach (DataColumn column in data.Columns)
                {
                    if (column.ColumnName.Contains("(VNĐ)") || column.ColumnName.Contains("(SP)"))
                    {
                        Series series = new Series(column.ColumnName, ViewType.Bar);

                        foreach (DataRow row in data.Rows)
                        {
                            string label = row[0].ToString();
                            var value = row[column.ColumnName];

                            if (value != DBNull.Value)
                            {
                                series.Points.Add(new SeriesPoint(label, value));
                            }
                        }

                        chartControl.Series.Add(series);
                    }
                }

                chartControl.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                ChartTitle chartTitle = new ChartTitle();
                chartTitle.Text = "Biểu đồ phân tích dữ liệu";
                chartControl.Titles.Clear();
                chartControl.Titles.Add(chartTitle);
            }
        }
        #endregion
    }
}
