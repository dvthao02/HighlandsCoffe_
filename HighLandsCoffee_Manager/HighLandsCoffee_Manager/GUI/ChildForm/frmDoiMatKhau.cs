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
    public partial class frmDoiMatKhau : DevExpress.XtraEditors.XtraForm
    {
        private bool isToggled = false;
        BUS_User acc = new BUS_User();

        // Biến để lưu NhanVienID, có thể nhận từ form đăng nhập hoặc DataGridView
        private string NhanVienID { get; set; }

        // Khởi tạo form với tham số NhanVienID từ các form khác
        public frmDoiMatKhau(string nhanVienID) : this()
        {
            // Lưu giá trị NhanVienID truyền vào
            this.NhanVienID = nhanVienID;
        }

        // Khởi tạo form mặc định
        public frmDoiMatKhau()
        {
            InitializeComponent();
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;  // Tắt nút maximize (phóng to)
            this.MinimizeBox = false;
            txtIDNV.Enabled = false;

            // Kiểm tra nếu NhanVienID có giá trị, thì hiển thị trên form
            if (!string.IsNullOrEmpty(this.NhanVienID))
            {
                txtIDNV.Text = this.NhanVienID;
            }
            else
            {
                // Nếu không có giá trị, lấy mặc định từ Settings hoặc từ nguồn khác (ví dụ người dùng đang đăng nhập)
                txtIDNV.Text = Properties.Settings.Default.madn;
            }

            // Cài đặt mật khẩu mặc định (có thể xóa dòng này khi triển khai thực tế)
            txtMatKhauMoi.Text = txtNhapLaiMK.Text = "123";
        }

        // Xử lý khi click nút "Hiện mật khẩu"
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

        // Xử lý khi click nút "Cập nhật mật khẩu"
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (txtMatKhauMoi.Text.Length == 0 || txtNhapLaiMK.Text.Length == 0)
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ mật khẩu cần thay đổi!");
                return;
            }
            else
            {
                if (!txtMatKhauMoi.Text.Equals(txtNhapLaiMK.Text))
                {
                    XtraMessageBox.Show("Mật khẩu bạn nhập không trùng khớp!");
                    return;
                }
                else
                {
                    DTO.DTO_NhanVien nv = new DTO.DTO_NhanVien
                    {
                        NhanVienID = txtIDNV.Text,
                        MatKhau = txtMatKhauMoi.Text
                    };

                    // Kiểm tra nếu thông tin mật khẩu thay đổi thành công
                    if (acc.changePass(nv))
                    {
                        XtraMessageBox.Show("Cập nhật mật khẩu thành công!");
                        this.Visible = false;
                    }
                    else
                    {
                        XtraMessageBox.Show("Cập nhật thất bại!");
                    }
                }
            }
        }
    }
}
