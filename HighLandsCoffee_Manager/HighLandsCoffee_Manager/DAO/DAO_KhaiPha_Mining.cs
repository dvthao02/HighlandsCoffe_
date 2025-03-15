using System;
using System.Data;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace HighLandsCoffee_Manager.DAO
{
    class DAO_KhaiPha_Mining
    {
        private string connectionString = "Provider=MSOLAP;Data Source=THAO\\MSSQLSERVER19;Initial Catalog=testDatamining;Integrated Security=SSPI;";

        // Thực hiện truy vấn DMX
        public DataTable ExecuteDMXQuery(string query)
        {
            DataTable resultTable = new DataTable();

            try
            {
                // Kiểm tra xem truy vấn có phải là null hoặc rỗng không
                if (string.IsNullOrWhiteSpace(query))
                {
                    Console.WriteLine("Truy vấn DMX không hợp lệ: Truy vấn không được để trống.");
                    return null;
                }

                using (AdomdConnection connection = new AdomdConnection(connectionString))
                {
                    connection.Open();
                    using (AdomdCommand command = new AdomdCommand(query, connection))
                    {
                        // Thiết lập timeout cho việc thực thi truy vấn
                        command.CommandTimeout = 60; // Timeout 60 giây

                        using (AdomdDataAdapter adapter = new AdomdDataAdapter(command))
                        {
                            adapter.Fill(resultTable);
                        }
                    }
                }

                // Kiểm tra nếu DataTable không có dữ liệu
                if (resultTable.Rows.Count == 0)
                {
                    Console.WriteLine("Truy vấn không trả về dữ liệu.");
                }

                return resultTable;
            }
            catch (AdomdErrorResponseException adomdEx)
            {
                // Xử lý lỗi của AdomdClient khi truy vấn DMX bị lỗi
                Console.WriteLine("Lỗi DMX: " + adomdEx.Message);
                Console.WriteLine("Thông tin chi tiết lỗi: " + adomdEx.StackTrace);
                return null;
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi chung
                Console.WriteLine("Lỗi không xác định khi thực thi truy vấn DMX: " + ex.Message);
                Console.WriteLine("Thông tin chi tiết lỗi: " + ex.StackTrace);
                return null;
            }
        }

        // Lấy danh sách các Mining Model đã có sẵn
        public DataTable GetDataMiningModels()
        {
            string query = "SELECT MODEL_NAME FROM $SYSTEM.DMSCHEMA_MINING_MODELS"; // Truy vấn lấy danh sách Mining Models
            return ExecuteDMXQuery(query);
        }

        // Cập nhật phương thức lấy dữ liệu ATTRIBUTE_NAME không trùng lặp
        public DataTable GetDistinctAttributes()
        {
            string query = @"
        SELECT DISTINCT ATTRIBUTE_NAME
        FROM [Fact Kinh Doanh].CONTENT
        WHERE ATTRIBUTE_NAME IS NOT NULL
    ";

            return ExecuteDMXQuery(query);
        }

        // Lấy dữ liệu phân phối từ NODE_DISTRIBUTION
        public DataTable GetMiningLegendFromModel(string modelName)
        {
            string query = $@"
                SELECT ATTRIBUTE_VALUE, 
                       SUPPORT, 
                       PROBABILITY, 
                       NODE_SUPPORT AS TotalCases
                FROM [{modelName}].NODE_DISTRIBUTION
                WHERE NODE_TYPE = 3"; // Lọc các Node lá (leaf nodes)
            return ExecuteDMXQuery(query);
        }

        // Kiểm tra kết nối tới SSAS
        public bool TestConnection()
        {
            try
            {
                using (AdomdConnection connection = new AdomdConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi kết nối
                Console.WriteLine("Lỗi kết nối: " + ex.Message);
                return false;
            }
        }
    }
}
