using HighLandsCoffee_Manager.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HighLandsCoffee_Manager.DAO
{
    public class DAO_Login
    {
        private readonly KetNoiSQL dt;

        public DAO_Login()
        {
            dt = new KetNoiSQL();
        }

        #region Đóng mở kết nối
        /// <summary>
        /// Lấy kết nối cơ sở dữ liệu từ lớp cấu hình.
        /// </summary>
        /// <returns>SqlConnection</returns>
        private SqlConnection GetConnection()
        {
            return dt.conDB();
        }
        #endregion

        #region Xử lý
        /// <summary>
        /// Kiểm tra kết nối đến server có thành công không.
        /// </summary>
        /// <returns>True nếu kết nối thành công, ngược lại trả về False.</returns>
        public bool KTKetNoi(ref string errorMessage)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi chi tiết
                errorMessage = $"[Lỗi kết nối] {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra tài khoản đăng nhập có bị bỏ trống không.
        /// </summary>
        /// <param name="pLogi">Đối tượng chứa thông tin đăng nhập.</param>
        /// <returns>True nếu tất cả thông tin được điền đầy đủ, ngược lại trả về False.</returns>
        public bool KTRong(DTO_Login pLogi)
        {
            return !(string.IsNullOrEmpty(pLogi.UserName) || string.IsNullOrEmpty(pLogi.Pass));
        }

        /// <summary>
        /// Xử lý đăng nhập, kiểm tra tài khoản và mật khẩu từ cơ sở dữ liệu.
        /// </summary>
        /// <param name="acc">Đối tượng chứa thông tin đăng nhập.</param>
        /// <param name="errorMessage">Thông báo lỗi chi tiết (nếu có).</param>
        /// <returns>Chuỗi mô tả kết quả đăng nhập: quyền, lỗi hoặc trạng thái tài khoản.</returns>
        public string DangNhap(DTO_Login acc, ref string errorMessage)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    string SQL = "EXEC sp_DangNhap @NhanVienID, @MatKhau";
                    using (SqlCommand cmd = new SqlCommand(SQL, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@NhanVienID", acc.UserName);
                        cmd.Parameters.AddWithValue("@MatKhau", acc.Pass);

                        using (SqlDataReader ad = cmd.ExecuteReader())
                        {
                            if (ad.HasRows && ad.Read())
                            {
                                string trangThai = ad["TrangThai"]?.ToString();
                                if (trangThai == "Locked")
                                {
                                    errorMessage = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.";
                                    return null; // Trả về null nếu tài khoản bị khóa
                                }

                                string quyen = ad["Quyen"]?.ToString();
                                return quyen; // Trả về quyền nếu đăng nhập thành công
                            }
                            else
                            {
                                errorMessage = "Tài khoản hoặc mật khẩu không chính xác.";
                                return null; // Trả về null nếu đăng nhập thất bại
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Trả về thông báo lỗi SQL chi tiết
                errorMessage = $"[Lỗi SQL] {sqlEx.Message}";
                return null;
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi hệ thống
                errorMessage = $"[Lỗi hệ thống] {ex.Message}";
                return null;
            }
        }
        #endregion
    }
}
