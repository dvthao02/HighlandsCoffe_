using DevExpress.XtraEditors;
using FontAwesome.Sharp;
using HighLandsCoffee_Manager.BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmChangePassword : DevExpress.XtraEditors.XtraForm
    {
        private bool isToggled = false;
        BUS_User acc = new BUS_User();
        private string IDDN;
        //string server, database, user, pass;
        public frmChangePassword(string giatrinhan) : this()
        {

            //txtNhanVienID.Text = giatrinhan;
        }
        private void btnHienMK_Click(object sender, EventArgs e)
        {
            if (isToggled)
            {
                btnHienMK.IconChar = IconChar.EyeSlash; // Icon khi chưa nhấn                           
                txtMatKhauMoi.UseSystemPasswordChar = false;
                txtNhapLaiMK.UseSystemPasswordChar = false;
            }
            else
            {
                btnHienMK.IconChar = IconChar.Eye; // Icon khi nhấn
                txtMatKhauMoi.UseSystemPasswordChar = true;
                txtNhapLaiMK.UseSystemPasswordChar = true;
            }

            isToggled = !isToggled;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (txtMatKhauMoi.Text.Length == 0 || txtNhapLaiMK.Text.Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ mật khẩu cần thay đổi!");
                return;
            }
            else
            {
                if (!txtMatKhauMoi.Text.Equals(txtNhapLaiMK.Text))
                {
                    MessageBox.Show("Mật khẩu bạn nhập không trùng khớp!");
                    return;
                }
                else
                {
                    DTO.DTO_NhanVien nv = new DTO.DTO_NhanVien
                    {
                        NhanVienID = txtIDNV.Text,
                        MatKhau = txtMatKhauMoi.Text
                    };

                    if (acc.changePass(nv))
                    {
                        MessageBox.Show("Cập nhật mật khẩu thành công!");
                        this.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại!");
                    }
                }
            }
        }

        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;  // Tắt nút maximize (phóng to)
            this.MinimizeBox = false;
            txtIDNV.Enabled = false;
            txtIDNV.Text = Properties.Settings.Default.madn;

            if (Properties.Settings.Default.quyentk != "User")
            {
                txtIDNV.Text = Properties.Settings.Default.tknv;
                txtMatKhauMoi.Text = txtNhapLaiMK.Text = "123";
            }
        }
    }
}