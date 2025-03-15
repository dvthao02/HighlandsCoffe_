using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HighLandsCoffee_Manager.DAO;
using HighLandsCoffee_Manager.DTO;

namespace HighLandsCoffee_Manager.BUS
{
    public class BUS_NhanVien
    {
        DAO_NhanVien dsnv = new DAO_NhanVien();
        public DataTable getDanhSachNhanVien(string quyen)
        {
            return dsnv.GetDanhSachNhanVien(quyen);
        }

        public bool themNV(DTO_NhanVien lnv)
        {
            return dsnv.themNV(lnv);
        }

        public bool xoaNV(DTO_NhanVien lnv)
        {
            return dsnv.XoaNV(lnv);
        }

        public bool suaNV(DTO_NhanVien lnv)
        {
            return dsnv.SuaNV(lnv);
        }
        //public bool sua1NV(NhanVienDTO lnv)
        //{
        //    return dsnv.Sua1NV(lnv);
        //}
    }
}
