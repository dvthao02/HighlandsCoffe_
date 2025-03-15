using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.DTO
{
    public class DTO_NhanVien
    {
        private string nhanVienID, matKhau, tenNhanVien, diaChi, Email, Sdt, chucVu, Quyen, Luong;
        public DTO_NhanVien ()
        {

        }
        public DTO_NhanVien (string nhanVienID, string matKhau, string tenNhanVien, string diaChi, string Email, string Sdt, string chucVu, string Quyen, string Luong)
        {
            this.nhanVienID = nhanVienID;
            this.matKhau = matKhau;
            this.tenNhanVien = tenNhanVien;
            this.diaChi = diaChi;
            this.Email = Email;
            this.Sdt = Sdt;
            this.chucVu = chucVu;
            this.Quyen = Quyen;
            this.Luong = Luong;
        }
        public DTO_NhanVien (string nhanVienID, string tenNhanVien, string diaChi, string Email, string Sdt, string chucVu, string Quyen, string Luong)
        {
            this.nhanVienID = nhanVienID;
            this.tenNhanVien = tenNhanVien;
            this.diaChi = diaChi;
            this.Email = Email;
            this.Sdt = Sdt;
            this.chucVu = chucVu;
            this.Quyen = Quyen;
            this.Luong = Luong;
        }
        public DTO_NhanVien (string tenNhanVien, string diaChi, string Email, string Sdt, string chucVu, string Quyen, string Luong)
        {
            this.tenNhanVien = tenNhanVien;
            this.diaChi = diaChi;
            this.Email = Email;
            this.Sdt = Sdt;
            this.chucVu = chucVu;
            this.Quyen = Quyen;
            this.Luong = Luong;
        }
        public DTO_NhanVien (string nhanVienID, string matKhau)
        {
            this.nhanVienID = nhanVienID;
            this.matKhau = matKhau;
        }
        public string NhanVienID
        {
            get { return nhanVienID; }
            set { nhanVienID = value; }
        }

        public string MatKhau
        {
            get { return matKhau; }
            set { matKhau = value; }
        }

        public string TenNhanVien
        {
            get { return tenNhanVien; }
            set { tenNhanVien = value; }
        }

        public string DiaChi
        {
            get { return diaChi; }
            set { diaChi = value; }
        }

        public string EMAIL
        {
            get { return Email; }
            set { Email = value; }
        }

        public string SDT
        {
            get { return Sdt; }
            set { Sdt = value; }
        }

        public string ChucVu
        {
            get { return chucVu; }
            set { chucVu = value; }
        }

        public string QUYEN
        {
            get { return Quyen; }
            set { Quyen = value; }
        }

        public string LUONG
        {
            get { return Luong; }
            set { Luong = value; }
        }
    }
    
}
