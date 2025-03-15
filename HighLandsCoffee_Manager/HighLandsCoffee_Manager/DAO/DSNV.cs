using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HighLandsCoffee_Manager.DTO;

namespace HighLandsCoffee_Manager.DAO
{
    public class DSNV
    {
        KetNoi con = new KetNoi();

        SqlConnection cn;
        DataSet ds_NhanVien = new DataSet();
        //tạo đối tượng SqlDataAdapter
        SqlDataAdapter da;

        // kiểm tra kết nối server thành công ----------------------------------------------------------------------------------------
        public bool KTKetNoi()
        {
            try
            {
                //cn = con.conDB();
                cn.Open();
                openConnection();
                closeConnection();
                return true;//thành công
            }
            catch
            {
                return false;//that bai
            }
        }

        // Open connection -----------------------------------------------------------------------------------------------------------
        public void openConnection()
        {
            // Mở kết nối đến CSDL
            if (cn.State == ConnectionState.Closed)
                cn.Open();
        }

        // Close connection ----------------------------------------------------------------------------------------------------------
        public void closeConnection()
        {
            // Kiểm tra kết nối và Đóng CSDL
            if (cn.State == ConnectionState.Open)
                cn.Close();
        }

        public DataTable GetDanhSachNhanVien(string pID_Quyen)
        {
            //con = cn.conDB();
            cn = con.getConnection();
            cn.Open();
            da = new SqlDataAdapter("exec sp_XemDanhSachNhanVien '" + pID_Quyen + "'", cn);
            da.Fill(ds_NhanVien, "NHANVIEN");
            DataColumn[] key = new DataColumn[1];
            if (pID_Quyen == "Admin")
            {
                key[0] = ds_NhanVien.Tables["NHANVIEN"].Columns[0];//chọn columns 0
                ds_NhanVien.Tables["NHANVIEN"].PrimaryKey = key;
            }
            return ds_NhanVien.Tables["NHANVIEN"];
        }

        public bool themNV(NhanVien lnv)
        {
            cn = con.getConnection();;
            try
            {
                cn.Open();

                string sql = "EXEC SP_THEMNV @NhanVienID, @MatKhau, @TenNhanVien, @DiaChi, @Email, @SDT, @ChucVu, @Quyen, @Luong";
                SqlCommand cmd = new SqlCommand(sql, cn);

                // Sử dụng SqlParameter để thêm các giá trị
                cmd.Parameters.AddWithValue("@NhanVienID", lnv.NhanVienID);
                cmd.Parameters.AddWithValue("@MatKhau", "123"); // Mật khẩu cố định
                cmd.Parameters.AddWithValue("@TenNhanVien", lnv.TenNhanVien);
                cmd.Parameters.AddWithValue("@DiaChi", lnv.DiaChi);
                cmd.Parameters.AddWithValue("@Email", lnv.EMAIL);
                cmd.Parameters.AddWithValue("@SDT", lnv.SDT);
                cmd.Parameters.AddWithValue("@ChucVu", lnv.ChucVu);
                cmd.Parameters.AddWithValue("@Quyen", lnv.QUYEN);
                cmd.Parameters.AddWithValue("@Luong", lnv.LUONG);

                // Thực thi câu lệnh insert
                cmd.ExecuteNonQuery();

                cn.Close();
                return true;
            }
            catch
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                return false;
            }
        }

        // Xoá -----------------------------------------------------------------------------------------------------------------------
        public bool XoaNV(NhanVien lnv)
        {
            cn = con.getConnection();;
            try
            {
                cn.Open();

                string sql = "exec SP_XOANV '" + lnv.NhanVienID + "'";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery(); // Thực thi câu lệnh update
                ds_NhanVien.Tables["NHANVIEN"].Clear();

                cn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sửa thông tin  ---------------------------------------------------------------------------------------------------
        public bool SuaNV(NhanVien lnv)
        {
            cn = con.getConnection();;
            try
            {
                cn.Open();

                string sql = "EXEC SP_SUANV @NhanVienID, @TenNhanVien, @DiaChi, @Email, @SDT, @ChucVu, @Quyen, @Luong";
                SqlCommand cmd = new SqlCommand(sql, cn);

                // Sử dụng SqlParameter để thêm các giá trị
                cmd.Parameters.AddWithValue("@NhanVienID", lnv.NhanVienID);               
                cmd.Parameters.AddWithValue("@TenNhanVien", lnv.TenNhanVien);
                cmd.Parameters.AddWithValue("@DiaChi", lnv.DiaChi);
                cmd.Parameters.AddWithValue("@Email", lnv.EMAIL);
                cmd.Parameters.AddWithValue("@SDT", lnv.SDT);
                cmd.Parameters.AddWithValue("@ChucVu", lnv.ChucVu);
                cmd.Parameters.AddWithValue("@Quyen", lnv.QUYEN);
                cmd.Parameters.AddWithValue("@Luong", lnv.LUONG);

                // Thực thi câu lệnh insert
                cmd.ExecuteNonQuery();

                cn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Sua1NV(NhanVien lnv)
        {
            cn = con.getConnection(); ;
            try
            {
                cn.Open();

                string sql = "EXEC SP_SUA1NV @NhanVienID, @TenNhanVien, @DiaChi, @Email, @SDT";
                SqlCommand cmd = new SqlCommand(sql, cn);

                // Sử dụng SqlParameter để thêm các giá trị
                cmd.Parameters.AddWithValue("@NhanVienID", lnv.NhanVienID);
                cmd.Parameters.AddWithValue("@TenNhanVien", lnv.TenNhanVien);
                cmd.Parameters.AddWithValue("@DiaChi", lnv.DiaChi);
                cmd.Parameters.AddWithValue("@Email", lnv.EMAIL);
                cmd.Parameters.AddWithValue("@SDT", lnv.SDT);


                // Thực thi câu lệnh insert
                cmd.ExecuteNonQuery();

                cn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        // Tìm kiếm thông tin nhân viên ------------------------------------------------------------------------------------------------------------------
        public DataTable timKiemNV(string timkiem)
        {
            //tao doi tuong sqldataadapter
            SqlDataAdapter da = new SqlDataAdapter("exec SP_Find_NV @timkiem", cn);

            // thêm tham số cho truy vấn
            da.SelectCommand.Parameters.AddWithValue("@timkiem", "%" + timkiem + "%");

            //dien du lieu vao dataSet hoac goi anh xa bang khoa len dataset
            DataSet ds_NhanVien = new DataSet();
            da.Fill(ds_NhanVien, "NV_CV_CN_TK");

            //truoc khi them xoa sua can dat khoa chinh cho table khach hang
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_NhanVien.Tables["NV_CV_CN_TK"].Columns[0]; //chọn columns 0
                                                                   //đặt làm khóa chính
            ds_NhanVien.Tables["NV_CV_CN_TK"].PrimaryKey = key;

            //trả về ds nhân viên
            return ds_NhanVien.Tables["NV_CV_CN_TK"];
        }
    }
}
