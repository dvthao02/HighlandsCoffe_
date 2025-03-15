using DevExpress.XtraEditors;
using DevExpress.Map.OpenGL;
using HighLandsCoffee_Manager.GUI.ChildForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;


namespace HighLandsCoffee_Manager.GUI
{
    public partial class frmMain : XtraForm
    {
        private SimpleButton currentButton = null; // Nút hiện tại đang được nhấn
        private string quyen;

        public frmMain()
        {
            InitializeComponent();
            this.CenterToScreen();
            setButton(); // Thiết lập trạng thái ban đầu cho các nút
        }
        public frmMain(string giatrinhan, string quyen) : this()
        {
            this.quyen = quyen;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị trang chủ khi form chính tải lên
            ShowChildForm(new frmHome());
            InitializeButtonEvents();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void InitializeButtonEvents()
        {
            // Gán sự kiện Click cho các nút
            btnTrangChu.Click += (s, e) => ActivateButton(s, new frmHome());
            btnNapDuLieu.Click += (s, e) => ActivateButton(s, new frmNapDuLieu());
            btnPhanTich.Click += (s, e) => ActivateButton(s, new frmPhanTichDuLieu());
            btnThongKe.Click += (s, e) => ActivateButton(s, new frmThongKeDuLieu());

            btnSaoLuu.Click += (s, e) => ActivateButton(s, new frmBackUp_Restore());
            btnDangXuat.Click += (s, e) => Application.Exit();

            // Gán sự kiện hover cho các nút
            foreach (Control control in this.Controls)
            {
                if (control is SimpleButton button)
                {
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;
                }
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is SimpleButton button && button != currentButton)
            {
                // Đổi màu hoặc hiệu ứng khi chuột di vào nút
                button.Appearance.BackColor = CustomColors.HoverColor;
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is SimpleButton button && button != currentButton)
            {
                // Phục hồi màu khi chuột rời khỏi nút
                button.Appearance.BackColor = CustomColors.DefaultColor;
            }
        }

        private void ActivateButton(object sender, Form childForm)
        {
            if (sender is SimpleButton button)
            {
                // Nếu có nút đang được chọn, khôi phục lại màu sắc của nút trước đó
                if (currentButton != null)
                {
                    SetButtonAppearance(currentButton); // Khôi phục trạng thái mặc định cho nút trước đó
                }
                // Thiết lập màu cho nút được chọn
                currentButton = button;
                currentButton.Appearance.BackColor = CustomColors.ActiveColor;
                currentButton.Appearance.ForeColor = CustomColors.DefaultTextColor;

                // Đặt RightToLeft cho nút đã chọn
                currentButton.RightToLeft = RightToLeft.Yes;

                // Hiển thị form con
                ShowChildForm(childForm);
            }
        }

        private void ShowChildForm(Form childForm)
        {
            panel_main.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panel_main.Controls.Add(childForm);
            panel_main.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void setButton()
        {
            // Thiết lập các thuộc tính giao diện của các nút bằng Appearance
            SetButtonAppearance(btnTrangChu);
            SetButtonAppearance(btnNapDuLieu);
            SetButtonAppearance(btnPhanTich);
            SetButtonAppearance(btnThongKe);
            SetButtonAppearance(btnKhaiPha);
            SetButtonAppearance(btnQuanTri);
            SetButtonAppearance(btnSaoLuu);
            SetButtonAppearance(btnDangXuat);
        }

        private void SetButtonAppearance(SimpleButton button)
        {
            // Cài đặt các thuộc tính của button thông qua Appearance
            button.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            button.Appearance.ForeColor = CustomColors.DefaultTextColor;
            button.Appearance.BackColor = CustomColors.DefaultColor;
            button.RightToLeft = RightToLeft.No;
        }

        private struct CustomColors
        {
            public static Color DefaultColor = ColorTranslator.FromHtml("#7A1818"); 
            public static Color HoverColor = ColorTranslator.FromHtml("#A83232"); 
            public static Color ActiveColor = ColorTranslator.FromHtml("#C87A19"); 
            public static Color DefaultTextColor = Color.White; 
        }

        private void btnQuanTri_Click(object sender, EventArgs e)
        {

            if (quyen == "Admin")
            {
                ShowChildForm(new frmAdministration());
            }
            else
            {
                // Mở form Cá Nhân
                ShowChildForm(new Account_Info());
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn đăng xuất không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rs == DialogResult.Yes)
            {
                frmDangNhap lg = new frmDangNhap();
                this.Visible = false;
                lg.ShowDialog();

            }
        }

        private void btnSaoLuu_Click(object sender, EventArgs e)
        {
            ShowChildForm(new frmBackUp_Restore());
        }
    }
}
