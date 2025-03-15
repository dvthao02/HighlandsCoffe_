using HighLandsCoffee_Manager.DAO;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.BUS
{
    public class BUS_NapDL
    {
        DAO_NapDL dt = new DAO_NapDL();

        // Nạp dữ liệu (Chạy job)
        public bool NapDuLieu(string jobName)
        {
            return dt.NapDuLieu(jobName);
        }
        public bool XoaDL(string dbXoa)
        {
            return dt.XoaDL(dbXoa);
        }

        // Lấy dữ liệu từ kho tạm NDS thông qua thủ tục
        public DataTable Get_DanhSach_NDS(string storedProcedureName)
        {
            return dt.Get_DanhSach_NDS(storedProcedureName);
        }

        // Lấy dữ liệu từ kho DW DDS thông qua thủ tục
        public DataTable Get_DanhSach_Kho_DW(string storedProcedureName)
        {
            return dt.Get_DanhSach_Kho_DW(storedProcedureName);
        }

        // Kiểm tra trạng thái job SQL đang chạy
        public bool Is_Run_NapKho()
        {
            return dt.Is_Run_NapKho();
        }
    }
}
