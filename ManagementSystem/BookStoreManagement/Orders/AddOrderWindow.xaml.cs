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
                           FROM Products as P, ProductCategories as C
                           WHERE P.CategoryId = C.CategoryId";

            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            BindingList<Product> _lib = new BindingList<Product>();

            //ObservableCollection<Product> _lib = new ObservableCollection<Product>();

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
    }
}
