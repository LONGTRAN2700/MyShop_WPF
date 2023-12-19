using BookStoreManagement.DBConnect;
using DocumentFormat.OpenXml.Spreadsheet;
using LiveCharts.Wpf;
using LiveCharts;
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
//using LiveCharts;
//using LiveCharts.Wpf;

namespace BookStoreManagement.Statistic
{
    /// <summary>
    /// Interaction logic for StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow : UserControl
    {
        public SeriesCollection ColumnData { get; set; }
        public string[] Labels { get; set; }
        public Func<int, string> Values { get; set; }

        public StatisticWindow()
        {
            InitializeComponent();

            ColumnData = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<int> { 10, 30, 20 , 70, 60, 50, 70 }
                }
            };

            Labels = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", 
                             "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"};
            Values = value => value.ToString("N");

            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }





        //Xác định Ngày Bắt Đầu và Kết Thúc (theo ngày, tuần, tháng)

        ///* -------------------ĐOẠN CODE ĐỂ XỬ LÍ TÍNH NĂNG THỐNG KÊ-----------------------*/

        //public static (DateTime StartDate, DateTime EndDate) GetDateRange(string reportType)
        //{
        //    DateTime now = DateTime.Now;
        //    DateTime startDate = now.Date; // 0:00 của ngày hiện tại
        //    DateTime endDate = now; // Thời điểm hiện tại

        //    if (reportType == "Weekly")
        //    {
        //        // Tuần bắt đầu từ Thứ Hai
        //        int dayOfWeek = (int)now.DayOfWeek;
        //        int difference = (dayOfWeek == 0 ? 7 : dayOfWeek) - 1;
        //        startDate = now.Date.AddDays(-difference);
        //    }
        //    else if (reportType == "Monthly")
        //    {
        //        startDate = new DateTime(now.Year, now.Month, 1);
        //    }
        //    //else if (reportType == "Yearly")
        //    //{
        //    //    startDate = new DateTime(now.Year, 1, 1);
        //    //}
        //    return (startDate, endDate);
        //}

        //static public System.Data.DataTable GetRevenueReport(string reportType)
        //{
        //    var (startDate, endDate) = Statistic.StatisticWindow.GetDateRange(reportType);

        //    string sql = @"SELECT CONVERT(DATE, OrderDate) AS Date, SUM(TotalAmount) AS Revenue
        //           FROM Orders
        //           WHERE OrderDate BETWEEN @StartDate AND @EndDate
        //           GROUP BY CONVERT(DATE, OrderDate);";

        //    var parameters = new Dictionary<string, object>
        //    {
        //        {"@StartDate", startDate},
        //        {"@EndDate", endDate}
        //     };

        //    return DBConnect.DB.LoadDB(sql, parameters);
        //}
        //static public System.Data.DataTable GetSalesReport(DateTime startDate, DateTime endDate)
        //{
        //    string sql = @"SELECT P.ProductName, SUM(OD.Quantity) AS SoldQuantity, CONVERT(DATE, O.OrderDate) AS Date
        //               FROM OrderDetails OD
        //               INNER JOIN Orders O ON OD.OrderID = O.OrderID
        //               INNER JOIN Products P ON OD.ProductID = P.ProductId
        //               WHERE O.OrderDate BETWEEN @StartDate AND @EndDate
        //               GROUP BY P.ProductName, CONVERT(DATE, O.OrderDate);";

        //    var parameters = new Dictionary<string, object>
        //{
        //    {"@StartDate", startDate},
        //    {"@EndDate", endDate}
        //};

        //    return DBConnect.DB.LoadDB(sql, parameters);
        //}

        //System.Data.DataTable dailyReport = GetRevenueReport("Daily");
        ////System.Data.DataTable weeklyReport = GetRevenueReport("Weekly");
        ////System.Data.DataTable monthlyReport = GetRevenueReport("Monthly");
        ////System.Data.DataTable yearlyReport = GetRevenueReport("Yearly");
        ////-------------------------------------------------------------
        ////public static System.Data.DataTable GetDailyRevenueReport()
        ////{
        ////    string query = "SELECT CONVERT(DATE, OrderDate) AS Date, SUM(TotalAmount) AS Revenue " +
        ////                       "FROM Orders " +
        ////                       "GROUP BY CONVERT(DATE, OrderDate);";

        ////    System.Data.DataTable dt = DBConnect.DB.LoadCSDL(query);
        ////    return dt;
        ////}

        ////-------------------------------------------------------------
        //private double[] ConvertDataTableToDoubleArray(System.Data.DataTable dt, string columnName)
        //{
        //    return dt.AsEnumerable().Select(row => row.Field<double>(columnName)).ToArray();
        //}

        ////private string[] ConvertDataTableToStringArray(System.Data.DataTable dt, string columnName)
        ////{
        ////    return dt.AsEnumerable().Select(row => row.Field<DateTime>(columnName).ToString("dd/MM/yyyy")).ToArray();
        ////}

        //public void DrawDailyCharts()
        //{
        //    var dailyChartData = ConvertDataTableToDoubleArray(dailyReport, "Revenue");
        //    //var dailyLabels = ConvertDataTableToStringArray(dailyReport, "Date");

        //    // Tạo và cấu hình biểu đồ cột cho doanh thu hàng ngày
        //    var dailyChart = new XYChart(300, 200);
        //    dailyChart.addTitle("Doanh Thu Hàng Ngày");
        //    dailyChart.setPlotArea(25, 25, 250, 150);
        //    dailyChart.addBarLayer(dailyChartData);
        //    //dailyChart.xAxis().setLabels(dailyLabels);

        //    //Gọi và vẽ
        //    dailyChartHost.Child = new System.Windows.Forms.PictureBox
        //    {
        //        Image = dailyChart.makeImage(),
        //        SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        //    };



        //}

        ////public void DrawWeeklyChart()
        ////{
        ////    var weeklyChartData = ConvertDataTableToDoubleArray(weeklyReport, "Revenue");
        ////    var weeklyLabels = weeklyReport.AsEnumerable().Select(row => "Tuần " + row.Field<int>("WeekNumber").ToString()).ToArray();

        ////    // Tạo và cấu hình biểu đồ cột cho doanh thu hàng tuần
        ////    var weeklyChart = new XYChart(300, 200);
        ////    weeklyChart.addTitle("Doanh Thu Hàng Tuần");
        ////    weeklyChart.setPlotArea(25, 25, 250, 150);
        ////    weeklyChart.addBarLayer(weeklyChartData);
        ////    weeklyChart.xAxis().setLabels(weeklyLabels);

        ////    //Gọi và vẽ
        ////    weeklyChartHost.Child = new System.Windows.Forms.PictureBox
        ////    {
        ////        Image = weeklyChart.makeImage(),
        ////        SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        ////    };
        ////}

        ////public void DrawMonthlyChart()
        ////{
        ////    var monthlyChartData = ConvertDataTableToDoubleArray(monthlyReport, "Revenue");
        ////    var monthlyLabels = monthlyReport.AsEnumerable().Select(row => "Tháng " + row.Field<int>("Month").ToString()).ToArray();

        ////    // Tạo và cấu hình biểu đồ tròn cho doanh thu hàng tháng
        ////    var monthlyChart = new ChartDirector.PieChart(300, 200);
        ////    monthlyChart.addTitle("Doanh Thu Hàng Tháng");
        ////    monthlyChart.setData(monthlyChartData, monthlyLabels);

        ////    //Gọi và vẽ
        ////    monthlyChartHost.Child = new System.Windows.Forms.PictureBox
        ////    {
        ////        Image = monthlyChart.makeImage(),
        ////        SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        ////    };
        ////}

        ////public void DrawYearlyChart()
        ////{
        ////    var yearlyChartData = ConvertDataTableToDoubleArray(yearlyReport, "Revenue");
        ////    var yearlyLabels = yearlyReport.AsEnumerable().Select(row => "Năm " + row.Field<int>("Year").ToString()).ToArray();

        ////    // Tạo và cấu hình biểu đồ tròn cho doanh thu hàng năm
        ////    var yearlyChart = new ChartDirector.PieChart(300, 200);
        ////    yearlyChart.addTitle("Doanh Thu Hàng Năm");
        ////    yearlyChart.setData(yearlyChartData, yearlyLabels);

        ////    //Gọi và vẽ
        ////    yearlyChartHost.Child = new System.Windows.Forms.PictureBox
        ////    {
        ////        Image = yearlyChart.makeImage(),
        ////        SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        ////    };
        ////}

        //private void StatisticWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    DrawDailyCharts();
        //    //DrawWeeklyChart();
        //    //DrawMonthlyChart();
        //    //DrawYearlyChart();
        //}



        //private void CreateDailyChart()
        //{
        //    System.Data.DataTable dailyData = GetDailyData();
        //    var dates = dailyData.AsEnumerable().Select(row => row.Field<DateTime>("Date").ToShortDateString()).ToArray();
        //    var totalRevenues = dailyData.AsEnumerable().Select(row => row.Field<decimal>("TotalRevenue")).ToArray();
        //    var totalProfits = dailyData.AsEnumerable().Select(row => row.Field<decimal>("TotalProfit")).ToArray();

        //    // Create a Windows Forms control (WinChartViewer)
        //    var viewer = new WinChartViewer();
        //    viewer.Size = new System.Drawing.Size(600, 400);

        //    // Create the ChartDirector chart as usual
        //    var c = new XYChart(600, 400);
        //    c.addTitle("Daily Revenue and Profit");
        //    // Configure the chart...

        //    // Add data to chart...

        //    // Display chart in the Windows Forms control
        //    viewer.Image = c.makeImage();

        //    // Add the Windows Forms control to the WindowsFormsHost
        //    DailyChartHost.Child = viewer;
        //}

        //private System.Data.DataTable GetDailyData()
        //{
        //    string sql = @"SELECT 
        //                    CAST(OrderDate AS DATE) as Date,
        //                    SUM(TotalAmount) as TotalRevenue,
        //                    SUM(OD.Price * OD.Quantity) as Sales,
        //                    SUM((OD.Price - P.Price) * OD.Quantity) as TotalProfit
        //                FROM 
        //                    Orders O
        //                JOIN 
        //                    OrderDetails OD ON O.OrderID = OD.OrderID
        //                JOIN 
        //                    Products P ON OD.ProductID = P.ProductId
        //                GROUP BY 
        //                    CAST(OrderDate AS DATE)";
        //    return DB.LoadDB(sql);
        //}

    }
}
