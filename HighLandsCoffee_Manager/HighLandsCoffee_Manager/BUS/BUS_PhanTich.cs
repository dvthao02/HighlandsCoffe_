using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.XtraCharts;
using HighLandsCoffee_Manager.DAO;

namespace HighLandsCoffee_Manager.BUS
{
    public class BUS_PhanTich
    {
        private readonly DAO_PhanTich daoPhanTich;

        public BUS_PhanTich()
        {
            daoPhanTich = new DAO_PhanTich();
        }

        #region Lấy danh sách chủ đề
        // Lấy danh sách chủ đề để hiển thị trên UI (ComboBox)
        public List<string> LayDanhSachChuDe()
        {
            try
            {
                return daoPhanTich.LayDanhSachChuDe();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chủ đề: " + ex.Message);
            }
        }
        #endregion

        #region Tạo và Thực thi Truy vấn MDX
        // Hàm tạo câu truy vấn MDX tổng quát và thực thi truy vấn
        public DataTable ThucThiPhanTich(List<string> danhSachMeasures,
                                  string chuDe,
                                  List<string> danhSachThoiGian,
                                  DateTime? ngayBD,
                                  DateTime? ngayKT,
                                  bool formatForGrid = true) // Thêm tham số formatForGrid
        {
            try
            {
                // Tạo câu truy vấn MDX tổng quát từ DAO
                string mdxQuery = daoPhanTich.TaoCauTruyVanTongQuat(danhSachMeasures, chuDe, danhSachThoiGian, ngayBD, ngayKT);

                // In ra câu truy vấn MDX (Có thể gỡ bỏ khi deploy vào production)
                Console.WriteLine("MDX Query: " + mdxQuery);

                // Thực thi câu truy vấn MDX và lấy kết quả dưới dạng DataTable
                DataTable dataTable = daoPhanTich.ThucThiTruyVanMDX(mdxQuery);

                if (formatForGrid)
                {
                    // Nếu cần định dạng cho DataGridView, ta gọi phương thức định dạng
                    return DinhDangGiaTri(dataTable);
                }
                else
                {
                    // Nếu không cần định dạng cho DataGridView (biểu đồ), trả về dữ liệu gốc
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi phân tích: " + ex.Message);
            }
        }

        #endregion

        #region Ánh xạ cột và định dạng
        // Phương thức ánh xạ tên cột cho UI
        public string GetColumnDisplayName(string columnName)
        {
            try
            {
                return daoPhanTich.GetColumnDisplayName(columnName);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi ánh xạ cột: " + ex.Message);
            }
        }

        // Phương thức định dạng giá trị cho UI
        public DataTable DinhDangGiaTri(DataTable dataTable)
        {
            try
            {
                return daoPhanTich.DinhDangGiaTri(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi định dạng giá trị: " + ex.Message);
            }
        }
        #endregion

        #region Biểu đồ 
        public void HienThiBieuDo(DataTable data, ChartControl chartControl)
        {
            try
            {
                // Khởi tạo lớp ChartDisplay để vẽ biểu đồ
                var chartDisplay = new DAO_PhanTich.ChartDisplay(chartControl);

                // Hiển thị biểu đồ với dữ liệu từ DataTable
                chartDisplay.HienThiBieuDo(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hiển thị biểu đồ: " + ex.Message);
            }
        }
        #endregion
    }
}
