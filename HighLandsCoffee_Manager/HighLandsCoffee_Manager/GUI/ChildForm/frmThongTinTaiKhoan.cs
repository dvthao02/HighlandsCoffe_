using DevExpress.XtraEditors;
using HighLandsCoffee_Manager.BUS;
using HighLandsCoffee_Manager.Helpers;
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
    public partial class frmThongTinTaiKhoan : DevExpress.XtraEditors.XtraForm
    {
        BUS_User acc = new BUS_User();
        private string IDDN;
        public frmThongTinTaiKhoan()
        {
            InitializeComponent();
            IDDN = Properties.Settings.Default.madn;
        }

        private void frmThongTinNhanVien_Load(object sender, EventArgs e)
        {
            Load1NV();
            txtIDNhanVien.Enabled = false;
            txtChucVu.Enabled = false;
            txtQuyen.Enabled = false;
            txtLuong.Enabled = false;
        }
         
      

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Load1NV();
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            // Lấy thông tin ID nhân viên từ textbox (hoặc nơi bạn lưu thông tin người dùng đăng nhập)
            string nhanVienID = txtIDNhanVien.Text;

            // Kiểm tra nếu ID không rỗng, mở form đổi mật khẩu và truyền giá trị vào
            if (!string.IsNullOrEmpty(nhanVienID))
            {
                frmDoiMatKhau dmk = new frmDoiMatKhau(nhanVienID);  // Truyền NhanVienID vào form
                dmk.Show();  // Hiển thị form đổi mật khẩu
            }
            else
            {
                XtraMessageBox.Show("Không tìm thấy thông tin nhân viên.");
            }
        }


        private void btnCapNhatTT_Click(object sender, EventArgs e)
        {
            if (txtTenNV.Text.Length == 0 || txtDiaChi.Text.Length == 0 || txtEmail.Text.Length == 0 || txtSoDT.Text.Length == 0 || txtChucVu.Text.Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin bắt buộc");
                return;
            }
            else
            {
                // Kiểm tra số điện thoại
                if (txtSoDT.Text.Length != 10 || !txtSoDT.Text.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại phải gồm 10 chữ số!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra email
                if (!txtEmail.Text.Contains("@highlands.com"))
                {
                    MessageBox.Show("Email phải có dạng @highlands.com!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Bạn có muốn sửa thông tin cá nhân?", "Thông Báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    DTO.DTO_NhanVien nv = new DTO.DTO_NhanVien
                    {
                        NhanVienID = txtIDNhanVien.Text,
                        TenNhanVien = txtTenNV.Text,
                        DiaChi = txtDiaChi.Text,
                        EMAIL = txtEmail.Text,
                        SDT = txtSoDT.Text,
                        ChucVu = txtChucVu.Text
                    };

                    if (acc.update_NVInfor(nv))
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thông tin thất bại!");
                    }

                }
            }
        }


        private void Load1NV()
        {
            txtIDNhanVien.Text = IDDN;

            DTO.DTO_NhanVien nv = acc.GetThongTinNV(IDDN);

            if (nv != null)
            {
                txtTenNV.Text = nv.TenNhanVien;
                txtDiaChi.Text = nv.DiaChi;
                txtEmail.Text = nv.EMAIL;
                txtSoDT.Text = nv.SDT;
                txtChucVu.Text = nv.ChucVu;
                txtQuyen.Text = nv.QUYEN;
                txtLuong.Text = nv.LUONG;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên với ID này.");
            }
        }
    }
}