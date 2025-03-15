using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HighLandsCoffee_Manager.DAO;
using HighLandsCoffee_Manager.DTO;

namespace HighLandsCoffee_Manager.BUS
{
    public class BUS_User
    {
        DAO_User acc = new DAO_User();
        public DTO.DTO_NhanVien GetThongTinNV(string IDNV)
        {
            return acc.getInfor(IDNV);
        }

        public bool update_NVInfor(DTO_NhanVien ttnv)
        {
            return acc.Update_NVInfor(ttnv);
        }

        public bool changePass(DTO_NhanVien rp)
        {
            return acc.ChangePass(rp);
        }
        public bool lockAccount(DTO_NhanVien la)
        {
            return acc.LockAccount(la);
        }
        public bool unlockAccount(DTO_NhanVien ula)
        {
            return acc.UnLockAccount(ula);
        }
    }
}
