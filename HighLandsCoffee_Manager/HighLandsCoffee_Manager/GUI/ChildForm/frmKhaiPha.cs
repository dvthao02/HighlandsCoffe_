using System;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraCharts;
using HighLandsCoffee_Manager.BUS;
using System.Windows.Forms;
using System.Linq;

namespace HighLandsCoffee_Manager.GUI.ChildForm
{
    public partial class frmKhaiPha : XtraForm
    {
        private BUS_KhaiPha_Mining busKhaiPhaMining;

        // Constructor
        public frmKhaiPha()
        {
            InitializeComponent();
            busKhaiPhaMining = new BUS_KhaiPha_Mining();
        }

        // Load form
        private void frmKhaiPha_Load(object sender, EventArgs e)
        {

        }

        // Load danh sách Mining Models
        private void LoadMiningModels()
        {
            var miningModels = busKhaiPhaMining.GetDataMiningModels();
            if (miningModels == null || miningModels.Rows.Count == 0)
            {
                XtraMessageBox.Show("Không có dữ liệu mô hình.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cboModel.Properties.Items.Clear();
            cboModel.Properties.Items.AddRange(miningModels.AsEnumerable().Select(row => row["MODEL_NAME"].ToString()).ToArray());
            cboModel.EditValue = miningModels.Rows[0]["MODEL_NAME"].ToString();

            UpdatePredictedData(cboModel.EditValue.ToString());
        }

        // Cập nhật dữ liệu dự đoán
        private void UpdatePredictedData(string modelName)
        {
            try
            {
                var predictedData = busKhaiPhaMining.GetDistinctAttributes(modelName);
                if (predictedData == null || predictedData.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Không có dữ liệu dự đoán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cboPredict.Properties.Items.Clear();
                cboPredict.Properties.Items.AddRange(predictedData.AsEnumerable().Select(row => row["ATTRIBUTE_NAME"].ToString()).Distinct().ToArray());
                cboPredict.EditValue = cboPredict.Properties.Items[0];

                gridControl.DataSource = predictedData;
                gridView1.PopulateColumns();

                UpdateChart(predictedData);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi cập nhật dữ liệu dự đoán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Khi thay đổi model
        private void cboModel_EditValueChanged(object sender, EventArgs e)
        {
            if (cboModel.EditValue == null) return;
            UpdatePredictedData(cboModel.EditValue.ToString());
        }

        // Khi thay đổi dự đoán
        private void cboPredict_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboPredict.EditValue == null || gridControl.DataSource == null) return;

                var predictedData = gridControl.DataSource as DataTable;
                var filteredData = predictedData.AsEnumerable()
                    .Where(row => row["ATTRIBUTE_NAME"].ToString() == cboPredict.EditValue.ToString())
                    .CopyToDataTable();

                gridControl.DataSource = filteredData;
                gridView1.PopulateColumns();

                UpdateChart(filteredData);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi lọc dữ liệu dự đoán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật biểu đồ
        private void UpdateChart(DataTable predictedData)
        {
            try
            {
                var chartData = new DataTable();
                chartData.Columns.Add("Category", typeof(string));
                chartData.Columns.Add("Value", typeof(decimal));

                foreach (DataRow row in predictedData.Rows)
                {
                    var newRow = chartData.NewRow();
                    newRow["Category"] = row["Chi Nhanh ID"].ToString();
                    newRow["Value"] = Convert.ToDecimal(row["PredictedDoanhThu"]);
                    chartData.Rows.Add(newRow);
                }

                chart_HT.DataSource = chartData;
                var series = chart_HT.Series[0];
                series.ArgumentDataMember = "Category";
                series.ValueDataMembers[0] = "Value";
                chart_HT.RefreshData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi cập nhật biểu đồ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
