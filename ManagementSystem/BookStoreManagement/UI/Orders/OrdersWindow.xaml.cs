using BookStoreManagement.Entities;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using System.Xml.Serialization;

namespace BookStoreManagement.Orders
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : UserControl
    {
        public OrdersWindow()
        {
            InitializeComponent();
        }

        private void Order_Loaded(object sender, RoutedEventArgs e)
        {
            OrderDateCombobox.SelectedIndex = 0;
            StatusCombobox.SelectedIndex = 0;
            LoadDefault();
            UpdatePageInfo();

        }

        //-----------------TÍNH NĂNG PHÂN TRANG-----------------
        private int currentPage = 1;
        private int itemsPerPage = GlobalData.itemsPerPageOrders; // Thay đổi số lượng mục trên mỗi trang theo ý bạn
        private int totalItems = 0;
        private string SortName = "";
        private void UpdatePageInfo()
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            PageInfo.Text = $"{currentPage}/{totalPages}";
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage();
                UpdatePageInfo();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            // tính tổng số trang bằng cách chia tổng số mục cho số mục trên mỗi trang
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage();
                UpdatePageInfo();
            }
        }

        private string sql_main = "";
        private string UpdateSql_Main(string name_ORDERBY)
        {
            string sql;
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                sql = sql_main + $@" AND OrderDate  >= '{StartDatePicker.Text} 00:00:00' AND OrderDate <= '{EndDatePicker.Text} 23:59:59'
                                     Order By {name_ORDERBY}
                                     OFFSET {(currentPage - 1) * itemsPerPage} ROWS
                                     FETCH NEXT {itemsPerPage} ROWS ONLY";
            }
            else
                sql = sql_main + $@" Order By {name_ORDERBY}
                                     OFFSET {(currentPage - 1) * itemsPerPage} ROWS
                                     FETCH NEXT {itemsPerPage} ROWS ONLY";
            return sql;
        }

        private int countSql_main()
        {
            BindingList<Order> _countLib = new BindingList<Order>();

            LoadDataBase(_countLib, sql_main);
  
            return _countLib.Count;
        }

        private void LoadDataBase(BindingList<Order> _lib, string sql)
        {
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var order = new Order()
                {
                    OrderId = (int)dt.Rows[i]["OrderID"],
                    CustomerName = (string)dt.Rows[i]["Name"],
                    OrderDate = (DateTime)dt.Rows[i]["OrderDate"],
                    Status = (string)dt.Rows[i]["Status"],
                    TotalAmount = (int)dt.Rows[i]["TotalAmount"],
           
                };
                _lib.Add(order);
            }
        }

        private void LoadDefault()
        {
            BindingList<Order> _lib = new BindingList<Order>();
        
            int countSql;
            sql_main = $@"SELECT * FROM Orders O, Customers C
                          Where O.CustomerID = C.CustomerID";
            SortName = "OrderID";
            string sql = UpdateSql_Main(SortName);

            LoadDataBase(_lib, sql);
            countSql = countSql_main();

            totalItems = countSql; //DBConnect.DB.QueryResult(countSql);
            OrderList.ItemsSource = _lib;//
        }

        private void LoadPage()
        {
            BindingList<Order> _lib = new BindingList<Order>();

            string sql = UpdateSql_Main(SortName);
            int countSql;
            
            //countSql = $@"SELECT COUNT(*) FROM Orders";

            LoadDataBase(_lib, sql);
            countSql = countSql_main();
            if (countSql > 0)
            {
                totalItems = countSql; //DBConnect.DB.QueryResult(countSql);
            }
            else
            {
                countSql = 1;
                totalItems = countSql;
            }
                
            OrderList.ItemsSource = _lib;

            //-----------------
        }

        public event EventHandler<OrderEventArgs>? NavigateToDetailOrder;
        private void detailOrderButton_Click(object sender, RoutedEventArgs e)
        {
            //Lấy đối tượng Order từ DataContext của Button
            var order = ((FrameworkElement)sender).DataContext as Order;

            if (order != null)
            {
                NavigateToDetailOrder?.Invoke(this, new OrderEventArgs(order));
            }
            else
                return;
        }


        private void OrderDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (OrderDateCombobox.SelectedItem != null)
            {
                //string? selectedValue = ((ComboBoxItem)OrderDateCombobox.SelectedItem).Content.ToString();
                string? selectedValue = ((ComboBoxItem)OrderDateCombobox.SelectedItem).Content.ToString();
                switch (selectedValue)
                {
                    case "⏬ Cũ nhất":
                        SortName = "OrderDate";
                        currentPage = 1;
                        OrderDateSort(SortName);
                        break;

                    case "⏫ Mới nhất":
                        SortName = "OrderDate DESC";
                        currentPage = 1;
                        OrderDateSort(SortName);
                        break;

                    case "⚙ Mặc định":
                        currentPage = 1;
                        DefaultOrderDateSort();
                        break;
                    default:
                        // Hành động mặc định hoặc xử lý lỗi
                        break;
                }
            }
            
        }

        private void OrderDateSort(string SortName)
        {

            if (StatusCombobox.SelectedValue != null && StatusCombobox.Text != "⚙ Mặc định") 
            {

                if (StatusCombobox.Text == "🚚 Đang giao hàng")
                {
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                         Where O.CustomerID = C.CustomerID AND O.Status = N'Đang giao hàng'";
                }
                else if (StatusCombobox.Text == "🔄 Đang xử lí")
                {
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Đang xử lí'";
                }
                else if (StatusCombobox.Text == "✅ Đã giao thành công")
                {
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Đã giao thành công'";
                }
                else //if (StatusCombobox.Text == "❌ Hủy đơn")
                {
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Hủy đơn'";
                }
            }
            else
                sql_main = $@"SELECT * FROM Orders O, Customers C
                      Where O.CustomerID = C.CustomerID";

            LoadPage();
            UpdatePageInfo();

        }

       
        private void StatusCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string SttName = "";
            if (StatusCombobox.SelectedItem != null)
            {            
                string? selectedValue = ((ComboBoxItem)StatusCombobox.SelectedItem).Content.ToString();
                switch (selectedValue)
                {
                    case "🚚 Đang giao hàng":
                        SttName = "Đang giao hàng";
                        currentPage = 1;
                        StatusSort(SttName);
                        break;

                    case "🔄 Đang xử lí":
                        SttName = "Đang xử lí";
                        currentPage = 1;
                        StatusSort(SttName);
                        break;

                    case "✅ Đã giao thành công":
                        SttName = "Đã giao thành công";
                        currentPage = 1;
                        StatusSort(SttName);
                        break;

                    case "❌ Hủy đơn":
                        SttName = "Hủy đơn";
                        currentPage = 1;
                        StatusSort(SttName);
                        break;

                    case "⚙ Mặc định":
                        currentPage = 1;
                        DefaultStatusSort();
                        break;
                    default:
                        // Hành động mặc định hoặc xử lý lỗi
                        break;
                }
            }

            
        }

        private void StatusSort(string SttName)
        {

         
            if (OrderDateCombobox.SelectedValue != null && OrderDateCombobox.Text != "⚙ Mặc định")
            {
                if (OrderDateCombobox.Text == "⏬ Cũ nhất")
                {
                    SortName = "OrderDate";
                    sql_main = $@"SELECT * FROM Orders O, Customers C
                                  Where O.CustomerID = C.CustomerID AND O.Status = N'{SttName}'";
                }
                else //if (OrderDateCombobox.Text == "⏫ Mới nhất")
                {
                    SortName = "OrderDate DESC";
                    sql_main = $@"SELECT * FROM Orders O, Customers C
                                  Where O.CustomerID = C.CustomerID AND O.Status = N'{SttName}'";
                }
            }
            else
            {
                SortName = "OrderID";
                sql_main = $@"SELECT * FROM Orders O, Customers C
                              Where O.CustomerID = C.CustomerID AND O.Status = N'{SttName}'";
            }

            LoadPage();
            UpdatePageInfo();
        }

        private void DefaultStatusSort()
        {
            //string sql;
            if (OrderDateCombobox.Text == "⚙ Mặc định" || OrderDateCombobox.Text.Length == 0)
            { 
                LoadDefault();
                UpdatePageInfo();
                return;
            }
            else
            {
                if (OrderDateCombobox.Text == "⏬ Cũ nhất")
                {
                    SortName = "OrderDate";
                    sql_main = $@"SELECT * FROM Orders O, Customers C
                                  Where O.CustomerID = C.CustomerID";
                }
                else //if (OrderDateCombobox.Text == "⏫ Mới nhất")
                {
                    SortName = "OrderDate DESC";
                    sql_main = $@"SELECT * FROM Orders O, Customers C
                                  Where O.CustomerID = C.CustomerID";

                }
            }
            BindingList<Order> _lib = new BindingList<Order>();

            LoadPage();
            UpdatePageInfo();

        }

        private void DefaultOrderDateSort()
        {
           // string sql;
            if (StatusCombobox.Text == "⚙ Mặc định" || StatusCombobox.Text.Length == 0)
            {
                LoadDefault();
                UpdatePageInfo();
                return;
            }
            else
            {
                if (StatusCombobox.Text == "🚚 Đang giao hàng")
                {
                    SortName = "OrderID";
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                                  Where O.CustomerID = C.CustomerID AND O.Status = N'Đang giao hàng'";
                }
                else if (StatusCombobox.Text == "🔄 Đang xử lí")
                {
                    SortName = "OrderID";
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                                  Where O.CustomerID = C.CustomerID AND O.Status = N'Đang xử lí'";
                }
                else if (StatusCombobox.Text == "✅ Đã giao thành công")
                {
                    SortName = "OrderID";
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                                  Where O.CustomerID = C.CustomerID AND O.Status = N'Đã giao thành công'";
                }
                else //if (StatusCombobox.Text == "❌ Hủy đơn")
                {
                    SortName = "OrderID";
                    sql_main = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Hủy đơn'";
                }
            }
            BindingList<Order> _lib = new BindingList<Order>();

            LoadPage();
            UpdatePageInfo();


        }

        public event EventHandler? NavigateToAddOrder;
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigateToAddOrder?.Invoke(this, EventArgs.Empty);
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            // Kiểm tra xem ngày bắt đầu và ngày kết thúc đã được chọn hay chưa
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                
                // Lấy giá trị ngày bắt đầu và ngày kết thúc
                DateTime startDate = StartDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;

                // Kiểm tra nếu ngày kết thúc trước ngày bắt đầu
                if (endDate < startDate)
                {
                    // Hiển thị thông báo lỗi
                    MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);

                    // Đặt ngày kết thúc về null hoặc một giá trị mặc định
                    EndDatePicker.SelectedDate = null; // hoặc bạn có thể đặt ngày mặc định khác
                }
                else
                {
                    LoadOrderDate();
                    UpdatePageInfo();
                }
            }
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {           
            // Kiểm tra xem ngày bắt đầu và ngày kết thúc đã được chọn hay chưa
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                // Lấy giá trị ngày bắt đầu và ngày kết thúc
                DateTime startDate = StartDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;

                // Kiểm tra nếu ngày kết thúc trước ngày bắt đầu
                if (endDate < startDate)
                {
                    // Hiển thị thông báo lỗi
                    MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);

                    // Đặt ngày kết thúc về null hoặc một giá trị mặc định
                    EndDatePicker.SelectedDate = null; // hoặc bạn có thể đặt ngày mặc định khác
                }
                else
                {
                    LoadOrderDate();
                    UpdatePageInfo();
                }
            }
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            LoadDefault();
            UpdatePageInfo();
        }

        private void LoadOrderDate()
        {
            currentPage = 1; //Đặt lại từ trang 1

            BindingList<Order> _lib = new BindingList<Order>();

            int countSql;
            sql_main = $@"SELECT * FROM Orders O, Customers C
                          Where O.CustomerID = C.CustomerID";

            string sql = UpdateSql_Main("OrderID");

            LoadDataBase(_lib, sql);

            countSql = countSql_main();
            
            if (countSql > 0)
            {
                totalItems = countSql; //DBConnect.DB.QueryResult(countSql);
            }
            else 
                totalItems = 1;
            OrderList.ItemsSource = _lib;
        }
    }
}
