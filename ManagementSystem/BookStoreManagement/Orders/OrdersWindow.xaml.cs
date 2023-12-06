using BookStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            LoadPage();
            
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

        private void LoadPage()
        {
            BindingList<Order> _lib = new BindingList<Order>();


            string sql;
            sql = $@"SELECT * FROM Orders O, Customers C
                      Where O.CustomerID = C.CustomerID";

            LoadDataBase(_lib, sql);
            OrderList.ItemsSource = _lib;
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
            string SortName = "";
            if (OrderDateCombobox.SelectedItem != null)
            {
                //string? selectedValue = ((ComboBoxItem)OrderDateCombobox.SelectedItem).Content.ToString();
                string? selectedValue = ((ComboBoxItem)OrderDateCombobox.SelectedItem).Content.ToString();
                switch (selectedValue)
                {
                    case "⏬ Cũ nhất":
                        SortName = "OrderDate";
                        OrderDateSort(SortName);
                        break;

                    case "⏫ Mới nhất":
                        SortName = "OrderDate DESC";
                        OrderDateSort(SortName);
                        break;

                    case "⚙ Mặc định":
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
            BindingList<Order> _lib = new BindingList<Order>();

            string sql;

            if (StatusCombobox.SelectedValue != null && StatusCombobox.Text != "⚙ Mặc định") 
            {

                if (StatusCombobox.Text == "🚚 Đang giao hàng")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                         Where O.CustomerID = C.CustomerID AND O.Status = N'Đang giao hàng'
                         ORDER BY " + SortName;
                }
                else if (StatusCombobox.Text == "🔄 Đang xử lí")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Đang xử lí'
                             ORDER BY " + SortName;
                }
                else if (StatusCombobox.Text == "✅ Đã giao thành công")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Đã giao thành công'
                             ORDER BY " + SortName;
                }
                else //if (StatusCombobox.Text == "❌ Hủy đơn")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Hủy đơn'
                             ORDER BY " + SortName;
                }
            }
            else
                sql = $@"SELECT * FROM Orders O, Customers C
                      Where O.CustomerID = C.CustomerID
                      ORDER BY {SortName}";

            LoadDataBase(_lib, sql);
            OrderList.ItemsSource = _lib;
        }

       
        private void StatusCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string SortName = "";
            if (StatusCombobox.SelectedItem != null)
            {            
                string? selectedValue = ((ComboBoxItem)StatusCombobox.SelectedItem).Content.ToString();
                switch (selectedValue)
                {
                    case "🚚 Đang giao hàng":
                        SortName = "Đang giao hàng";
                        StatusSort(SortName);
                        break;

                    case "🔄 Đang xử lí":
                        SortName = "Đang xử lí";
                        StatusSort(SortName);
                        break;

                    case "✅ Đã giao thành công":
                        SortName = "Đã giao thành công";
                        StatusSort(SortName);
                        break;

                    case "❌ Hủy đơn":
                        SortName = "Hủy đơn";
                        StatusSort(SortName);
                        break;

                    case "⚙ Mặc định":
                        DefaultStatusSort();
                        break;
                    default:
                        // Hành động mặc định hoặc xử lý lỗi
                        break;
                }
            }
        }

        private void StatusSort(string SortName)
        {
            BindingList<Order> _lib = new BindingList<Order>();

            string sql;
         

            if (OrderDateCombobox.SelectedValue != null && OrderDateCombobox.Text != "⚙ Mặc định")
            {
                if (OrderDateCombobox.Text == "⏬ Cũ nhất")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C
                      Where O.CustomerID = C.CustomerID AND O.Status = N'{SortName}' 
                      ORDER BY OrderDate";
                }
                else //if (OrderDateCombobox.Text == "⏫ Mới nhất")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C
                      Where O.CustomerID = C.CustomerID AND O.Status = N'{SortName}' 
                      ORDER BY OrderDate DESC";
                }
            }
            else
            {
                sql = $@"SELECT * FROM Orders O, Customers C
                      Where O.CustomerID = C.CustomerID AND O.Status = N'{SortName}'";
            }

            LoadDataBase(_lib, sql);
            OrderList.ItemsSource = _lib;
        }

        private void DefaultStatusSort()
        {
            string sql;
            if (OrderDateCombobox.Text == "⚙ Mặc định" || OrderDateCombobox.Text.Length == 0)
            { 
                LoadPage();
                return;
            }
            else
            {
                if (OrderDateCombobox.Text == "⏬ Cũ nhất")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C
                    Where O.CustomerID = C.CustomerID
                    ORDER BY OrderDate";
                }
                else //if (OrderDateCombobox.Text == "⏫ Mới nhất")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C
                    Where O.CustomerID = C.CustomerID
                    ORDER BY OrderDate DESC";

                }
            }
            BindingList<Order> _lib = new BindingList<Order>();
            LoadDataBase(_lib, sql);
            OrderList.ItemsSource = _lib;
        }

        private void DefaultOrderDateSort()
        {
            string sql;
            if (StatusCombobox.Text == "⚙ Mặc định" || StatusCombobox.Text.Length == 0)
            {
                LoadPage();
                return;
            }
            else
            {
                if (StatusCombobox.Text == "🚚 Đang giao hàng")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                         Where O.CustomerID = C.CustomerID AND O.Status = N'Đang giao hàng'";
                }
                else if (StatusCombobox.Text == "🔄 Đang xử lí")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Đang xử lí'";
                }
                else if (StatusCombobox.Text == "✅ Đã giao thành công")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Đã giao thành công'";
                }
                else //if (StatusCombobox.Text == "❌ Hủy đơn")
                {
                    sql = $@"SELECT * FROM Orders O, Customers C 
                             Where O.CustomerID = C.CustomerID AND O.Status = N'Hủy đơn'";
                }
            }
            BindingList<Order> _lib = new BindingList<Order>();
            LoadDataBase(_lib, sql);
            OrderList.ItemsSource = _lib;

        }

        public event EventHandler? NavigateToAddOrder;
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigateToAddOrder?.Invoke(this, EventArgs.Empty);
        }
    }
}
