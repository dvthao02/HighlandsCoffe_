using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.DTO
{
    public class KetNoiSQL
    {
        string NameDataBase = "QL_BanHang_HighlandsCoffee";
        string NameCuBe = "DDS HighLands Coffee";
        string NameDataBase_SSAS = "SSAS_HighLandsCoffee_";
        #region Kết nối sql
        //Đọc file lưu tên server
        public string Docfile()
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
            return "Data Source=" + ServerName + ";Initial Catalog=" + NameDataBase + ";Persist Security Info=True;User ID=" + UserName + ";Password=" + pass + "";//giá trị của file

        }
        //connec để đăng nhập
        public SqlConnection conDB()
        {
            SqlConnection con = new SqlConnection(Docfile());
            return con;
        }
        #endregion

        #region Kết nối Sao luu phuc hoi
        // Đọc file lưu tên server (sao lưu, phục hồi) -------------------------------------------------------------------------------
        public string Docfile_SaoLuu(string pNameDataBase)
        {
            string ServerName = "";
            string UserName = "";
            string pass = "";
            string line = "";
            string path = System.Windows.Forms.Application.StartupPath + "\\ServerName.txt";
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                ServerName = line;
                line = sr.ReadLine(); // Đọc username
                UserName = line;
                line = sr.ReadLine(); // Đọc password
                pass = line;
            }
            sr.Close();
            // Kết nối tới cơ sở dữ liệu cần sao lưu
            return "Data Source=" + ServerName + ";Initial Catalog=" + pNameDataBase + ";Persist Security Info=True;User ID=" + UserName + ";Password=" + pass;
        }

        public SqlConnection conDB_SaoLuu(string pNameDataBase)
        {
            SqlConnection con = new SqlConnection(Docfile_SaoLuu(pNameDataBase));
            return con;
        }

        public string Docfile_PhucHoi()
        {
            string ServerName = "";
            string UserName = "";
            string pass = "";
            string line = "";
            string path = System.Windows.Forms.Application.StartupPath + "\\ServerName.txt";
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                ServerName = line;
                line = sr.ReadLine(); // Đọc username
                UserName = line;
                line = sr.ReadLine(); // Đọc password
                pass = line;
            }
            sr.Close();
            // Kết nối tới master database (không cần chỉ định cơ sở dữ liệu)
            return "Data Source=" + ServerName + ";Initial Catalog=master;Persist Security Info=True;User ID=" + UserName + ";Password=" + pass;
        }

        public SqlConnection conDB_PhucHoi()
        {
            SqlConnection con = new SqlConnection(Docfile_PhucHoi());
            return con;
        }

        #endregion
        #region Lấy danh sách tên database
        public List<string> GetDatabaseNames()
        {
            List<string> databaseNames = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(Docfile_PhucHoi()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')",
                        con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        databaseNames.Add(reader["name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách cơ sở dữ liệu: " + ex.Message);
            }
            return databaseNames;
        }
        #endregion


        #region Kết nối Olap
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
        #endregion

        #region Kết Nối cude
        //Đọc file lưu tên server
        public string DocFile_KetNoi_CuBe()
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
            return "Provider=MSOLAP.3;Data Source=" + ServerName + ";Initial Catalog=" + NameDataBase_SSAS + "";//giá trị của file

        }
        #endregion
        //connec để kết nối olap
        public AdomdConnection conDB_CuBe()
        {
            AdomdConnection con = new AdomdConnection(DocFile_KetNoi_CuBe());
            return con;
        }


        #region Kết nối database
        public string Docfile_DB(string pNameDataBase)
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
            return "Data Source=" + ServerName + ";Initial Catalog=" + pNameDataBase + ";Persist Security Info=True;User ID=" + UserName + ";Password=" + pass + "";//giá trị của file

        }
        //connec để đăng nhập
        public SqlConnection conDB_DB(string pNameDataBase)
        {
            SqlConnection con = new SqlConnection(Docfile_DB(pNameDataBase));
            return con;
        }
        #endregion
    }
}
