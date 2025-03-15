using HighLandsCoffee_Manager.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HighLandsCoffee_Manager.DAO
{
    public class DAO_NapDL
    {
        SqlConnection con;
        KetNoiSQL dt = new KetNoiSQL();
        SqlDataAdapter da;

        public DAO_NapDL()
        {
        }

        #region Hiển thị dữ liệu sử dụng thủ tục

        public DataTable Get_DanhSach_NDS(string storedProcedureName)
        {
            con = dt.conDB_DB("NDS_HighlandsCoffee");
            DataTable dataTable = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(storedProcedureName, con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hiển thị dữ liệu từ NDS: " + ex.Message);
            }
            return dataTable;
        }

        public DataTable Get_DanhSach_Kho_DW(string storedProcedureName)
        {
            con = dt.conDB_DB("DDS_HighlandsCoffee");
            DataTable dataTable = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(storedProcedureName, con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hiển thị dữ liệu từ DDS: " + ex.Message);
            }
            return dataTable;
        }

        #endregion

        #region Đóng mở kết nối

        public void openConnection()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        public void closeConnection()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }

        #endregion

        #region Kiểm tra job nạp kho đang chạy

        public bool Is_Run_NapKho()
        {
            try
            {
                con = dt.conDB();
                bool isRun = true;

                while (isRun)
                {
                    openConnection();
                    SqlCommand cmd = new SqlCommand("sp_Is_RunJob_NapKho", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    var reader = cmd.ExecuteReader();

                    isRun = reader.HasRows;
                    closeConnection();
                }

                return isRun;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Xử lý nạp dữ liệu (Chạy job)

        public bool NapDuLieu(string jobName)
        {
            try
            {
                con = dt.conDB();
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "msdb.dbo.sp_start_job"
                };
                cmd.Parameters.AddWithValue("@job_name", jobName);
                cmd.Connection = con;

                using (con)
                {
                    openConnection();
                    using (cmd)
                    {
                        cmd.ExecuteNonQuery();
                    }   
                    closeConnection();
                }
                return true; // Thành công
            }
            catch
            {
                return false; // Thất bại
            }
        }

        #endregion
        public bool XoaDL(string jobXoa)
        {
            try
            {
                con = dt.conDB();
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "msdb.dbo.sp_start_job"
                };
                cmd.Parameters.AddWithValue("@job_name", jobXoa);
                cmd.Connection = con;

                using (con)
                {
                    openConnection();
                    using (cmd)
                    {
                        cmd.ExecuteNonQuery();
                    }
                    closeConnection();
                }
                return true; // Thành công
            }
            catch
            {
                return false; // Thất bại
            }
        }
    }
}
