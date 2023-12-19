using DocumentFormat.OpenXml.Bibliography;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace BookStoreManagement
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : UserControl
    {      
        public DashboardWindow()
        {
            InitializeComponent();          
        }

        public SeriesCollection? ColumnData { get; set; }
        public ColumnSeries? columnSeries { get; set; }
        public SeriesCollection? DoughNutData { get; set; }
        public PieSeries? pieSeries { get; set; }
        public string[]? Labels { get; set; }
        public Func<int, string>? Values { get; set; }
        private int yearNow = DateTime.Now.Year;
        public void ColumnChart(int year)
        {
            columnSeries = new ColumnSeries
            {
                Title = "Doanh thu mỗi tháng",
                Values = new ChartValues<int>
                {
                        GetRevenueInMonth(1, year),
                        GetRevenueInMonth(2, year),
                        GetRevenueInMonth(3 , year),
                        GetRevenueInMonth(4, year),
                        GetRevenueInMonth(5, year),
                        GetRevenueInMonth(6, year),
                        GetRevenueInMonth(7, year),
                        GetRevenueInMonth(8, year),
                        GetRevenueInMonth(9, year),
                        GetRevenueInMonth(10, year),
                        GetRevenueInMonth(11, year),
                        GetRevenueInMonth(12, year),
                },
            
            };
            columnChartName.DataContext = columnSeries;
            

            Labels = new[] { "tháng 1", "tháng 2", "tháng 3", "tháng 4", "tháng 5", "tháng 6",
                             "tháng 7", "tháng 8", "tháng 9", "tháng 10", "tháng 11", "tháng 12"};
            Values = value => value.ToString("N");

            DataContext = this;
        }
        private int GetRevenueInMonth (int month, int year)
        {
            string sql = $@"SELECT COALESCE(SUM(TotalAmount), 0)
                            FROM Orders
                            WHERE DATEPART(MONTH, OrderDate) = {month} AND DATEPART(YEAR, OrderDate) = {year}";
            return DBConnect.DB.QueryResult(sql);
        }

        private void DoughNutChart(int year)
        {

            pieSeries = new PieSeries
            {
                Title = "Đã giao thành công",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(GetStatusInOrders("Đã giao thành công", year))
                },
                DataLabels = true
            };
            pieChart1.DataContext = pieSeries;

            pieSeries = new PieSeries
            {
                Title = "Đang xử lí",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(GetStatusInOrders("Đang xử lí", year))
                },
                DataLabels = true
            };
            pieChart2.DataContext = pieSeries;

            pieSeries = new PieSeries
            {
                Title = "Hủy đơn",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(GetStatusInOrders("Hủy đơn", year))
                },
                DataLabels = true
            };
            pieChart3.DataContext = pieSeries;

            pieSeries = new PieSeries
            {
                Title = "Đang giao hàng",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(GetStatusInOrders("Đang giao hàng", year))
                },
                DataLabels = true
            };
            pieChart4.DataContext = pieSeries;


            DataContext = this;
        }

        private int GetStatusInOrders(string status, int year)
        {
            string sql = $@"SELECT COUNT(*) FROM Orders
                            WHERE Status= N'{status}' AND DATEPART(YEAR, OrderDate) = {year}";
            return DBConnect.DB.QueryResult(sql);
        }

        private void GetOrderInWeekReport()
        {
            DateTime now = DateTime.Now;
            DateTime startDate = now.Date; // 0:00 của ngày hiện tại
            DateTime endDate = now; // Thời điểm hiện tại

            
            // Tuần bắt đầu từ Thứ Hai
            int dayOfWeek = (int)now.DayOfWeek;
            int difference = (dayOfWeek == 0 ? 7 : dayOfWeek) - 1;
            startDate = now.Date.AddDays(-difference);

            string sql = @$"SELECT SUM(TotalOrders) AS GrandTotalOrders
                            FROM (
                                  SELECT CONVERT(DATE, OrderDate) AS Date, COUNT(OrderID) AS TotalOrders
                                  FROM Orders
                                  WHERE OrderDate BETWEEN '{startDate}' AND '{endDate}'
                                  GROUP BY CONVERT(DATE, OrderDate)
                                 ) AS Subquery";
            DataTable dt =  DBConnect.DB.LoadCSDL(sql);
            try
            {
                int OrderQuantity = (int)dt.Rows[0]["GrandTotalOrders"];
                tblOrderQuantity.Text = OrderQuantity.ToString();
            }
            catch
            {
                tblOrderQuantity.Text = "0";
            }
        }

        //Số sản phẩm đang bán
        private void GetProducts()
        {
            string sql = @"SELECT COUNT(*) FROM Products";
            int count = DBConnect.DB.QueryResult(sql);
            tblProduct.Text = count.ToString();
        }

        private void GetRunOutOfStock()
        {
            string sql = @"SELECT COUNT(*) FROM Products
                           WHERE Quantity < 5";
            int count = DBConnect.DB.QueryResult(sql);
            tblStock.Text = count.ToString();
        }

        private void GetCustomer()
        {
            string sql = @"SELECT COUNT(*) FROM Customers";
            int count = DBConnect.DB.QueryResult(sql);
            tblCustomer.Text = count.ToString();
        }
        private void DashBoard_Loaded(object sender, RoutedEventArgs e)
        {
            //PRODUCTS
            GetProducts();

            //ORDERS
            GetOrderInWeekReport();

            //RUNNING OUT OF STOCK
            GetRunOutOfStock();

            //CUSTOMER
            GetCustomer();

            //Biểu đồ cột
            ColumnChart(yearNow);

            //Biểu đồ tròn
            DoughNutChart(yearNow);
        }
        private int toINT(string text)
        {
            // Chuyển đổi chuỗi thành số nguyên
            if (int.TryParse(text, out int number))
            {
                return number;
            }
            else
            {
                return 0;
            }
        }

        private void YearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedYear = ((ComboBoxItem)YearCombobox.SelectedItem).Content.ToString();
            if (selectedYear != null)
            {
                ColumnChart(toINT(selectedYear));
                DoughNutChart(toINT(selectedYear));
            }
            else
                return;
            
        }
    }
}
