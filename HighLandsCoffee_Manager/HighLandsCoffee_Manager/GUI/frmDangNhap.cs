using DevExpress.XtraEditors;
using HighLandsCoffee_Manager.BUS;
using HighLandsCoffee_Manager.DTO;
using HighLandsCoffee_Manager.Helpers;  // Thêm namespace cho WaitFormHelper và NavButtonHelpers
using System;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.GUI
{
    public partial class frmDangNhap : DevExpress.XtraEditors.XtraForm
    {
        private BUS_Login busLogin = new BUS_Login();

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            CauHinhForm();
        }

        private void CauHinhForm()
        {
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Xử lý sự kiện phím Enter để di chuyển qua các TextBox hoặc thực hiện đăng nhập
            txtTaiKhoan.KeyDown += Txt_KeyDown;
            txtMatKhau.KeyDown += Txt_KeyDown;
        }

        // Xử lý sự kiện khi nhấn phím Enter
        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap.PerformClick(); // Gọi sự kiện đăng nhập khi nhấn Enter
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            UI_Helpers.ActivateButton((SimpleButton)sender); // Kích hoạt giao diện nút
            if (string.IsNullOrWhiteSpace(txtTaiKhoan.Text) || string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                XtraMessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu thiếu thông tin
            }

            // Hiển thị WaitForm khi kiểm tra kết nối
            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang kiểm tra kết nối...");

            string errorMessage = string.Empty;
            if (!KiemTraKetNoi(ref errorMessage))
            {
                WaitFormHelper.CloseWaitForm(splashScreenManager1); // Đóng WaitForm khi có lỗi
                XtraMessageBox.Show(errorMessage, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WaitFormHelper.CloseWaitForm(splashScreenManager1); // Đóng WaitForm khi hoàn tất kiểm tra kết nối
            ThucHienDangNhap(ref errorMessage);
        }

        private bool KiemTraKetNoi(ref string errorMessage)
        {
            return busLogin.KiemTraKetNoi(ref errorMessage);
        }

        private void ThucHienDangNhap(ref string errorMessage)
        {
            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang đăng nhập, vui lòng chờ...");
            DTO_Login taiKhoan = new DTO_Login(txtTaiKhoan.Text, txtMatKhau.Text);
            string ketQuaDangNhap = busLogin.DangNhap(taiKhoan, ref errorMessage); // Nhận kết quả từ DAO_Login

            WaitFormHelper.CloseWaitForm(splashScreenManager1); // Đóng WaitForm khi hoàn tất đăng nhập

            if (string.IsNullOrEmpty(ketQuaDangNhap))
            {
                XtraMessageBox.Show(errorMessage, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ketQuaDangNhap.Contains("Tài khoản") || ketQuaDangNhap.Contains("Lỗi"))
            {
                // Nếu có thông báo lỗi hoặc tài khoản không chính xác
                XtraMessageBox.Show(ketQuaDangNhap, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                XuLyDangNhapThanhCong(ketQuaDangNhap); // Xử lý đăng nhập thành công
            }
        }

        private void XuLyDangNhapThanhCong(string quyen)
        {
            XtraMessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Properties.Settings.Default.madn = txtTaiKhoan.Text;
            Properties.Settings.Default.quyentk = quyen;
            Properties.Settings.Default.Save();

            txtTaiKhoan.Clear();
            txtMatKhau.Clear();

            this.Hide(); // Ẩn form đăng nhập

            using (frmChuongTrinh mainForm = new frmChuongTrinh(txtTaiKhoan.Text, quyen))
            {
                mainForm.ShowDialog(); // Mở form chính
            }

        }

        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            UI_Helpers.ActivateButton((SimpleButton)sender); // Kích hoạt giao diện nút
            HienThiFormKetNoi();
        }

        private void HienThiFormKetNoi()
        {
            this.Hide(); // Ẩn form đăng nhập

            using (frmKetNoiServer connectServer = new frmKetNoiServer())
            {
                connectServer.ShowDialog(); // Mở form kết nối server
            }

            this.Show(); // Hiển thị lại form đăng nhập sau khi frmKetNoiServer đóng
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Thoát ứng dụng khi đóng form đăng nhập
        }
    }
}
