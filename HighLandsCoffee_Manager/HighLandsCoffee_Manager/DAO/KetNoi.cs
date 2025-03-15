using DevExpress.PivotGrid.OLAP.AdoWrappers;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.XtraEditors.Mask.MaskSettings;
//using Microsoft.AnalysisServices.AdomdClient;

namespace HighLandsCoffee_Manager.DAO
{
    public class KetNoi
    {
        static SqlConnection con;

        //string NameCuBe = "DW Global Superstore";
        //string NameDataBase_SSAS = "SSAS_Vinamilk";
        //string NameCuBe = "DDS_HighlandsCoffee";
        public static string Server { get; set; }
        public static string Database { get; set; }
        public static string User{ get; set; }
        public static string Pass { get; set; }
        public SqlConnection getConnection()
        {
           

            // Tạo chuỗi kết nối từ các thuộc tính
            string connectionString = $"Data Source={Server};Initial Catalog={Database};User ID={User};Password={Pass};Encrypt=False;";

            // Khởi tạo đối tượng kết nối
            return new SqlConnection(connectionString);

        }
        public bool TestConnection( out string message)
        {
            using (SqlConnection con = getConnection())
            {
                try
                {
                    con.Open();
                    message = "Kết nối thành công!";
                    return true;
                }
                catch (Exception ex)
                {
                    message = "Kết nối thất bại: " + ex.Message;
                    return false;
                }
            }
        }
        //lấy dữ liệu 
        public DataTable LoadData(string sql, string server, string database, string user, string pass)
        {
            getConnection().Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            getConnection().Close();
            return dt;
        }

        public string Docfile_SaoLuu_PhucHoi(string pNameDataBase)
        {
            string ServerName = "";
            string UserName = "";
            string pass = "";
            string line = "";
            //đường dẫn
            string path = System.Windows.Forms.Application.StartupPath + "\\ServerName.txt";
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            //đọc file
            while ((line = sr.ReadLine()) != null)
            {
                ServerName = line;
                line = sr.ReadLine();//qua dòng mới
                UserName = line;
                line = sr.ReadLine();//qua dòng mới
                pass = line;
            }
            sr.Close();
            return @"Data Source=" + ServerName + ";Initial Catalog=" + pNameDataBase + ";Persist Security Info=True;User ID=" + UserName + ";Password=" + pass + ";Encrypt=False";//giá trị của file

        }
        //connec để đăng nhập
        public SqlConnection conDB_SaoLuu_PhucHoi(string pNameDataBase)
        {
            SqlConnection con = new SqlConnection(Docfile_SaoLuu_PhucHoi(pNameDataBase));
            return con;
        }

        //Đọc file lưu tên server
        public string Docfile_Olap()
        {
            string ServerName = "";
            string UserName = "";
            string pass = "";
            string line = "";
            //đường dẫn
            string path = System.Windows.Forms.Application.StartupPath + "\\ServerName.txt";
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            //đọc file
            while ((line = sr.ReadLine()) != null)
            {
                ServerName = line;
                line = sr.ReadLine();//qua dòng mới
                UserName = line;
                line = sr.ReadLine();//qua dòng mới
                pass = line;
            }
            sr.Close();
            return "Data Source=" + ServerName + ";Integrated Security=True";//giá trị của file

        }
        //connec để kết nối olap
        public SqlConnection conDB_Olap()
        {
            SqlConnection con = new SqlConnection(Docfile_Olap());
            return con;
        }
    }
}
