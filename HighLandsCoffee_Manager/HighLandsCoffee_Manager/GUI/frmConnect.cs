using DevExpress.XtraEditors;
using HighLandsCoffee_Manager.BUS;
using HighLandsCoffee_Manager.DAO;
using HighLandsCoffee_Manager.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace HighLandsCoffee_Manager.GUI
{
    public partial class frmConnect : Form
    {
        public KetNoi dbConnection;
        
        public frmConnect()
        {
            InitializeComponent();
            dbConnection = new KetNoi();
            
        }

        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            string database = txtDatabase.Text;
            string user = txtUser.Text;
            string pass = txtPass.Text;
            string message;
            KetNoi.Server = server;
            KetNoi.Database = database;
            KetNoi.User = user;
            KetNoi.Pass = pass;
            bool isConnected = dbConnection.TestConnection( out message);
            if (isConnected)
            {
                // Kết nối thành công
                MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Chuyển sang form Login
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
                
                // Đóng form kết nối sau khi chufrmLoginyển sang form Login
                this.Hide();
            }
            else
            {
                // Kết nối thất bại
                MessageBox.Show("Kết nối thất bại: " + message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Xóa các ô nhập để người dùng nhập lại
                txtServer.Clear();
                txtDatabase.Clear();
                txtUser.Clear();
                txtPass.Clear();

                // Đưa con trỏ về ô Server để người dùng nhập lại từ đầu
                txtServer.Focus();
            }

        }
    }
}
