using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HighLandsCoffee_Manager.DTO;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.DAO
{
    public class DAO_NhanVien
    {
        KetNoiSQL dt = new KetNoiSQL();
        SqlConnection con;
        DataSet ds_NhanVien = new DataSet();
        //tạo đối tượng SqlDataAdapter
        SqlDataAdapter da;

        // kiểm tra kết nối server thành công ----------------------------------------------------------------------------------------
        public bool KTDatabaseConnectionDAO()
        {
            try
            {
                con = dt.conDB();
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
            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        // Close connection ----------------------------------------------------------------------------------------------------------
        public void closeConnection()
        {
            // Kiểm tra kết nối và Đóng CSDL
            if (con.State == ConnectionState.Open)
                con.Close();
        }

        public DataTable GetDanhSachNhanVien(string pID_Quyen)
        {
            // Kiểm tra nếu DataSet đã tồn tại dữ liệu, xóa dữ liệu trước khi nạp lại
            if (ds_NhanVien.Tables.Contains("NHANVIEN"))
            {
                ds_NhanVien.Tables["NHANVIEN"].Clear();
            }

            try
            {
                // Sử dụng using để đảm bảo SqlConnection được giải phóng sau khi sử dụng
                using (SqlConnection con = dt.conDB()) // dt.conDB() trả về SqlConnection
                {
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter("exec sp_XemDanhSachNhanVien @pID_Quyen", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@pID_Quyen", pID_Quyen);
                        da.Fill(ds_NhanVien, "NHANVIEN");
                    }
                }

                // Đặt khóa chính nếu quyền là Admin
                if (pID_Quyen == "Admin")
                {
                    DataColumn[] key = new DataColumn[1];
                    key[0] = ds_NhanVien.Tables["NHANVIEN"].Columns[0]; // chọn cột đầu tiên
                    ds_NhanVien.Tables["NHANVIEN"].PrimaryKey = key;
                }

                return ds_NhanVien.Tables["NHANVIEN"];
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc hiển thị thông báo cho người dùng
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public bool themNV(DTO_NhanVien lnv)
        {
            con = dt.conDB(); ;
            try
            {
                con.Open();

                string sql = "EXEC SP_THEMNV @NhanVienID, @MatKhau, @TenNhanVien, @DiaChi, @Email, @SDT, @ChucVu, @Quyen, @Luong";
                SqlCommand cmd = new SqlCommand(sql, con);

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

                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                // Hiển thị thông báo lỗi cho người dùng
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        // Xoá -----------------------------------------------------------------------------------------------------------------------
        public bool XoaNV(DTO_NhanVien lnv)
        {
            con = dt.conDB(); ;
            try
            {
                con.Open();

                string sql = "exec SP_XOANV '" + lnv.NhanVienID + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery(); // Thực thi câu lệnh update
                ds_NhanVien.Tables["NHANVIEN"].Clear();

                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sửa thông tin  ---------------------------------------------------------------------------------------------------
        public bool SuaNV(DTO_NhanVien lnv)
        {
            con = dt.conDB(); ;
            try
            {
                con.Open();

                string sql = "EXEC SP_SUANV @NhanVienID, @TenNhanVien, @DiaChi, @Email, @SDT, @ChucVu, @Quyen, @Luong";
                SqlCommand cmd = new SqlCommand(sql, con);

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

                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                System.Windows.Forms.MessageBox.Show("Lỗi khi cập nhật thông tin: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //public bool Sua1NV(DTO_NhanVien lnv)
        //{
        //    con = dt.conDB_DB(); ;
        //    try
        //    {
        //        con.Open();

        //        string sql = "EXEC SP_SUA1NV @NhanVienID, @TenNhanVien, @DiaChi, @Email, @SDT";
        //        SqlCommand cmd = new SqlCommand(sql, con);

        //        // Sử dụng SqlParameter để thêm các giá trị
        //        cmd.Parameters.AddWithValue("@NhanVienID", lnv.NhanVienID);
        //        cmd.Parameters.AddWithValue("@TenNhanVien", lnv.TenNhanVien);
        //        cmd.Parameters.AddWithValue("@DiaChi", lnv.DiaChi);
        //        cmd.Parameters.AddWithValue("@Email", lnv.EMAIL);
        //        cmd.Parameters.AddWithValue("@SDT", lnv.SDT);


        //        // Thực thi câu lệnh insert
        //        cmd.ExecuteNonQuery();

        //        con.Close();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        // Tìm kiếm thông tin nhân viên ------------------------------------------------------------------------------------------------------------------
        public DataTable timKiemNV(string timkiem)
        {
            //tao doi tuong sqldataadapter
            SqlDataAdapter da = new SqlDataAdapter("exec SP_Find_NV @timkiem", con);

            // thêm tham số cho truy vấn
            da.SelectCommand.Parameters.AddWithValue("@timkiem", "%" + timkiem + "%");

            //dien du lieu vao dataSet hoac goi anh xa bang khoa len dataset
            DataSet ds_NhanVien = new DataSet();
            da.Fill(ds_NhanVien, "NV_CV_con_TK");

            //truoc khi them xoa sua can dat khoa chinh cho table khach hang
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_NhanVien.Tables["NV_CV_con_TK"].Columns[0]; //chọn columns 0
                                                                   //đặt làm khóa chính
            ds_NhanVien.Tables["NV_CV_con_TK"].PrimaryKey = key;

            //trả về ds nhân viên
            return ds_NhanVien.Tables["NV_CV_con_TK"];
        }
    }
}
