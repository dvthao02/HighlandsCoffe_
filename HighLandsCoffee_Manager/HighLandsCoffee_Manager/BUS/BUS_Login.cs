using HighLandsCoffee_Manager.DAO;
using HighLandsCoffee_Manager.DTO;
using System;

namespace HighLandsCoffee_Manager.BUS
{
    public class BUS_Login
    {
        private DAO_Login daoLogin = new DAO_Login();

        /// <summary>
        /// Kiểm tra dữ liệu đăng nhập có rỗng không.
        /// </summary>
        /// <param name="login">Đối tượng DTO_Login chứa thông tin tài khoản và mật khẩu.</param>
        /// <returns>True nếu tất cả các trường đều có giá trị, ngược lại trả về False.</returns>
        public bool KiemTraRong(DTO_Login login)
        {
            return daoLogin.KTRong(login);
        }

        /// <summary>
        /// Thực hiện đăng nhập.
        /// </summary>
        /// <param name="login">Đối tượng DTO_Login chứa thông tin tài khoản và mật khẩu.</param>
        /// <param name="errorMessage">Thông báo lỗi chi tiết (nếu có).</param>
        /// <returns>Trả về kết quả đăng nhập (quyền hoặc thông báo lỗi).</returns>
        public string DangNhap(DTO_Login login, ref string errorMessage)
        {
            return daoLogin.DangNhap(login, ref errorMessage);
        }

        /// <summary>
        /// Kiểm tra kết nối đến máy chủ.
        /// </summary>
        /// <param name="errorMessage">Thông báo lỗi chi tiết khi kết nối thất bại.</param>
        /// <returns>True nếu kết nối thành công, ngược lại trả về False.</returns>
        public bool KiemTraKetNoi(ref string errorMessage)
        {
            return daoLogin.KTKetNoi(ref errorMessage);
        }
    }
}
