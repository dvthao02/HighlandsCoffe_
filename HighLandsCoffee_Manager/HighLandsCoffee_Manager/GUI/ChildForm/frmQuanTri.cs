using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HighLandsCoffee_Manager.BUS;
using HighLandsCoffee_Manager.DTO;
using static DevExpress.XtraEditors.Mask.MaskSettings;
using DevExpress.Office.PInvoke;
using Guna.UI2.WinForms;
using HighLandsCoffee_Manager.Helpers;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmQuanTri : DevExpress.XtraEditors.XtraForm
    {
        private string quyen;
        BUS_NhanVien dsnv = new BUS_NhanVien();
        BUS_User acc = new BUS_User();

        private void frmQuanTri_Load(object sender, EventArgs e)
        {
            quyen = Properties.Settings.Default.quyentk;
            dgv_danhSachNV.DataSource = dsnv.getDanhSachNhanVien(quyen);
            LoadDuLieu();
            dgv_danhSachNV.ClearSelection();
            btnThem.Enabled = false;
            txtIDNV.Enabled = false;
            txtLuong.Enabled = false;
            UI_Helpers.CustomizeGridViewAppearance(dgv_danhSachNV);
        }
        public void LoadTextBox()
        {
            if (dgv_danhSachNV.Columns.Contains("NhanVienID"))
            {
                // Thiết lập tên các cột theo tên cột trong thủ tục
                dgv_danhSachNV.Columns[0].HeaderText = "Mã Nhân Viên";
                dgv_danhSachNV.Columns[1].HeaderText = "Tên Nhân Viên";
                dgv_danhSachNV.Columns[2].HeaderText = "Địa Chỉ";
                dgv_danhSachNV.Columns[3].HeaderText = "Email";
                dgv_danhSachNV.Columns[4].HeaderText = "Số Điện Thoại";
                dgv_danhSachNV.Columns[5].HeaderText = "Chức Vụ";
                dgv_danhSachNV.Columns[6].HeaderText = "Quyền";
                dgv_danhSachNV.Columns[7].HeaderText = "Lương";
                dgv_danhSachNV.Columns[8].HeaderText = "Trạng Thái";

                dgv_danhSachNV.Refresh();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có trường nào bị bỏ trống không
            if (txtTenNV.Text.Length == 0 || txtDiaChi.Text.Length == 0 || txtEmail.Text.Length == 0 || txtSDT.Text.Length == 0 || cboChucVu.Text.Length == 0 || cboQuyen.SelectedText == null)
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ thông tin bắt buộc!");
                return;
            }
            else
            {
                // Kiểm tra số điện thoại
                if (txtSDT.Text.Length != 10 || !txtSDT.Text.All(char.IsDigit))
                {
                    XtraMessageBox.Show("Số điện thoại phải gồm 10 chữ số!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra email
                if (!txtEmail.Text.Contains("@highlands.com"))
                {
                    XtraMessageBox.Show("Email phải có dạng @highlands.com!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hiển thị thông báo xác nhận
                if (XtraMessageBox.Show("Bạn có muốn Thêm mới nhân viên", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    DTO_NhanVien lnv = new DTO_NhanVien(txtIDNV.Text, txtTenNV.Text, txtDiaChi.Text, txtEmail.Text, txtSDT.Text, cboChucVu.Text, cboQuyen.Text, txtLuong.Text);

                    // Thêm
                    if (dsnv.themNV(lnv)) //thành công
                    {
                        XtraMessageBox.Show("Thêm thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgv_danhSachNV.DataSource = dsnv.getDanhSachNhanVien(quyen);
                        btnThem.Enabled = false;
                        return;
                    }
                    else //thất bại
                    {
                        XtraMessageBox.Show("Thêm thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void XuLyTxt()
        {
            dgv_danhSachNV.ClearSelection();
            txtIDNV.Clear();
            txtTenNV.Text = string.Empty;
            txtSDT.ResetText();
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            cboChucVu.Text = "";
            txtTimKiem.Text = "";
            cboQuyen.Text = "";
            txtLuong.Text = "";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtIDNV.Enabled = true;
            btnThem.Enabled = true;
            XuLyTxt();

            txtIDNV.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra tài khoản đăng nhập hiện tại có trùng với dữ liệu đang chọn
            if (string.Equals(txtIDNV.Text, Properties.Settings.Default.madn, StringComparison.OrdinalIgnoreCase))
            {
                XtraMessageBox.Show("Bạn không thể xoá dữ liệu của mình!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                DTO_NhanVien lnv = new DTO_NhanVien(txtIDNV.Text, txtTenNV.Text, txtDiaChi.Text, txtEmail.Text, txtSDT.Text,
               cboChucVu.Text, cboQuyen.Text, txtLuong.Text);

                //Hiển thị thông báo xác nhận
                if (XtraMessageBox.Show("Bạn có muốn xóa?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //xóa
                    if (dsnv.xoaNV(lnv))//thành công
                    {
                        dgv_danhSachNV.DataSource = dsnv.getDanhSachNhanVien(quyen);
                        XtraMessageBox.Show("Xóa thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else//thất bại
                    {
                        XtraMessageBox.Show("Xóa thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (txtTenNV.Text.Length == 0 || txtDiaChi.Text.Length == 0 || txtEmail.Text.Length == 0 || txtSDT.Text.Length == 0 || cboChucVu.Text.Length == 0 || cboQuyen.SelectedItem == null || txtLuong.Text.Length == 0)
            {
                XtraMessageBox.Show("Bạn chưa nhập đầy đủ thông tin bắt buộc!");
                return;
            }
            else
            {
                // Kiểm tra số điện thoại
                if (txtSDT.Text.Length != 10 || !txtSDT.Text.All(char.IsDigit))
                {
                    XtraMessageBox.Show("Số điện thoại phải gồm 10 chữ số!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra email
                if (!txtEmail.Text.Contains("@highlands.com"))
                {
                    XtraMessageBox.Show("Email phải có dạng @highlands.com!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.Equals(txtIDNV.Text, Properties.Settings.Default.madn, StringComparison.OrdinalIgnoreCase))
                {
                    if (Properties.Settings.Default.quyentk == "Admin")
                    {
                        // Nếu người dùng là Admin và cố gắng sửa quyền của chính mình
                        if (cboQuyen.SelectedItem == "User")
                        {
                            XtraMessageBox.Show("Bạn không thể chỉnh sửa quyền của chính mình!");
                        }
                        else
                        {
                            // Tạo DTO
                            DTO_NhanVien lnv = new DTO_NhanVien(txtIDNV.Text, txtTenNV.Text, txtDiaChi.Text, txtEmail.Text, txtSDT.Text,
                               cboChucVu.Text, cboQuyen.Text, txtLuong.Text);

                            // Hiển thị thông báo xác nhận
                            if (XtraMessageBox.Show("Bạn có muốn cập nhật?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {


                                //sửa
                                if (dsnv.suaNV(lnv)) //thành công
                                {
                                    XtraMessageBox.Show("Cập nhật thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dgv_danhSachNV.DataSource = dsnv.getDanhSachNhanVien(quyen);

                                    return;
                                }
                                else//thất bại
                                {
                                    XtraMessageBox.Show("Cập nhật thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Tạo DTO
                    DTO_NhanVien lnv = new DTO_NhanVien(txtIDNV.Text, txtTenNV.Text, txtDiaChi.Text, txtEmail.Text, txtSDT.Text,
                       cboChucVu.Text, cboQuyen.Text, txtLuong.Text);

                    // Hiển thị thông báo xác nhận
                    if (XtraMessageBox.Show("Bạn có muốn cập nhật?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //sửa
                        if (dsnv.suaNV(lnv)) //thành công
                        {
                            XtraMessageBox.Show("Cập nhật thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgv_danhSachNV.DataSource = dsnv.getDanhSachNhanVien(quyen);

                            return;
                        }
                        else//thất bại
                        {
                            XtraMessageBox.Show("Cập nhật thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }

        private void dgv_danhSachNV_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_danhSachNV.CurrentRow != null)
            {
                LoadTextBox();
                dgv_danhSachNV.ReadOnly = true;
                dgv_danhSachNV.AllowUserToAddRows = false;

            }

            if (dgv_danhSachNV.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_danhSachNV.SelectedRows[0];
                txtIDNV.Text = selectedRow.Cells["NhanVienID"].Value.ToString();
                txtTenNV.Text = selectedRow.Cells["TenNhanVien"].Value.ToString();
                txtDiaChi.Text = selectedRow.Cells["DiaChi"].Value.ToString();
                txtEmail.Text = selectedRow.Cells["Email"].Value.ToString();
                txtSDT.Text = selectedRow.Cells["SDT"].Value.ToString();
                cboChucVu.Text = selectedRow.Cells["ChucVu"].Value.ToString();
                cboQuyen.Text = selectedRow.Cells["Quyen"].Value.ToString();
                txtLuong.Text = selectedRow.Cells["Luong"].Value.ToString();
            }
        }

        private void cboQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem quyền được chọn là "Admin" hay "User"
            if (cboQuyen.SelectedItem.ToString() == "Admin")
            {
                // Nếu quyền là "Admin", chỉ hiển thị "Quản Lý" trong cboChucVu
                cboChucVu.Properties.Items.Clear();
                cboChucVu.Properties.Items.Add("Quản Lý");
            }
            else if (cboQuyen.SelectedItem.ToString() == "User")
            {
                // Nếu quyền là "User", hiển thị "Trợ Lý" và "Nhân Viên" trong cboChucVu
                cboChucVu.Properties.Items.Clear();
                cboChucVu.Properties.Items.Add("Trợ Lý");
                cboChucVu.Properties.Items.Add("Nhân Viên");
            }
        }

        private void cboChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChucVu.SelectedItem.ToString() == "Quản Lý")
            {
                // Nếu quyền là "Admin", chỉ hiển thị "Quản Lý" trong cboChucVu
                txtLuong.Clear();
                txtLuong.Text = "12000000";
            }
            else if (cboChucVu.SelectedItem.ToString() == "Trợ Lý")
            {
                // Nếu quyền là "Admin", chỉ hiển thị "Quản Lý" trong cboChucVu
                txtLuong.Clear();
                txtLuong.Text = "8000000";
            }
            else if (cboChucVu.SelectedItem.ToString() == "Nhân Viên")
            {
                // Nếu quyền là "Admin", chỉ hiển thị "Quản Lý" trong cboChucVu
                txtLuong.Clear();
                txtLuong.Text = "5000000";
            }
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            if (dgv_danhSachNV.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn trong DataGridView
                DataGridViewRow selectedRow = dgv_danhSachNV.SelectedRows[0];

                // Lấy giá trị NhanVienID từ cột tương ứng trong DataGridView
                string nhanVienID = selectedRow.Cells["NhanVienID"].Value.ToString();

                // Lưu thông tin NhanVienID vào Settings (nếu cần thiết)
                Properties.Settings.Default.tknv = nhanVienID;
                Properties.Settings.Default.Save();

                // Mở form đổi mật khẩu và truyền NhanVienID
                frmDoiMatKhau dmk = new frmDoiMatKhau(nhanVienID);  // Truyền NhanVienID vào form
                dmk.Show();
            }
            else
            {
                XtraMessageBox.Show("Vui lòng chọn một nhân viên.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            //// sử dụng thuộc tính RowFilter để tìm kiếm theo tên
            //string s1 = string.Format("NhanVienID like '{0}'", "*" + txtTimKiem.Text + "*"); // NhanVienID
            //string s2 = string.Format("TenNhanVien like '{0}'", "*" + txtTimKiem.Text + "*"); // Tên Nhân Viên
            //string rowFilter = "(" + s1 + ")" + "or" + "(" + s2 + ")";
            //(dgv_danhSachNV.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
        }

        private void btnKhoaTK_Click(object sender, EventArgs e)
        {
            if (txtIDNV.Text.Length == 0)
            {
                XtraMessageBox.Show("Vui lòng chọn nhân viên cần khóa!");
                return;
            }
            else
            {
                DTO.DTO_NhanVien nv = new DTO.DTO_NhanVien
                {
                    NhanVienID = txtIDNV.Text,
                };
                if (acc.lockAccount(nv))
                {
                    XtraMessageBox.Show("Cập nhật trạng thái thành công!");
                    dgv_danhSachNV.DataSource = dsnv.getDanhSachNhanVien(quyen);
                }
            }
        }

        private void btnMoKhoaTK_Click(object sender, EventArgs e)
        {
            if (txtIDNV.Text.Length == 0)
            {
                XtraMessageBox.Show("Vui lòng chọn nhân viên cần khóa!");
                return;
            }
            else
            {
                DTO.DTO_NhanVien nv = new DTO.DTO_NhanVien
                {
                    NhanVienID = txtIDNV.Text,
                };
                if (acc.unlockAccount(nv))
                {
                    XtraMessageBox.Show("Cập nhật trạng thái thành công!");
                    dgv_danhSachNV.DataSource = dsnv.getDanhSachNhanVien(quyen);
                }
            }
        }

        public frmQuanTri()
        {
            InitializeComponent();
        }
        public frmQuanTri(string giatrinhan, string quyen) : this()
        {

        }
        private void LoadDuLieu()
        {
            // Đặt chế độ chọn chỉ từ danh sách cho ComboBoxEdit
            cboQuyen.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            // Thêm các lựa chọn "Admin" và "User" vào ComboBoxEdit
            string[] s2 = new string[] { "Admin", "User" };
            foreach (var item in s2)
            {
                cboQuyen.Properties.Items.Add(item);
            }

            cboQuyen.SelectedIndex = 0; // Đặt lựa chọn mặc định là "Admin"
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string s1 = string.Format("NhanVienID like '{0}'", "*" + txtTimKiem.Text + "*");
            string s2 = string.Format("TenNhanVien like '{0}'", "*" + txtTimKiem.Text + "*");
            string rowFilter = "(" + s1 + ")" + "or" + "(" + s2 + ")";
            (dgv_danhSachNV.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
        }
    }
}