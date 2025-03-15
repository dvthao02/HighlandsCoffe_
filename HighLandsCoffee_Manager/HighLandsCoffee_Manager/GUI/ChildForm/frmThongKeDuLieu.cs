using DevExpress.XtraEditors;
using DevExpress.DashboardWin;
using DevExpress.DashboardCommon;
using System;
using System.Windows.Forms;
using DevExpress.DataAccess.ConnectionParameters;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmThongKeDuLieu : XtraForm
    {
        public frmThongKeDuLieu()
        {
            InitializeComponent();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                // Đường dẫn tới file XML
                string dashboardFilePath = @"D:\DoAnChuyenNganh\QL_HighlandsCoffee\HighLandsCoffee_Manager\ThongKeDoanhThuTheoTuoiVaTungNam.xml";

                // Nạp file vào Dashboard Viewer
                dashboardDesigner1.LoadDashboard(dashboardFilePath);

                // Đăng ký sự kiện cấu hình kết nối dữ liệu
                dashboardDesigner1.ConfigureDataConnection += DashboardViewer1_ConfigureDataConnection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải Dashboard: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DashboardViewer1_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
        {
            if (e.ConnectionName == "THAO\\MSSQLSERVER22_SSAS_HighlandsCoffee__DDS Highlands Coffee")
            {
                // Cấu hình thông tin kết nối OLAP
                var olapParams = (OlapConnectionParameters)e.ConnectionParameters;
                olapParams.ConnectionString = "data source=THAO\\MSSQLSERVER22;initial catalog=SSAS_HighlandsCoffee_;Cube Name=DDS Highlands Coffee;";
            }
        }
    }
}
