using DevExpress.XtraEditors;
using HighLandsCoffee_Manager.BUS;
using HighLandsCoffee_Manager.DTO;
using HighLandsCoffee_Manager.Helpers;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.GUI
{
    public partial class frmKetNoiServer : DevExpress.XtraEditors.XtraForm
    {
        private BUS_ConnectServer dt = new BUS_ConnectServer();

        public frmKetNoiServer()
        {
            InitializeComponent();
        }

        #region Sự kiện load form và cấu hình
        private void frmKetNoiDB_Load(object sender, EventArgs e)
        {
            CauHinhForm();
            LoadServerFromFile();
        }

        private void CauHinhForm()
        {
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Đăng ký sự kiện phím Enter
            cboServer.KeyDown += Txt_KeyDown;
            txtUser.KeyDown += Txt_KeyDown;
            txtPass.KeyDown += Txt_KeyDown;

            // Đăng ký sự kiện DropDown cho ComboBox
            cboServer.Properties.QueryPopUp += (s, ev) => LoadLocalServers();
        }
        #endregion

        #region Xử lý sự kiện phím Enter
        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnKetNoi.PerformClick();
            }
        }
        #endregion

        #region Load thông tin server từ file
        private void LoadServerFromFile()
        {
            try
            {
                string path = Application.StartupPath + "\\ServerName.txt";
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                    {
                        string line;
                        if ((line = sr.ReadLine()) != null)
                        {
                            cboServer.Properties.Items.Add(line); // Thêm server từ file vào ComboBox
                            cboServer.Text = line; // Hiển thị server đầu tiên
                        }
                        if ((line = sr.ReadLine()) != null)
                            txtUser.Text = line; // Tên user
                        if ((line = sr.ReadLine()) != null)
                            txtPass.Text = line; // Mật khẩu
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Load danh sách server cục bộ
        private bool isLocalServersLoaded = false;

        private async void LoadLocalServers()
        {
            if (isLocalServersLoaded) return; // Kiểm tra nếu danh sách đã được load rồi, thì không làm gì nữa.

            try
            {
                // Hiển thị WaitForm
                WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang tải danh sách server từ máy...");

                await Task.Run(() =>
                {
                    var dataSourceEnum = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
                    foreach (System.Data.DataRow row in dataSourceEnum.Rows)
                    {
                        string serverName = row["ServerName"].ToString();
                        string instanceName = row["InstanceName"].ToString();

                        if (!string.IsNullOrEmpty(instanceName))
                            serverName += "\\" + instanceName; // Nếu có instance, thêm vào tên server

                        if (!cboServer.Properties.Items.Contains(serverName))
                        {
                            this.Invoke(new Action(() =>
                            {
                                cboServer.Properties.Items.Add(serverName); // Thêm vào ComboBox nếu chưa tồn tại
                            }));
                        }
                    }
                });

                isLocalServersLoaded = true; // Đánh dấu là đã load danh sách
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Lỗi khi load danh sách server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đóng WaitForm sau khi hoàn thành
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
            }
        }

        #endregion

        #region Xử lý kết nối server
        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            UI_Helpers.ActivateButton((SimpleButton)sender); // Kích hoạt giao diện nút
            DTO_ConnectServer dTO_KetNoi = new DTO_ConnectServer(cboServer.Text, txtUser.Text, txtPass.Text);

            // Hiển thị WaitForm khi bắt đầu kiểm tra kết nối
            WaitFormHelper.ShowWaitForm(splashScreenManager1, "Đang kiểm tra kết nối đến server...");

            if (dt.kTRong(dTO_KetNoi))
            {
                // Lưu thông tin tài khoản và server
                if (dt.luuTaiKhoan(dTO_KetNoi))
                {
                    // Kiểm tra kết nối đến cơ sở dữ liệu
                    if (dt.kTKetNoi())
                    {
                        WaitFormHelper.CloseWaitForm(splashScreenManager1); // Đóng WaitForm
                        XtraMessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Chuyển sang form Login
                        this.Hide(); // Ẩn form hiện tại
                    }
                    else
                    {
                        WaitFormHelper.CloseWaitForm(splashScreenManager1);
                        XtraMessageBox.Show("Kết nối thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cboServer.Focus();
                    }
                }
                else
                {
                    WaitFormHelper.CloseWaitForm(splashScreenManager1);
                    XtraMessageBox.Show("Lưu thông tin thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                WaitFormHelper.CloseWaitForm(splashScreenManager1);
                XtraMessageBox.Show("Chưa nhập đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
    }
}
