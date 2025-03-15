using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraCharts; 
using HighLandsCoffee_Manager.BUS;
using HighLandsCoffee_Manager.Helpers;
using Guna.UI2.WinForms;
using System.Drawing;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using DevExpress.XtraEditors;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmPhanTichDuLieu : DevExpress.XtraEditors.XtraForm
    {
        private BUS_PhanTich busPhanTich;

        public frmPhanTichDuLieu()
        {

            InitializeComponent();
            busPhanTich = new BUS_PhanTich(); // Khởi tạo BUS_PhanTich
        }

        #region Methods for Form Load and Button Click Events

        private void frmPhanTichDuLieu_Load(object sender, EventArgs e)
        {
            // Load danh sách chủ đề vào ComboBox
            List<string> danhSachChuDe = busPhanTich.LayDanhSachChuDe();
            cboChuDe.Properties.Items.AddRange(danhSachChuDe);
            cboChuDe.SelectedIndex = 0;
        }

        private void btnThucHien_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị WaitForm
                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang xử lý, vui lòng đợi...");

                // Lấy các giá trị đầu vào
                string chuDe = cboChuDe.EditValue.ToString();
                DateTime ngayBD = dateFirst.Value;
                DateTime ngayKT = dateSecond.Value;

                // Kiểm tra ngày bắt đầu và ngày kết thúc
                if (ngayBD > ngayKT)
                {
                    XtraMessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy danh sách các measures từ checkbox
                List<string> danhSachMeasures = LayDanhSachMeasures();

                // Lấy danh sách thời gian từ checkbox
                List<string> danhSachThoiGian = LayDanhSachThoiGian();

                // Thực hiện phân tích dữ liệu và hiển thị kết quả
                DataTable resultForGrid = busPhanTich.ThucThiPhanTich(danhSachMeasures, chuDe, danhSachThoiGian, ngayBD, ngayKT, true);
                DataTable resultForChart = busPhanTich.ThucThiPhanTich(danhSachMeasures, chuDe, danhSachThoiGian, ngayBD, ngayKT, false);

                // Hiển thị kết quả lên DataGridView
                if (resultForGrid != null && resultForGrid.Rows.Count > 0)
                {
                    HienThiDataGridView(resultForGrid);
                }

                // Hiển thị biểu đồ
                if (resultForChart != null && resultForChart.Rows.Count > 0)
                {
                    HienThiBieuDo(resultForChart);
                }
                else
                {
                    XtraMessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi thực hiện phân tích: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đóng WaitForm khi thao tác hoàn tất
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
            }
        }

        #endregion

        #region Methods for Data and Time Handling

        // Phương thức lấy danh sách measures
        private List<string> LayDanhSachMeasures()
        {
            List<string> danhSachMeasures = new List<string>();

            if (cbDoanhThu.Checked) danhSachMeasures.Add("Doanh Thu");
            if (cbChietKhau.Checked) danhSachMeasures.Add("Chiet Khau");
            if (cbSoluongBan.Checked) danhSachMeasures.Add("So Luong Ban");
            if (cbLoiNhuan.Checked) danhSachMeasures.Add("Loi Nhuan");

            return danhSachMeasures;
        }

        // Phương thức lấy danh sách thời gian
        private List<string> LayDanhSachThoiGian()
        {
            List<string> danhSachThoiGian = new List<string>();

            if (cbNam.Checked) danhSachThoiGian.Add("Nam");
            if (cbQuy.Checked) danhSachThoiGian.Add("Quy");
            if (cbThang.Checked) danhSachThoiGian.Add("Thang");

            return danhSachThoiGian;
        }
        //private void CustomizeGridViewAppearance(Guna2DataGridView gridView)
        //{
        //    gridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        //    gridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        //    gridView.ColumnHeadersHeight = 35;
        //    gridView.ReadOnly = true;
        //    gridView.AllowUserToAddRows = false;
        //    gridView.AllowUserToDeleteRows = false;
        //    gridView.EnableHeadersVisualStyles = false;
        //    gridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        //    gridView.RowHeadersVisible = false;
        //    gridView.ScrollBars = ScrollBars.Both;
        //    gridView.ForeColor = Color.Black;
        //    gridView.DefaultCellStyle.Font = new Font("Segoe UI", 10); // Đặt font mặc định cho các ô
        //    gridView.DefaultCellStyle.ForeColor = Color.DarkBlue; // Đặt màu chữ mặc định
        //    gridView.DefaultCellStyle.SelectionBackColor = Color.LightGray; // Màu nền khi chọn
        //    gridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Màu chữ khi chọn

        //    // Tự động chỉnh chiều rộng cột
        //    gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

        //    // Đặt chiều rộng tối thiểu
        //    foreach (DataGridViewColumn column in gridView.Columns)
        //    {
        //        column.MinimumWidth = 100;
        //    }

        //    // Lắng nghe sự kiện DataBindingComplete để cập nhật kiểu
        //    gridView.DataBindingComplete += (s, e) =>
        //    {
        //        foreach (DataGridViewRow row in gridView.Rows)
        //        {
        //            foreach (DataGridViewCell cell in row.Cells)
        //            {
        //                // Đảm bảo màu chữ được áp dụng sau khi dữ liệu được load
        //                cell.Style.ForeColor = Color.DarkBlue;
        //            }
        //        }
        //    };
        //}



        #endregion

        #region Hiển thị dữ liệu lên dgv và biểu đồ

        // Phương thức hiển thị dữ liệu lên DataGridView
        private void HienThiDataGridView(DataTable result)
        {
            UI_Helpers.CustomizeGridViewAppearance(dgvLoadDLPT);
            dgvLoadDLPT.DataSource = result;
            dgvLoadDLPT.Refresh();
        }

        // Phương thức hiển thị biểu đồ
        private void HienThiBieuDo(DataTable result)
        {
            try
            {
                // Xóa biểu đồ hiện tại trước khi vẽ lại
                loadChartPT.Series.Clear();

                // Kiểm tra nếu DataTable có dữ liệu hợp lệ
                if (result == null || result.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Không có dữ liệu để hiển thị biểu đồ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Tạo biểu đồ từ DataTable
                foreach (DataColumn column in result.Columns)
                {
                    // Kiểm tra nếu cột là một measure (có "(VNĐ)" hoặc "(SP)")
                    if (column.ColumnName.Contains("(VNĐ)") || column.ColumnName.Contains("(SP)"))
                    {
                        // Tạo một Series mới cho mỗi measure
                        Series series = new Series(column.ColumnName, ViewType.Bar);

                        // Duyệt qua từng dòng của DataTable và thêm điểm vào series
                        foreach (DataRow row in result.Rows)
                        {
                            // Kiểm tra nếu cột đầu tiên (dimension) có dữ liệu hợp lệ
                            if (row[0] != DBNull.Value && row[0] != null)
                            {
                                string label = row[0].ToString();  // Cột đầu tiên là dimension (VD: Chi Nhánh, Tháng)

                                // Lấy giá trị của measure, kiểm tra nếu giá trị hợp lệ
                                var value = row[column.ColumnName];

                                // Kiểm tra nếu giá trị của measure hợp lệ (không phải DBNull hoặc null)
                                if (value != DBNull.Value && value != null)
                                {
                                    // Kiểm tra xem giá trị có phải là một kiểu số hợp lệ (IConvertible)
                                    if (value is IConvertible)
                                    {
                                        double numericValue = Convert.ToDouble(value);  // Chuyển giá trị về kiểu số (double)
                                        series.Points.Add(new SeriesPoint(label, numericValue));
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show($"Giá trị không hợp lệ tại cột {column.ColumnName}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    XtraMessageBox.Show($"Giá trị trong cột {column.ColumnName} tại dòng {row[0]} là không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                        // Thêm series vào biểu đồ
                        loadChartPT.Series.Add(series);
                    }
                }

                // Cài đặt thuộc tính cho biểu đồ
                loadChartPT.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                // Đặt tiêu đề cho biểu đồ
                ChartTitle chartTitle = new ChartTitle();
                chartTitle.Text = "Biểu đồ phân tích dữ liệu";
                loadChartPT.Titles.Clear();
                loadChartPT.Titles.Add(chartTitle);

                // Làm mới biểu đồ
                loadChartPT.Refresh();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi hiển thị biểu đồ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Xuất File Word & Excel

        private void btnXuatWord_Click(object sender, EventArgs e)
        {
            try
            {
                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang xuất Word, vui lòng đợi...");

                if (dgvLoadDLPT.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Khởi tạo ứng dụng Word
                Word.Application wordApp = new Word.Application();
                Word.Document doc = wordApp.Documents.Add();

                // Thêm tiêu đề cho tài liệu
                Word.Paragraph para = doc.Paragraphs.Add();
                para.Range.Text = "Báo Cáo Phân Tích Dữ Liệu";
                para.Range.Font.Size = 14;
                para.Range.Font.Bold = 1;
                para.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                para.Range.InsertParagraphAfter();

                // Thêm bảng vào tài liệu
                Word.Table table = doc.Tables.Add(para.Range, dgvLoadDLPT.RowCount + 1, dgvLoadDLPT.ColumnCount);
                table.Borders.Enable = 1;

                // Thiết lập tiêu đề cột
                for (int i = 0; i < dgvLoadDLPT.ColumnCount; i++)
                {
                    table.Cell(1, i + 1).Range.Text = dgvLoadDLPT.Columns[i].HeaderText;
                    table.Cell(1, i + 1).Range.Font.Bold = 1;
                }

                // Thêm dữ liệu vào bảng
                for (int i = 0; i < dgvLoadDLPT.RowCount; i++)
                {
                    for (int j = 0; j < dgvLoadDLPT.ColumnCount; j++)
                    {
                        table.Cell(i + 2, j + 1).Range.Text = dgvLoadDLPT.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Lưu biểu đồ dưới dạng ảnh
                string chartImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChartImage.png");
                loadChartPT.ExportToImage(chartImagePath, System.Drawing.Imaging.ImageFormat.Png);

                // Thêm ảnh vào Word
                Word.Paragraph chartPara = doc.Paragraphs.Add();
                chartPara.Range.InlineShapes.AddPicture(chartImagePath);
                chartPara.Range.InsertParagraphAfter();

                // Lưu tài liệu vào màn hình chính
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Đường dẫn đến màn hình chính
                string path = Path.Combine(desktopPath, "BaoCaoPhanTichDuLieu.docx");
                doc.SaveAs2(path);
                doc.Close();
                wordApp.Quit();

                // Đóng WaitForm trước khi xuất thông báo
                WaitFormHelper.CloseWaitForm(splashScreenManager1);

                XtraMessageBox.Show("Đã xuất thành công! File được lưu tại: " + path, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở file Word sau khi lưu
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                // Đóng WaitForm trước khi xuất thông báo lỗi
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
                XtraMessageBox.Show("Lỗi khi xuất dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang xuất Excel, vui lòng đợi...");

                if (dgvLoadDLPT.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Khởi tạo ứng dụng Excel
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false; // Ẩn ứng dụng Excel khi xuất

                // Tạo một workbook mới và worksheet mới
                var workbooks = excelApp.Workbooks;
                var workbook = workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

                // Thiết lập tiêu đề cột cho bảng Excel
                for (int i = 0; i < dgvLoadDLPT.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvLoadDLPT.Columns[i].HeaderText;
                }

                // Thêm dữ liệu vào worksheet
                for (int i = 0; i < dgvLoadDLPT.RowCount; i++)
                {
                    for (int j = 0; j < dgvLoadDLPT.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvLoadDLPT.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Tạo biểu đồ và thêm vào Excel
                var charts = worksheet.ChartObjects() as Microsoft.Office.Interop.Excel.ChartObjects;
                var chartObject = charts.Add(100, 100, 500, 300) as Microsoft.Office.Interop.Excel.ChartObject;
                var chart = chartObject.Chart;

                // Đặt phạm vi dữ liệu cho biểu đồ (ví dụ: sử dụng dữ liệu từ cột đầu tiên và cột thứ hai)
                var chartRange = worksheet.Range["A1", $"B{dgvLoadDLPT.RowCount + 1}"];
                chart.SetSourceData(chartRange);

                // Thiết lập loại biểu đồ
                chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered; // Ví dụ: biểu đồ cột

                // Lưu file Excel vào màn hình chính (Desktop)
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Đường dẫn đến màn hình chính
                string excelFilePath = Path.Combine(desktopPath, "BaoCaoPhanTichDuLieuVoiBieuDo.xlsx");
                workbook.SaveAs(excelFilePath);

                // Đóng workbook và ứng dụng Excel
                workbook.Close(false);
                excelApp.Quit();

                // Đóng WaitForm trước khi xuất thông báo
                WaitFormHelper.CloseWaitForm(splashScreenManager1);

                XtraMessageBox.Show("Đã xuất thành công! File Excel được lưu tại: " + excelFilePath, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở file Excel sau khi lưu
                System.Diagnostics.Process.Start(excelFilePath);
            }
            catch (Exception ex)
            {
                // Đóng WaitForm trước khi xuất thông báo lỗi
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
                XtraMessageBox.Show("Lỗi khi xuất dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }


}

