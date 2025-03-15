using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.Helpers
{
    public static class WaitFormHelper
    {
        public static void ShowWaitForm(DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager, string message = "Đang tải dữ liệu, vui lòng chờ...")
        {
            // Đặt vị trí hiển thị form
            splashScreenManager.SplashFormStartPosition = DevExpress.XtraSplashScreen.SplashFormStartPosition.CenterScreen;

            // Hiển thị WaitForm nếu chưa hiển thị
            if (!splashScreenManager.IsSplashFormVisible)
            {
                splashScreenManager.ShowWaitForm();
            }

            // Cập nhật nội dung hiển thị
            splashScreenManager.SetWaitFormDescription(message);
        }

        public static void CloseWaitForm(DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager)
        {
            if (splashScreenManager.IsSplashFormVisible)
            {
                splashScreenManager.CloseWaitForm();
            }
        }
    }

}
