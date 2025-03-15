using HighLandsCoffee_Manager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.DAO
{
    public class DAO_User
    {
        KetNoiSQL dt = new KetNoiSQL();
        SqlConnection cn;

        //private string HashPassword(string password)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        StringBuilder builder = new StringBuilder();
        //        foreach (byte b in bytes)
        //        {
        //            builder.Append(b.ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}
        public DTO.DTO_NhanVien getInfor(string IDNV)
        {
            cn = dt.conDB();
            DTO.DTO_NhanVien nv = null;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_ThongTinNV", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NhanVienID", IDNV);

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    nv = new DTO.DTO_NhanVien
                    {
                        NhanVienID = rdr["NhanVienID"].ToString(),
                        TenNhanVien = rdr["TenNhanVien"].ToString(),
                        DiaChi = rdr["DiaChi"].ToString(),
                        EMAIL = rdr["Email"].ToString(),
                        SDT = rdr["SDT"].ToString(),
                        ChucVu = rdr["ChucVu"].ToString(),
                        QUYEN = rdr["Quyen"].ToString(),
                        LUONG = rdr["Luong"].ToString()
                        
                    };

                }
                rdr.Close();
                cn.Close();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            return nv;
        }
        public bool Update_NVInfor(DTO_NhanVien ttnv)
        {
            try
            {
                cn.Open();

                string sql = "SP_SUA1NV";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số vào lệnh SQL
                cmd.Parameters.AddWithValue("@NhanVienID", ttnv.NhanVienID);
                cmd.Parameters.AddWithValue("@TenNhanVien", ttnv.TenNhanVien);
                cmd.Parameters.AddWithValue("@DiaChi", ttnv.DiaChi);
                cmd.Parameters.AddWithValue("@Email", ttnv.EMAIL);
                cmd.Parameters.AddWithValue("@SDT", ttnv.SDT);


                cmd.ExecuteNonQuery(); // Thực thi câu lệnh update                   

                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                System.Windows.Forms.MessageBox.Show("Lỗi khi cập nhật thông tin: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool ChangePass(DTO_NhanVien rp)
        {
            cn = dt.conDB();
            //DTO.NhanVien nv = null;
            try
            {
                cn.Open();

                string sql = "SP_DoiMK";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số vào lệnh SQL
                cmd.Parameters.AddWithValue("@NhanVienID", rp.NhanVienID);
                cmd.Parameters.AddWithValue("@MatKhau", rp.MatKhau);


                cmd.ExecuteNonQuery(); // Thực thi câu lệnh update                   

                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                System.Windows.Forms.MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool LockAccount(DTO_NhanVien la)
        {
            cn = dt.conDB();  // Kết nối tới cơ sở dữ liệu
            try
            {
                cn.Open();

                string sql = "sp_LockAccount";  // Thủ tục khóa tài khoản
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NhanVienID", la.NhanVienID);
                cmd.ExecuteNonQuery();

                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                System.Windows.Forms.MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;  // Trả về false nếu có lỗi
            }
        }
        public bool UnLockAccount(DTO_NhanVien ula)
        {
            cn = dt.conDB();  // Kết nối tới cơ sở dữ liệu
            try
            {
                cn.Open();

                string sql = "sp_UnLockAccount";  // Thủ tục mở khóa tài khoản
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NhanVienID", ula.NhanVienID);
                cmd.ExecuteNonQuery();

                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                System.Windows.Forms.MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;  // Trả về false nếu có lỗi
            }
        }
    }
}
