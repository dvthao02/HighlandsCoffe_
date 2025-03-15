using DevExpress.PivotGrid.OLAP.AdoWrappers;
using DevExpress.XtraRichEdit.Model;
using HighLandsCoffee_Manager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace HighLandsCoffee_Manager.DAO
{
    public class DAO_ConnectDB
    {
        SqlConnection con;
        KetNoiSQL dt = new KetNoiSQL();

        public DAO_ConnectDB()
        {
        }

        #region Lưu file thông tin server
        //lưu File ----------------------------------------------------------------------------------------------------------------
        public bool Luufile(DTO_ConnectDB pKetNoi)
        {
            try
            {
                string[] luu = new string[] { pKetNoi.TenServer, pKetNoi.UserName, pKetNoi.Pass };
                ////đường dẫn
                string path = System.Windows.Forms.Application.StartupPath + "\\ServerName.txt";
                //false là không ghi đè,  true là ghi đè
                StreamWriter ws = new StreamWriter(path, false, Encoding.UTF8);//lưu đường dẫn
                                                                               //ghi dữ liệu vào
                                                                               //ws.WriteLine("Data Source = " + tenserver + "; Initial Catalog = QLTiemThuocTay; User ID = sa; Password = " + pass + "");

                //lưu từng dòng
                foreach (string s in luu)
                {
                    ws.WriteLine(s);//add vào file txt
                }
                //đóng file
                ws.Close();
                //thành công
                return true;
            }
            catch
            {
                return false;//thất Bại
            }
        }
        #endregion

        #region đóng mở kết nối

        // Open connection --------------------------------------------------------------------------------------------------------
        public void openConnection()
        {
            // Mở kết nối đến CSDL
            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        // Close connection -------------------------------------------------------------------------------------------------------
        public void closeConnection()
        {
            // Kiểm tra kết nối và Đóng CSDL
            if (con.State == ConnectionState.Open)
                con.Close();
        }
        #endregion

        #region Xử lý

        //kiểm tra kết nối thành công ---------------------------------------------------------------------------------------------
        public bool KTKetNoi()
        {
            try
            {
                con = dt.conDB();
                openConnection();
                closeConnection();
                return true;//thành công
            }
            catch
            {
                return false;//that bai
            }
        }

        //kiểm tra rỗng -----------------------------------------------------------------------------------------------------------
        public bool KTRong(DTO_ConnectDB pKetNoi)
        {
            if (pKetNoi.TenServer.Length == 0)//rỗng
            {
                return false;
            }
            if (pKetNoi.UserName.Length == 0)//rỗng
            {
                return false;
            }
            if (pKetNoi.Pass.Length == 0)//rỗng
            {
                return false;
            }
            return true;//đã nhập tất cả
        }

        #endregion
    }
}
