using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using HighLandsCoffee_Manager.GUI.ChildForm;
using HighLandsCoffee_Manager.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.GUI
{
    public partial class frmChuongTrinh : DevExpress.XtraEditors.XtraForm
    {
        private string quyen;

        // Biến lưu trữ nút đang active
        private SimpleButton currentButton = null;

        #region Khởi tạo form và sự kiện load
        public frmChuongTrinh()
        {
            InitializeComponent();
        }

        public frmChuongTrinh(string giatrinhan, string quyen) : this()
        {
            this.quyen = quyen;
        }

        private void frmChuongTrinh_Load(object sender, EventArgs e)
        {
            // Hiển thị SplashScreen
            WaitFormHelper.ShowWaitForm(splashScreenManager1);

            // Thiết lập giao diện
            SetupForm();

            // Đóng SplashScreen
            WaitFormHelper.CloseWaitForm(splashScreenManager1);
        }

        private void SetupForm()
        {
            // Kiểm tra quyền và thiết lập quyền của người dùng
            btnQuanTri.Enabled = quyen == "Admin";

            // Hiển thị tên tài khoản người dùng
            btnThongTinTK.Text = $"{Properties.Settings.Default.quyentk}:{Properties.Settings.Default.madn}";

            // Đặt lại giao diện cho tất cả các nút (Màu sắc mặc định)
            ResetAllButtonsAppearance(this.Controls);

            // Hiển thị form con mặc định
            ShowChildForm(new frmHome());

            // Thiết lập cửa sổ ở chế độ full màn hình
            this.WindowState = FormWindowState.Maximized;
        }

        // Hàm đệ quy để duyệt qua tất cả các nút trong form và các control con
        private void ResetAllButtonsAppearance(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is SimpleButton btn)
                {
                    // Đặt lại giao diện nút mặc định
                    UI_Helpers.ResetButtonAppearance(btn);

                    // Thêm sự kiện cho nút hover
                    btn.MouseEnter += (sender, e) => UI_Helpers.ApplyMainButtonHover(btn);
                    btn.MouseLeave += (sender, e) => UI_Helpers.ResetButtonAppearance(btn); // Đặt lại khi chuột rời
                }
                else if (control.HasChildren)
                {
                    ResetAllButtonsAppearance(control.Controls); // Đệ quy với các control con
                }
            }
        }

        #endregion

        #region Hiển thị form con
        private void ShowChildFormWithWaitForm(Form childForm)
        {
            // Hiển thị màn hình đợi khi chuyển đổi form
            WaitFormHelper.ShowWaitForm(splashScreenManager1);

            // Ẩn hoặc đóng form cũ nếu có
            foreach (Form openForm in this.MdiChildren)
            {
                openForm.Close();
            }

            // Hiển thị form con
            ShowChildForm(childForm);

            // Đóng màn hình đợi sau khi form đã tải xong
            WaitFormHelper.CloseWaitForm(splashScreenManager1);
        }

        private void ShowChildForm(Form childForm)
        {
            panel_main.Controls.Clear();

            childForm.TopLevel = false; // Đặt form con không phải là top-level
            childForm.FormBorderStyle = FormBorderStyle.None; // Xóa border
            childForm.Dock = DockStyle.Fill; // Căn chỉnh form con khớp với panel

            panel_main.Controls.Add(childForm); // Thêm form con vào panel
            panel_main.Tag = childForm; // Lưu thông tin form con
            childForm.BringToFront(); // Đưa form con lên trước
            childForm.Show(); // Hiển thị form con

            // Áp dụng style cho form con
            UI_Helpers.ApplyChildFormStyle(childForm);
        }
        #endregion

        #region Sự kiện nút nhấn để mở form
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmHome());
        }

        private void btnNapDuLieu_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmNapDuLieu());
        }

        private void btnPhanTich_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmPhanTichDuLieu());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmThongKeDuLieu());
        }

        private void btnKhaiPha_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmKhaiPha());
        }

        private void btnQuanTri_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmQuanTri());
        }

        private void btnSaoLuu_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmSaoLuuPhuchoi());
        }

        private void btnThongTinTK_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender, new frmThongTinTaiKhoan());
        }
        #endregion

        #region Sự kiện đăng xuất và đóng form
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult rs = XtraMessageBox.Show("Bạn có muốn đăng xuất không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rs == DialogResult.Yes)
            {
                frmDangNhap lg = new frmDangNhap();
                this.Hide(); // Đảm bảo form hiện tại ẩn đi
                lg.ShowDialog();
                this.Close(); // Đảm bảo form này sẽ đóng sau khi đăng xuất
            }
        }

        private void frmChuongTrinh_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        // Hàm chung xử lý việc nhấn nút và hiển thị form con
        private void HandleButtonClick(object sender, Form childForm)
        {
            // Đặt lại giao diện cho tất cả các nút trước khi kích hoạt nút hiện tại
            ResetAllButtonsAppearance(this.Controls);

            // Áp dụng màu active cho nút hiện tại
            SimpleButton button = (SimpleButton)sender;

            // Kiểm tra nếu nút hiện tại không phải là nút đã active, reset nút trước đó và active nút mới
            if (currentButton != button)
            {
                UI_Helpers.ActivateButton(button);
                if (currentButton != null)
                {
                    UI_Helpers.ResetButtonAppearance(currentButton);
                }
                currentButton = button;
            }

            // Hiển thị form con
            ShowChildFormWithWaitForm(childForm);
        }
    }
}
