using HighLandsCoffee_Manager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HighLandsCoffee_Manager.DAO
{
    public class TaiKhoan
    {
        KetNoi con = new KetNoi();
        SqlConnection cn;

        public string getAccount(NhanVien acc)
        {
            cn = con.getConnection();
            try
            {
                cn.Open();
                string SQL = "EXEC sp_DangNhap @NhanVienID, @MatKhau";
                SqlCommand cmd = new SqlCommand(SQL, cn) { CommandType = CommandType.Text };

                cmd.Parameters.AddWithValue("@NhanVienID", acc.NhanVienID);
                cmd.Parameters.AddWithValue("@MatKhau", acc.MatKhau);

                SqlDataReader ad = cmd.ExecuteReader();

                if (ad.Read())
                {
                    string quyen = ad["Quyen"].ToString();
                    cn.Close();
                    return quyen;
                }


                cn.Close();
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DTO.NhanVien getInfor(string IDNV)
        {
            cn = con.getConnection();
            DTO.NhanVien nv = null;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_ThongTinNV", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NhanVienID", IDNV);

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    nv = new DTO.NhanVien
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
                MessageBox.Show("Lỗi");
            }


            return nv;
        }
        public bool Update_NVInfor(NhanVien ttnv)
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
                return false;
            }
        }
        public bool ChangePass(NhanVien rp)
        {
            cn = con.getConnection();
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
                return false;
            }
        }
    }
}
