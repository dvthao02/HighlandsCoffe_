using HighLandsCoffee_Manager.DAO;
using HighLandsCoffee_Manager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.BUS
{
    public class BusDSNV
    {
        DSNV dsnv = new DSNV();
        public DataTable getDanhSachNhanVien(string quyen)
        {
            return dsnv.GetDanhSachNhanVien(quyen);
        }

        public bool themNV(NhanVien lnv)
        {
            return dsnv.themNV(lnv);
        }

        public bool xoaNV(NhanVien lnv)
        {
            return dsnv.XoaNV(lnv);
        }

        public bool suaNV(NhanVien lnv)
        {
            return dsnv.SuaNV(lnv);
        }
        public bool sua1NV(NhanVien lnv)
        {
            return dsnv.Sua1NV(lnv);
        }
    }
}
