using Guna.UI2.WinForms;
using HighLandsCoffee_Manager.BUS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmLoadData : DevExpress.XtraEditors.XtraForm
    {
        public frmLoadData()
        {
            InitializeComponent();
        }

        private void frmLoadData_Load(object sender, EventArgs e)
        {
            LoadComBobox();
            ApplyGridViewAppearance();
        }

        #region Job
        string Job_deletedata = "exec_delete_data";
        string Job_HeThong = "exec_form_to_nds_HLCF";
        string Job_Access = "exec_access_to_nds_HLCF";
        string Job_NapDDS = "exec_nds_to_dds_HLCF";
        #endregion

        BUS_NapDL dt = new BUS_NapDL();

        #region Status job nap
        public void LoadComBobox()
        {
            cboHTDL.Properties.Items.Clear();
            cboHTDL.Properties.Items.Add("DDS Highlands Coffee");
            cboHTDL.Properties.Items.Add("NDS Highlands Coffee");
            cboHTDL.SelectedIndex = 0; // Chọn mục đầu tiên
            Get_Table();

        }

        public void KiemTra_Status_Job_NapKho()
        {
            try
            {
                splashScreenManager1.SplashFormStartPosition = DevExpress.XtraSplashScreen.SplashFormStartPosition.CenterScreen;
                splashScreenManager1.ShowWaitForm();
                if (dt.Is_Run_NapKho() == false)//ngừng chạy
                {
                    Get_Table();
                    splashScreenManager1.CloseWaitForm();
                }
            }
            catch
            {
                splashScreenManager1.CloseWaitForm();
            }
        }
        #endregion


        #region Hiển thị dữ liệu
        private int lastSelectedPageIndex = -1; // Biến lưu trữ trang đã chọn cuối cùng
        private int lastSelectedComboBoxIndex = -1; // Biến lưu trữ lựa chọn combobox cuối cùng
        // Hàm Get_Table chỉ được gọi khi trạng thái thay đổi
        public void Get_Table()
        {
            // Kiểm tra xem trang hiện tại và combobox có thay đổi so với lần trước không
            if (lastSelectedPageIndex == navP1.SelectedPageIndex && lastSelectedComboBoxIndex == cboHTDL.SelectedIndex)
            {
                // Nếu không thay đổi, không gọi lại Get_Table
                return;
            }

            // Nếu có thay đổi, tiếp tục gọi Get_Table
            splashScreenManager1.SplashFormStartPosition = DevExpress.XtraSplashScreen.SplashFormStartPosition.CenterScreen;
            splashScreenManager1.ShowWaitForm();

            try
            {
                // Lưu trạng thái hiện tại
                lastSelectedPageIndex = navP1.SelectedPageIndex;
                lastSelectedComboBoxIndex = cboHTDL.SelectedIndex;

                if (cboHTDL.SelectedIndex == 0)
                {
                    switch (navP1.SelectedPageIndex)
                    {
                        case 0:
                            dgv1.DataSource = dt.Get_DanhSach_Kho_DW("FactKinhDoanh");
                            dgv1.AutoResizeColumns();
                            dgv1.DataSource = dt.LamMoi_DuLieu_DW("FactKinhDoanh");
                            break;
                        case 1:
                            dgv2.DataSource = dt.Get_DanhSach_Kho_DW("DimThoiGian");
                            dgv2.AutoResizeColumns();
                            dgv2.DataSource = dt.LamMoi_DuLieu_DW("DimThoiGian");
                            break;
                        case 2:
                            dgv3.DataSource = dt.Get_DanhSach_Kho_DW("DimSanPham");
                            dgv3.AutoResizeColumns();
                            dgv3.DataSource = dt.LamMoi_DuLieu_DW("DimSanPham");
                            break;
                        case 3:
                            dgv4.DataSource = dt.Get_DanhSach_Kho_DW("DimDanhMuc");
                            dgv4.AutoResizeColumns();
                            dgv4.DataSource = dt.LamMoi_DuLieu_DW("DimDanhMuc");
                            break;
                        case 4:
                            dgv5.DataSource = dt.Get_DanhSach_Kho_DW("DimNhanVien");
                            dgv5.AutoResizeColumns();
                            dgv5.DataSource = dt.LamMoi_DuLieu_DW("DimNhanVien");
                            break;
                        case 5:
                            dgv6.DataSource = dt.Get_DanhSach_Kho_DW("DimChiNhanh");
                            dgv6.AutoResizeColumns();
                            dgv6.DataSource = dt.LamMoi_DuLieu_DW("DimChiNhanh");
                            break;
                        case 6:
                            dgv7.DataSource = dt.Get_DanhSach_Kho_DW("DimKhuVuc");
                            dgv7.AutoResizeColumns();
                            dgv7.DataSource = dt.LamMoi_DuLieu_DW("DimKhuVuc");
                            break;
                        case 7:
                            dgv8.DataSource = dt.Get_DanhSach_Kho_DW("DimKhachHang");
                            dgv8.AutoResizeColumns();
                            dgv8.DataSource = dt.LamMoi_DuLieu_DW("DimKhachHang");
                            break;
                        case 8:
                            dgv9.DataSource = dt.Get_DanhSach_Kho_DW("DimLoaiKhachHang");
                            dgv9.AutoResizeColumns();
                            dgv9.DataSource = dt.LamMoi_DuLieu_DW("DimLoaiKhachHang");
                            break;
                        default:
                            MessageBox.Show("Vui lòng chọn mục hợp lệ.");
                            break;
                    }
                }
                else if (cboHTDL.SelectedIndex == 1)
                {
                    switch (navP1.SelectedPageIndex)
                    {
                        case 0:
                            dgv1.DataSource = dt.Get_DanhSach_NDS("DonHang");
                            dgv1.AutoResizeColumns();
                            dgv1.DataSource = dt.LamMoi_DuLieuNDS("DonHang");
                            break;
                        case 1:
                            dgv2.DataSource = dt.Get_DanhSach_NDS("ChiTietDonHang");
                            dgv2.AutoResizeColumns();
                            dgv2.DataSource = dt.LamMoi_DuLieuNDS("ChiTietDonHang");
                            break;
                        case 2:
                            dgv3.DataSource = dt.Get_DanhSach_NDS("KhachHang");
                            dgv3.AutoResizeColumns();
                            dgv3.DataSource = dt.LamMoi_DuLieuNDS("KhachHang");
                            break;
                        case 3:
                            dgv4.DataSource = dt.Get_DanhSach_NDS("LoaiKhachHang");
                            dgv4.AutoResizeColumns();
                            dgv4.DataSource = dt.LamMoi_DuLieuNDS("LoaiKhachHang");
                            break;
                        case 4:
                            dgv5.DataSource = dt.Get_DanhSach_NDS("SanPham");
                            dgv5.AutoResizeColumns();
                            dgv5.DataSource = dt.LamMoi_DuLieuNDS("SanPham");
                            break;
                        case 5:
                            dgv6.DataSource = dt.Get_DanhSach_NDS("DanhMuc");
                            dgv6.AutoResizeColumns();
                            dgv6.DataSource = dt.LamMoi_DuLieuNDS("DanhMuc");
                            break;
                        case 6:
                            dgv7.DataSource = dt.Get_DanhSach_NDS("KhuVuc");
                            dgv7.AutoResizeColumns();
                            dgv7.DataSource = dt.LamMoi_DuLieuNDS("KhuVuc");
                            break;
                        case 7:
                            dgv8.DataSource = dt.Get_DanhSach_NDS("ChiNhanh");
                            dgv8.AutoResizeColumns();
                            dgv8.DataSource = dt.LamMoi_DuLieuNDS("ChiNhanh");
                            break;
                        case 8:
                            dgv9.DataSource = dt.Get_DanhSach_NDS("NhanVien");
                            dgv9.AutoResizeColumns();
                            dgv9.DataSource = dt.LamMoi_DuLieuNDS("NhanVien");
                            break;
                        default:
                            MessageBox.Show("Vui lòng chọn mục hợp lệ.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                // Đóng WaitForm sau khi dữ liệu đã được tải
                splashScreenManager1.CloseWaitForm();
            }
        }
        #endregion

        private void navP1_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {
            // Gọi lại Get_Table khi thay đổi trang
            Get_Table();
        }
        #region Tùy chỉnh giao diện
        private void ApplyGridViewAppearance()
        {
            // Duyệt qua từng DataGridView và áp dụng tùy chỉnh
            foreach (var gridView in new[] { dgv1, dgv2, dgv3, dgv4, dgv5, dgv6, dgv7, dgv8, dgv9 })
            {
                CustomizeGridViewAppearance(gridView);
            }
        }
        // Tùy chỉnh giao diện DataGridView
        private void CustomizeGridViewAppearance(Guna2DataGridView gridView)
        {
            // Đặt nền bảng
            gridView.BackgroundColor = Color.FromArgb(245, 236, 222); // Màu kem nhạt (tone Highlands Coffee)

            // Màu lưới
            gridView.GridColor = Color.FromArgb(189, 144, 97); // Màu nâu sáng (tone cà phê)

            // Header cột
            gridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 92, 58); // Màu nâu sẫm
            gridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Màu chữ trắng
            gridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold); // Tăng kích thước font
            gridView.ColumnHeadersHeight = 40; // Chiều cao của header cột

            // Các ô dữ liệu
            gridView.DefaultCellStyle.BackColor = Color.White; // Màu nền ô dữ liệu
            gridView.DefaultCellStyle.ForeColor = Color.Black; // Màu chữ
            gridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(124, 92, 58); // Màu nền khi chọn (tone cà phê)
            gridView.DefaultCellStyle.SelectionForeColor = Color.White; // Màu chữ khi chọn
            gridView.DefaultCellStyle.Font = new Font("Segoe UI", 10); // Tăng kích thước font

            // Đặt chế độ chỉ xem, không cho phép thêm/xoá hàng
            gridView.ReadOnly = true;
            gridView.AllowUserToAddRows = false;
            gridView.AllowUserToDeleteRows = false;

            // Tắt chế độ focus/selection trên header
            gridView.EnableHeadersVisualStyles = false;

            // Border Style
            gridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridView.RowHeadersVisible = false; // Ẩn các header dòng

            // Tự động điều chỉnh cột để vừa khung
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        #endregion


        private void cboHTDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem combobox có chọn DDS hay NDS
            if (cboHTDL.SelectedIndex == 0) // DDS Highlands Coffee
            {
                // Đặt lại caption cho các trang của navP1
                navP1.Pages[0].Caption = "Thông tin Fact";
                navP1.Pages[1].Caption = "Thời gian";
                navP1.Pages[2].Caption = "Sản phẩm";
                navP1.Pages[3].Caption = "Danh mục";
                navP1.Pages[4].Caption = "Nhân viên";
                navP1.Pages[5].Caption = "Chi nhánh";
                navP1.Pages[6].Caption = "Khu vực";
                navP1.Pages[7].Caption = "Khách hàng";
                navP1.Pages[8].Caption = "Loại khách hàng";
            }
            else if (cboHTDL.SelectedIndex == 1) // NDS Highlands Coffee
            {
                // Đặt lại caption cho các trang của navP1
                navP1.Pages[0].Caption = "Đơn hàng";
                navP1.Pages[1].Caption = "Chi tiết đơn hàng";
                navP1.Pages[2].Caption = "Khách hàng";
                navP1.Pages[3].Caption = "Loại khách hàng";
                navP1.Pages[4].Caption = "Sản phẩm";
                navP1.Pages[5].Caption = "Danh mục";
                navP1.Pages[6].Caption = "Khu Vực";
                navP1.Pages[7].Caption = "Chi Nhánh";
                navP1.Pages[8].Caption = "Nhân Viên";
            }
            // Gọi lại phương thức để tải dữ liệu mới dựa trên lựa chọn combobox
            Get_Table();


        }

        private void navP1_Click(object sender, EventArgs e)
        {

        }
    }
}