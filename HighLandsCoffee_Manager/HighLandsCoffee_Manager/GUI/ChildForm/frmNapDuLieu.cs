using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using HighLandsCoffee_Manager.BUS;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using HighLandsCoffee_Manager.Helpers;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmNapDuLieu : XtraForm
    {
        private BUS_NapDL dt = new BUS_NapDL();

        #region JobName 
        private const string Job_XoaDL = "delete_data_dds";
        private const string Job_HeThong = "exec_form_to_nds";
        private const string Job_Access = "exec_access_to_nds";
        private const string Job_NapDDS = "exec_nds_to_dds";
        #endregion
        public frmNapDuLieu()
        {
            InitializeComponent();
        }

        private void frmNapDuLieu_Load(object sender, EventArgs e)
        {
            SetStart();
        }

        #region Hàm Xử lý sự kiện 
        private void btnLamSach_Click(object sender, EventArgs e)
        {
            LamSachDuLieu();
            LoadComboBox();
        }
        private void btnNapDDS_Click(object sender, EventArgs e)
        {
            NapKhoDDS();
            LoadComboBox();
        }
        private void btnXoaDL_Click(object sender, EventArgs e)
        {
            XoaDuLieuKho();
            LoadComboBox();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetStart();
        }



        #endregion

        #region Status job nap
        public bool KiemTra_Status_Job_NapKho()
        {
            try
            {
                // Kiểm tra trạng thái công việc nạp dữ liệu
                return dt.Is_Run_NapKho(); // Nếu đang chạy, trả về true, nếu không trả về false
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Nếu có lỗi, trả về false
            }
        }
        #endregion

        #region Hiển thị dữ liệu
        private int lastSelectedPageIndex = -1;
        private int lastSelectedComboBoxIndex = -1;

        public void GetTable()
        {
            try
            {
                // Tránh gọi lại nếu không có thay đổi
                if (lastSelectedPageIndex == navPane.SelectedPageIndex && lastSelectedComboBoxIndex == cboHTDL.SelectedIndex)
                {
                    return;
                }
                ApplyGridViewAppearance();

                // Hiển thị WaitForm khi tải dữ liệu
                WaitFormHelper.ShowWaitForm(splashScreenManager1,"Đang tải dữ liệu , vui lòng chờ...");

                // Lưu trạng thái hiện tại
                lastSelectedPageIndex = navPane.SelectedPageIndex;
                lastSelectedComboBoxIndex = cboHTDL.SelectedIndex;

                // Kiểm tra trạng thái dữ liệu (DDS hoặc NDS)
                if (cboHTDL.SelectedIndex == 0) // NDS
                {
                    LoadNDSData();
                }
                else if (cboHTDL.SelectedIndex == 1) // DDS
                {
                    LoadDDSData();
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có ngoại lệ
                MessageBox.Show($"Đã xảy ra lỗi khi tải dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đóng WaitForm
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
            }
        }
        private void LoadDDSData()
        {
            switch (navPane.SelectedPageIndex)
            {
                case 0:
                    dgv1.DataSource = dt.Get_DanhSach_Kho_DW("sp_HienThiThongTinKinhDoanh");
                    dgv1.AutoResizeColumns();
                    break;
                case 1:
                    dgv2.DataSource = dt.Get_DanhSach_Kho_DW("sp_HienThiDoanhThuTheoThoiGian");
                    dgv2.AutoResizeColumns();
                    break;
                case 2:
                    dgv3.DataSource = dt.Get_DanhSach_Kho_DW("sp_HienThiThongTinKhachHang");
                    dgv3.AutoResizeColumns();
                    break;
                case 3:
                    dgv4.DataSource = dt.Get_DanhSach_Kho_DW("sp_HienThiThongTinSanPham");
                    dgv4.AutoResizeColumns();
                    break;
                case 4:
                    dgv5.DataSource = dt.Get_DanhSach_Kho_DW("sp_HienThiLoiNhuanTheoKhuVuc");
                    dgv5.AutoResizeColumns();
                    break;
                case 5:
                    dgv6.DataSource = dt.Get_DanhSach_Kho_DW("sp_HienThiThongTinChiNhanh");
                    dgv6.AutoResizeColumns();
                    break;
                default:
                    MessageBox.Show("Vui lòng chọn mục hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void LoadNDSData()
        {
            switch (navPane.SelectedPageIndex)
            {
                case 0:
                    dgv1.DataSource = dt.Get_DanhSach_NDS("sp_HienThiThongTinDonHang");
                    dgv1.AutoResizeColumns();
                    break;
                case 1:
                    dgv2.DataSource = dt.Get_DanhSach_NDS("sp_HienThiKhachHangLoaiKH");
                    dgv2.AutoResizeColumns();
                    break;
                case 2:
                    dgv3.DataSource = dt.Get_DanhSach_NDS("sp_HienThiCongThucSanPham");
                    dgv3.AutoResizeColumns();
                    break;
                case 3:
                    dgv4.DataSource = dt.Get_DanhSach_NDS("sp_HienThiThongTinNhapHang");
                    dgv4.AutoResizeColumns();
                    break;
                case 4:
                    dgv5.DataSource = dt.Get_DanhSach_NDS("sp_HienThiChiTietKhuyenMai");
                    dgv5.AutoResizeColumns();
                    break;
                case 5:
                    dgv6.DataSource = dt.Get_DanhSach_NDS("sp_HienThiTonKhoNguyenLieu");
                    dgv6.AutoResizeColumns();
                    break;
                default:
                    MessageBox.Show("Vui lòng chọn mục hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }
        #endregion

        #region Combobox và Giao diện Navigation Pane
        private void LoadComboBox()
        {
            cboHTDL.Items.Clear();
            cboHTDL.Items.Add("NDS Highlands Coffee");
            cboHTDL.Items.Add("DDS Highlands Coffee");
            cboHTDL.SelectedIndex = 0;
            
        }

        private void UpdateNavigationPane()
        {
            string[] ndsCaptions = { "THÔNG TIN ĐƠN HÀNG", "THÔNG TIN KHÁCH HÀNG", "THÔNG TIN CT SẢN PHẨM", "THÔNG TIN NHẬP HÀNG", "THÔNG TIN KHUYẾN MẠI", "THÔNG TIN TỒN KHO" };
            string[] ddsCaptions = { "THÔNG TIN KINH DOANH", "DOANH THU THEO THỜI GIAN", "THÔNG TIN KHÁCH HÀNG", "THÔNG TIN SẢN PHẨM", "THÔNG TIN KHU VỰC", "THÔNG TIN CHI NHÁNH" };

            var captions = cboHTDL.SelectedIndex == 0 ? ndsCaptions: ddsCaptions;

            for (int i = 0; i < captions.Length; i++)
            {
                navPane.Pages[i].Caption = captions[i];
            }
        }


        private void cboHTDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTable();
            UpdateNavigationPane();
            
        }

        private void navPane_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {
            GetTable();
            

        }
        #endregion

        #region Tùy chỉnh giao diện GridView
        private void ApplyGridViewAppearance()
        {
            foreach (var gridView in new[] { dgv1, dgv2, dgv3, dgv4, dgv5, dgv6 })
            {
                UI_Helpers.CustomizeGridViewAppearance(gridView);
            }
        }

        #endregion

        #region Hàm xử lý
        public void LamSachDuLieu()
        {
            string thongBaoThanhCong = "";
            string thongBaoLoi = "";

            try
            {
                // Kiểm tra nếu không có checkbox nào được chọn
                if (!KiemTraChonCheckBox())
                {
                    ShowMessage("Thông Báo",
                        "Vui lòng chọn ít nhất một mục để làm sạch dữ liệu.",
                        MessageBoxIcon.Warning);
                    return;
                }

                // Hiển thị WaitForm khi bắt đầu công việc
                WaitFormHelper.ShowWaitForm(splashScreenManager1,"Đang làm sạch dữ liệu, vui lòng chờ...");

                // Xử lý khi cả 2 checkbox đều được chọn
                if (cbAccess.Checked && cbUngDung.Checked)
                {
                    // Làm sạch dữ liệu Access
                    if (cbAccess.Enabled)
                    {
                        // Thực hiện công việc nạp dữ liệu Access
                        if (dt.NapDuLieu(Job_Access))
                        {
                            // Kiểm tra nếu có công việc nào đang chạy
                            if (KiemTra_Status_Job_NapKho())
                            {
                                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang làm sạch dữ liệu Access, vui lòng đợi...");
                            }

                            isChonNap(cbAccess);
                            thongBaoThanhCong += "Làm sạch dữ liệu Access thành công!\n";
                        }
                        else
                        {
                            thongBaoLoi += "Lỗi làm sạch dữ liệu của hệ thống (Access).\n";
                        }
                    }

                    // Làm sạch dữ liệu Ứng dụng
                    if (cbUngDung.Enabled)
                    {
                        // Thực hiện công việc nạp dữ liệu Ứng dụng
                        if (dt.NapDuLieu(Job_HeThong))
                        {
                            // Kiểm tra nếu có công việc nào đang chạy
                            if (KiemTra_Status_Job_NapKho())
                            {
                                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang làm sạch dữ liệu Ứng dụng, vui lòng đợi...");
                            }

                            isChonNap(cbUngDung);
                            thongBaoThanhCong += "Làm sạch dữ liệu Ứng dụng thành công!\n";
                        }
                        else
                        {
                            thongBaoLoi += "Lỗi làm sạch dữ liệu của hệ thống (Ứng dụng).\n";
                        }
                    }
                }
                else if (cbAccess.Checked) // Chỉ xử lý checkbox Access
                {
                    if (dt.NapDuLieu(Job_Access))
                    {
                        // Kiểm tra nếu có công việc nào đang chạy
                        if (KiemTra_Status_Job_NapKho())
                        {
                            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang làm sạch dữ liệu Access, vui lòng đợi...");
                        }

                        isChonNap(cbAccess);
                        thongBaoThanhCong += "Làm sạch dữ liệu Access thành công!\n";
                    }
                    else
                    {
                        thongBaoLoi += "Lỗi làm sạch dữ liệu của hệ thống (Access).\n";
                    }
                }
                else if (cbUngDung.Checked) // Chỉ xử lý checkbox Ứng dụng
                {
                    if (dt.NapDuLieu(Job_HeThong))
                    {
                        // Kiểm tra nếu có công việc nào đang chạy
                        if (KiemTra_Status_Job_NapKho())
                        {
                            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang làm sạch dữ liệu Ứng dụng, vui lòng đợi...");
                        }

                        isChonNap(cbUngDung);
                        thongBaoThanhCong += "Làm sạch dữ liệu Ứng dụng thành công!\n";
                    }
                    else
                    {
                        thongBaoLoi += "Lỗi làm sạch dữ liệu của hệ thống (Ứng dụng).\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Thông Báo",
                    $"Có lỗi xảy ra: {ex.Message}",
                    MessageBoxIcon.Error);
            }
            finally
            {
                // Đảm bảo đóng WaitForm trước khi hiển thị thông báo
                WaitFormHelper.CloseWaitForm(splashScreenManager1);

                // Hiển thị thông báo thành công
                if (!string.IsNullOrEmpty(thongBaoThanhCong))
                {
                    ShowMessage("Thông Báo",
                        thongBaoThanhCong,
                        MessageBoxIcon.Information);
                }

                // Hiển thị thông báo lỗi (nếu có)
                if (!string.IsNullOrEmpty(thongBaoLoi))
                {
                    ShowMessage("Thông Báo",
                        thongBaoLoi,
                        MessageBoxIcon.Error);
                }

                // Kiểm tra trạng thái checkbox và cập nhật trạng thái nút
                if (!KiemTraConCheckBox())
                {
                    SetDisable(btnLamSach);
                    SetEnable(btnNapDDS);
                }
            }
        }
        public void NapKhoDDS()
        {
            bool isSuccess = false;
            string thongBao = "";

            try
            {
                // Kiểm tra trạng thái công việc trước khi thực hiện
                if (KiemTra_Status_Job_NapKho())
                {
                    ShowMessage("Thông Báo",
                        "Một công việc đang thực hiện. Vui lòng đợi cho đến khi công việc hiện tại hoàn tất.",
                        MessageBoxIcon.Warning);
                    return; // Không thực hiện nếu có công việc đang chạy
                }
                // Thực hiện công việc nạp dữ liệu
                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang nạp dữ liệu vào kho , vui lòng chờ...");
                if (dt.NapDuLieu(Job_NapDDS))
                {
                    if (KiemTra_Status_Job_NapKho())
                    {
                        WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang nạp dữ liệu vào kho, vui lòng chờ...");
                    }
                    
                    isSuccess = true; // Đánh dấu thành công
                    thongBao = "Nạp dữ liệu DDS thành công!";
                }
                else
                {
                    thongBao = "Lỗi khi nạp dữ liệu DDS. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                thongBao = $"Đã xảy ra lỗi: {ex.Message}";
            }
            finally
            {
                // Đảm bảo đóng WaitForm bất kể kết quả
                WaitFormHelper.CloseWaitForm(splashScreenManager1);

                // Bật lại nút sau khi công việc hoàn tất
                SetEnable(btnNapDDS);

                // Hiển thị thông báo
                if (!string.IsNullOrEmpty(thongBao))
                {
                    MessageBoxIcon icon = isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error;
                    ShowMessage("Thông Báo", thongBao, icon);
                }
            }
        }

        public void XoaDuLieuKho()
        {
            bool isSuccess = false;
            string thongBao = "";

            try
            {
                // Kiểm tra trạng thái công việc trước khi thực hiện
                if (KiemTra_Status_Job_NapKho())
                {
                    ShowMessage("Thông Báo",
                        "Một công việc đang thực hiện. Vui lòng đợi cho đến khi công việc hiện tại hoàn tất.",
                        MessageBoxIcon.Warning);
                    return; // Không thực hiện nếu có công việc đang chạy
                }

                // Hiển thị WaitForm khi bắt đầu công việc
                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang xóa dữ liệu, vui lòng chờ...");

                // Thực hiện công việc xóa dữ liệu
                if (dt.NapDuLieu(Job_XoaDL))
                {
                    if (KiemTra_Status_Job_NapKho())
                    {
                        WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang xóa dữ liệu, vui lòng chờ...");
                    }

                    isSuccess = true; // Đánh dấu xóa thành công
                    thongBao = "Xóa dữ liệu thành công!";
                }
                else
                {
                    thongBao = "Lỗi khi xóa dữ liệu. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                thongBao = $"Đã xảy ra lỗi: {ex.Message}";
            }
            finally
            {
                // Đảm bảo đóng WaitForm trong mọi trường hợp
                WaitFormHelper.CloseWaitForm(splashScreenManager1);

                // Kích hoạt lại nút xóa dữ liệu sau khi công việc hoàn tất
                SetEnable(btnXoaDL);

                // Hiển thị thông báo kết quả
                if (!string.IsNullOrEmpty(thongBao))
                {
                    MessageBoxIcon icon = isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error;
                    ShowMessage("Thông Báo", thongBao, icon);
                }
            }
        }


        #endregion

        #region Hàm xử lý phụ
        public void isChonNap(Guna2CheckBox check)
        {
            if (check.Enabled == true && check.Checked == true)
            {
                check.Enabled = false;
            }
        }
        private void ShowMessage(string title, string message, MessageBoxIcon icon)
        {
            XtraMessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        public void SetEnable(SimpleButton button)
        {
            button.ForeColor = Color.Black;
            button.Enabled = true;
        }
        private void SetDisable(SimpleButton button)
        {
            button.ForeColor = Color.DarkGray;
            button.Enabled = false;
        }

        private void SetStart()
        {
            cbAccess.Enabled = true;
            cbAccess.Checked = false;
            cbUngDung.Enabled = true;
            cbUngDung.Checked = false;
            SetEnable(btnXoaDL);
            SetEnable(btnLamSach);
            SetEnable(btnRefresh);
            SetEnable(btnNapDDS);
            LoadComboBox();

        }
        //KT còn checkbox chưa chọn hay không
        public bool KiemTraConCheckBox()
        {
            if (cbAccess.Enabled == true && cbAccess.Checked == false)
            {
                return true;
            }
            if (cbUngDung.Enabled == true && cbUngDung.Checked == false)
            {
                return true;
            }
            return false;
        }
        //Kiểm tra chọn checkbox
        public bool KiemTraChonCheckBox()
        {
            if (cbAccess.Enabled == true && cbAccess.Checked == true)
            {
                return true;
            }
            if (cbUngDung.Enabled == true && cbUngDung.Checked == true)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
