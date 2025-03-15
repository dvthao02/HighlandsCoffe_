using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HighLandsCoffee_Manager.DAO;
using HighLandsCoffee_Manager.DTO;

namespace HighLandsCoffee_Manager.BUS
{
    public class BusTaiKhoan
    {
        TaiKhoan acc = new TaiKhoan();
        public string getAccount1(NhanVien ac)
        {
            return acc.getAccount(ac);
        }

        public DTO.NhanVien GetThongTinNV(string IDNV)
        {
            return acc.getInfor(IDNV);
        }

        public bool update_NVInfor(NhanVien ttnv)
        {
            return acc.Update_NVInfor(ttnv);
        }

        public bool changePass(NhanVien rp)
        {
            return acc.ChangePass(rp);
        }
    }
}
