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
            int OrderQuantity = (int)dt.Rows[0]["GrandTotalOrders"];
            tblOrderQuantity.Text = OrderQuantity.ToString();
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

        }


    }
}
