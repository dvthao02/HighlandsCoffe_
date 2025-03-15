using HighLandsCoffee_Manager.DAO;
using HighLandsCoffee_Manager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.BUS
{
    class BUS_ConnectDB
    {
        DAO_ConnectDB dt = new DAO_ConnectDB();

        //kiểm tra rỗng -----------------------------------------------------------------------------------------------------------
        public bool kTRong(DTO_ConnectDB pKetNoi)
        {
            return dt.KTRong(pKetNoi);
        }

        //lưu tài khoản -----------------------------------------------------------------------------------------------------------
        public bool luuTaiKhoan(DTO_ConnectDB pKetNoi)
        {
            return dt.Luufile(pKetNoi);
        }

        //kiểm tra kết nôi --------------------------------------------------------------------------------------------------------
        public bool kTKetNoi()
        {
            return dt.KTKetNoi();
        }
    }
}
