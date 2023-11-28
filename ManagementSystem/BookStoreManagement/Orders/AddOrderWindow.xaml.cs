using BookStoreManagement.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace BookStoreManagement.Orders
{
    /// <summary>
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : UserControl
    {
        public AddOrderWindow()
        {
            InitializeComponent();

            
        }

        private void AddOrder_Loaded(object sender, RoutedEventArgs e)
        {
            //Truyền dữ liệu vào ComboBox Khách hàng
            BindingCustomers();

            //Truyền dữ liệu vào ComboBox Sản phẩm
            BindingProducts();

            Order newOrder = new Order();

            newOrder.OrderDate = DateTime.Now;
            txtOrderDate.Text = newOrder.OrderDate.ToString(); 
        }

        //Truyền dữ liệu vào ô ComboBox của Khách hàng
        private void BindingCustomers()
        {
            string sql = @"SELECT * FROM Customers";
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            ObservableCollection<Customers> _lib = new ObservableCollection<Customers>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var customer = new Customers()
                {
                    CustomerID = (int)dt.Rows[i]["CustomerID"],
                    Name = (string)dt.Rows[i]["Name"],
                    Sex = (string)dt.Rows[i]["SEX"],
                    Phone = (string)dt.Rows[i]["Phone"],
                    Email = (string)dt.Rows[i]["Email"],
                    Address = (string)dt.Rows[i]["Address"],
                };
                _lib.Add(customer);
            }

            txtCustomer.ItemsSource = _lib;
        }

        //Truyền dữ liệu vào ô ComboBox của Sản phẩm
        private void BindingProducts()
        {
            string sql = @"SELECT* 
                           FROM Products P, ProductCategories C
                           WHERE P.CategoryId = C.CategoryId";

            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            //BindingList<Product> _lib = new BindingList<Product>();

            ObservableCollection<Product> _lib = new ObservableCollection<Product>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var product = new Product()
                {
                    BookId = (int)dt.Rows[i]["ProductId"],
                    BookName = (string)dt.Rows[i]["ProductName"],
                    Price = (int)dt.Rows[i]["Price"],
                    Author = (string)dt.Rows[i]["Author"],
                    Quantity = (int)dt.Rows[i]["Quantity"],
                    CategoryId = (int)dt.Rows[i]["CategoryId"],
                    CategoryName = (string)dt.Rows[i]["CategoryName"],
                    Description = (string)dt.Rows[i]["Description"],
                    ImageURL = (string)dt.Rows[i]["ImageURL"],
                };
                _lib.Add(product);
            }

            txtProduct.ItemsSource = _lib;
        }

        //Sự kiện của nút BackButton
        public event EventHandler? NavigateBackToOrdersWindow;
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackToOrdersWindow?.Invoke(this, EventArgs.Empty);
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomer = new NewCustomerWindow();
            if (newCustomer.ShowDialog() == true)
            {
                BindingCustomers();
            }
            else
                return;
        }

        private BindingList<OrderDetail> _lib = new BindingList<OrderDetail>();
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var Result = MessageBox.Show("Bạn có chắc chắn muốn lưu đơn hàng này ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (Result == MessageBoxResult.Yes)
            {
                //Xử lí không cho thay đổi dữ liệu khách hàng
                txtCustomer.IsEnabled = false;
                AddCustomer.IsEnabled = false;
                AddCustomer.Visibility = Visibility.Hidden;

                //Tạo 1 chi tiết đơn hàng

                var orderdetail = new OrderDetail
                {
                    //ProductName = book.BookName,
                    UnitPrice = toINT(txtUnitPrice.Text),
                    Quantity = toINT(txtQuantity.Text),
                    //Price = UnitPrice * Quantity,
                };
                _lib.Add(orderdetail);

                DetailOrderList.ItemsSource = _lib;
            }              
            else
                return;

            
        }

        //Hàm chuyển chuỗi về số nguyên
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
        private void AddQuantity_Click(object sender, RoutedEventArgs e)
        {
            int quantity;
            quantity = toINT(txtQuantity.Text);

            quantity++;
            txtQuantity.Text = quantity.ToString();      
        }

        private void SubQuantity_Click(object sender, RoutedEventArgs e)
        {

            int quantity;
            quantity = toINT(txtQuantity.Text);
            if (quantity == 1)
            {
                return;
            }
            else
            {
                quantity--;
                txtQuantity.Text = quantity.ToString();
            }
                

        }
    }
}
