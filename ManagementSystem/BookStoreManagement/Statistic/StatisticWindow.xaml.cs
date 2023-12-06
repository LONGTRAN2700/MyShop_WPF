using BookStoreManagement.DBConnect;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DocumentFormat.OpenXml.Drawing.Charts;


namespace BookStoreManagement.Statistic
{
    /// <summary>
    /// Interaction logic for StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow : UserControl
    {
        public StatisticWindow()
        {
            InitializeComponent();
        }

        //Xác định Ngày Bắt Đầu và Kết Thúc (theo ngày, tuần, tháng)


        /* -------------------ĐOẠN CODE ĐỂ XỬ LÍ TÍNH NĂNG THỐNG KÊ-----------------------*/

        public static (DateTime StartDate, DateTime EndDate) GetDateRange(string reportType)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = now.Date; // 0:00 của ngày hiện tại
            DateTime endDate = now; // Thời điểm hiện tại

            if (reportType == "Weekly")
            {
                // Tuần bắt đầu từ Thứ Hai
                int dayOfWeek = (int)now.DayOfWeek;
                int difference = (dayOfWeek == 0 ? 7 : dayOfWeek) - 1;
                startDate = now.Date.AddDays(-difference);
            }
            else if (reportType == "Monthly")
            {
                startDate = new DateTime(now.Year, now.Month, 1);
            }
            return (startDate, endDate);
        }

        static public System.Data.DataTable GetRevenueReport(string reportType)
        {
            var (startDate, endDate) = Statistic.StatisticWindow.GetDateRange(reportType);

            string sql = @"SELECT CONVERT(DATE, OrderDate) AS Date, SUM(TotalAmount) AS Revenue
                   FROM Orders
                   WHERE OrderDate BETWEEN @StartDate AND @EndDate
                   GROUP BY CONVERT(DATE, OrderDate);";

            var parameters = new Dictionary<string, object>
            {
                {"@StartDate", startDate},
                {"@EndDate", endDate}
             };

            return DBConnect.DB.LoadDB(sql, parameters);
        }
        static public System.Data.DataTable GetSalesReport(DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT P.ProductName, SUM(OD.Quantity) AS SoldQuantity, CONVERT(DATE, O.OrderDate) AS Date
                       FROM OrderDetails OD
                       INNER JOIN Orders O ON OD.OrderID = O.OrderID
                       INNER JOIN Products P ON OD.ProductID = P.ProductId
                       WHERE O.OrderDate BETWEEN @StartDate AND @EndDate
                       GROUP BY P.ProductName, CONVERT(DATE, O.OrderDate);";

            var parameters = new Dictionary<string, object>
        {
            {"@StartDate", startDate},
            {"@EndDate", endDate}
        };

            return DBConnect.DB.LoadDB(sql, parameters);
        }

        System.Data.DataTable dailyReport = GetRevenueReport("Daily");
        System.Data.DataTable weeklyReport = GetRevenueReport("Weekly");
        System.Data.DataTable monthlyReport = GetRevenueReport("Monthly");


        //----------------------------------------------------------------------
       
    }
}
