using DevExpress.XtraEditors;
using HighLandsCoffee_Manager.BUS;
using HighLandsCoffee_Manager.DTO;
using HighLandsCoffee_Manager.Helper;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.GUI
{
    public partial class frmKetNoiDB : DevExpress.XtraEditors.XtraForm
    {
        public frmKetNoiDB()
        {
            InitializeComponent();
        }

        private void frmKetNoiDB_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            LoadTTServer();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;  // Tắt nút maximize (phóng to)
            this.MinimizeBox = false;
        }

        BUS_ConnectDB dt = new BUS_ConnectDB();

        // Load thông tin server
        public void LoadTTServer()
        {
            try
            {
                string line = "";
                // Đường dẫn đến file lưu tên server
                string path = System.Windows.Forms.Application.StartupPath + "\\ServerName.txt";
                StreamReader sr = new StreamReader(path, Encoding.UTF8);

                // Đọc file và gán giá trị vào các TextBox
                while ((line = sr.ReadLine()) != null)
                {
                    txtServer.Text = line;
                    line = sr.ReadLine();  // Qua dòng mới
                    txtUser.Text = line;
                    line = sr.ReadLine();  // Qua dòng mới
                    txtPass.Text = line;
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            DTO_ConnectDB dTO_KetNoi = new DTO_ConnectDB(txtServer.Text, txtDatabase.Text, txtUser.Text, txtPass.Text);

            // Hiển thị WaitForm khi bắt đầu kiểm tra kết nối
            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang kiểm tra kết nối cơ sở dữ liệu...");

            if (dt.kTRong(dTO_KetNoi))
            {
                // Lưu lại thông tin tài khoản và server
                if (dt.luuTaiKhoan(dTO_KetNoi))
                {
                    // Kiểm tra kết nối đến cơ sở dữ liệu
                    if (dt.kTKetNoi())
                    {
                        // Kết nối thành công
                        WaitFormHelper.CloseWaitForm(splashScreenManager1);
                        MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Chuyển sang form Login
                        frmDangNhap loginForm = new frmDangNhap();
                        loginForm.Show();

                        // Đóng form kết nối sau khi chuyển sang form Login
                        this.Hide();
                    }
                    else
                    {
                        // Kết nối thất bại
                        WaitFormHelper.CloseWaitForm(splashScreenManager1);
                        MessageBox.Show("Kết nối thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtServer.Focus();
                    }
                }
                else
                {
                    // Lưu thông tin thất bại
                    WaitFormHelper.CloseWaitForm(splashScreenManager1);
                    MessageBox.Show("Lưu thông tin thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Thông báo thiếu thông tin
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
                MessageBox.Show("Chưa nhập đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnVeDangNhap_Click(object sender, EventArgs e)
        {
            frmDangNhap loginForm = new frmDangNhap();
            loginForm.Show();
            this.Hide();
        }
    }
}
