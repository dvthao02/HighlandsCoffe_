using System;
using System.Data;
using HighLandsCoffee_Manager.DAO;

namespace HighLandsCoffee_Manager.BUS
{
    class BUS_KhaiPha_Mining
    {
        private DAO_KhaiPha_Mining daoKhaiPhaMining;

        public BUS_KhaiPha_Mining()
        {
            // Khởi tạo đối tượng DAO
            daoKhaiPhaMining = new DAO_KhaiPha_Mining();
        }

        // Lấy danh sách các Mining Model đã có sẵn
        public DataTable GetDataMiningModels()
        {
            try
            {
                return daoKhaiPhaMining.GetDataMiningModels();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi trong BUS khi lấy danh sách Mining Models: " + ex.Message);
                return null; // Trả về null nếu có lỗi
            }
        }

        // Tự động lấy dữ liệu dự đoán từ Mining Model
        public DataTable GetDistinctAttributes(string modelName)
        {
            try
            {
                return daoKhaiPhaMining.GetDistinctAttributes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi trong BUS khi lấy dữ liệu dự đoán: " + ex.Message);
                return null; // Trả về null nếu có lỗi
            }
        }

        // Lấy dữ liệu phân phối từ NODE_DISTRIBUTION
        public DataTable GetMiningLegendFromModel(string modelName)
        {
            try
            {
                return daoKhaiPhaMining.GetMiningLegendFromModel(modelName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi trong BUS khi lấy dữ liệu phân phối từ mô hình: " + ex.Message);
                return null; // Trả về null nếu có lỗi
            }
        }

        // Kiểm tra kết nối tới SSAS
        public bool TestConnection()
        {
            try
            {
                return daoKhaiPhaMining.TestConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi trong BUS khi kiểm tra kết nối: " + ex.Message);
                return false; // Trả về false nếu không thể kết nối
            }
        }
    }
}
