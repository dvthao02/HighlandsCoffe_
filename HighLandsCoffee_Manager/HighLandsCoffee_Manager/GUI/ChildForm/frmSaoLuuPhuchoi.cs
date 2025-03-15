using DevExpress.XtraEditors;
using HighLandsCoffee_Manager.DTO;
using HighLandsCoffee_Manager.Helpers;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmSaoLuuPhuchoi : DevExpress.XtraEditors.XtraForm
    {
        private readonly KetNoiSQL dt = new KetNoiSQL();

        public frmSaoLuuPhuchoi()
        {
            InitializeComponent();
        }

        private void frmSaoLuuPhuchoi_Load(object sender, EventArgs e)
        {
            LoadDatabaseList();
            btnSaoLuu.Enabled = false; // Vô hiệu hóa nút sao lưu khi chưa chọn đường dẫn
        }

        private void btnSaoLuu_Click(object sender, EventArgs e)
        {
            try
            {
                string backupPath = GetBackupFilePath();
                PerformBackup(backupPath);
                XtraMessageBox.Show("Sao lưu cơ sở dữ liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi sao lưu cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
            }
        }

        private void btnPhucHoiCSDL_Click(object sender, EventArgs e)
        {
            try
            {
                string backupFile = txtDiaChiPhucHoi.Text;

                // Kiểm tra nếu địa chỉ phục hồi trống
                if (string.IsNullOrEmpty(backupFile))
                {
                    XtraMessageBox.Show("Vui lòng nhập địa chỉ file sao lưu cần phục hồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng quá trình phục hồi nếu không có địa chỉ
                }

                // Tiến hành phục hồi cơ sở dữ liệu
                PerformRestore(backupFile);
                XtraMessageBox.Show("Phục hồi cơ sở dữ liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi phục hồi cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
            }
        }


        private void btnDiaChiSaoLuu_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtDiaChiSaoLuu.Text = dlg.SelectedPath;
                    btnSaoLuu.Enabled = true;
                }
            }
        }

        private void btnDuongDanPhucHoi_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "SQL SERVER database backup files|*.bak";
                dlg.Title = "Chọn file sao lưu để phục hồi";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtDiaChiPhucHoi.Text = dlg.FileName;
                }
            }
        }

        // Lấy tên cơ sở dữ liệu từ ComboBox
        private string Get_NameDataBase()
        {
            if (cbbTenCSDL.SelectedItem != null)
            {
                return cbbTenCSDL.SelectedItem.ToString();
            }
            throw new Exception("Vui lòng chọn cơ sở dữ liệu.");
        }

        // Tải danh sách cơ sở dữ liệu vào ComboBox
        private void LoadDatabaseList()
        {
            try
            {
                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang tải danh sách cơ sở dữ liệu...");
                cbbTenCSDL.Items.Clear();
                var databaseNames = dt.GetDatabaseNames();
                cbbTenCSDL.Items.AddRange(databaseNames.ToArray());
                cbbTenCSDL.SelectedIndex = cbbTenCSDL.Items.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải danh sách cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
            }
        }

        // Lấy đường dẫn sao lưu từ người dùng
        private string GetBackupFilePath()
        {
            if (string.IsNullOrEmpty(txtDiaChiSaoLuu.Text))
            {
                throw new Exception("Vui lòng chọn đường dẫn sao lưu.");
            }

            string dbName = Get_NameDataBase();
            string backupPath = Path.Combine(txtDiaChiSaoLuu.Text, $"{dbName}_{DateTime.Now:yyyyMMdd}.bak");

            // Kiểm tra tệp sao lưu đã tồn tại và yêu cầu người dùng xác nhận
            if (File.Exists(backupPath))
            {
                DialogResult result = XtraMessageBox.Show("Tệp sao lưu đã tồn tại. Bạn có muốn ghi đè không?",
                    "Xác nhận ghi đè", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    backupPath = Path.Combine(txtDiaChiSaoLuu.Text, $"{dbName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
                }
            }

            return backupPath;
        }

        // Tiến hành sao lưu cơ sở dữ liệu
        private void PerformBackup(string backupPath)
        {
            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang sao lưu cơ sở dữ liệu, vui lòng chờ...");
            string dbName = Get_NameDataBase();
            string query = $"BACKUP DATABASE [{dbName}] TO DISK = '{backupPath}' WITH FORMAT, MEDIANAME = 'DbBackup', NAME = 'Full Backup of {dbName}'";

            using (SqlConnection con = dt.conDB_SaoLuu(dbName))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        }

        // Tiến hành phục hồi cơ sở dữ liệu
        private void PerformRestore(string backupFile)
        {
            string dbName = Get_NameDataBase();
            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang phục hồi cơ sở dữ liệu, vui lòng chờ...");

            using (SqlConnection con = dt.conDB_PhucHoi())
            {
                con.Open();

                // Set database to single-user mode
                string setSingleUser = $"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                SqlCommand cmdSingleUser = new SqlCommand(setSingleUser, con);
                cmdSingleUser.ExecuteNonQuery();

                // Phục hồi cơ sở dữ liệu
                string restoreQuery = $@"
                RESTORE DATABASE [{dbName}] 
                FROM DISK = '{backupFile}' 
                WITH REPLACE, 
                MOVE '{dbName}' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER22\MSSQL\Data\{dbName}.mdf',
                MOVE '{dbName}_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER22\MSSQL\Data\{dbName}_log.ldf'";

                SqlCommand cmdRestore = new SqlCommand(restoreQuery, con);
                cmdRestore.ExecuteNonQuery();

                // Set database back to multi-user mode
                string setMultiUser = $"ALTER DATABASE [{dbName}] SET MULTI_USER";
                SqlCommand cmdMultiUser = new SqlCommand(setMultiUser, con);
                cmdMultiUser.ExecuteNonQuery();
            }
        }
    }
}
