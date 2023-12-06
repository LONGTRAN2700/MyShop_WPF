using BookStoreManagement.Entities;
using ClosedXML;
using DocumentFormat.OpenXml.Drawing.Charts;
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

            Entities.Order newOrder = new Entities.Order();

            newOrder.OrderDate = DateTime.Now;
            txtOrderDate.Text = newOrder.OrderDate.ToString(); 
        }

        //Truyền dữ liệu vào ô ComboBox của Khách hàng
        private void BindingCustomers()
        {
            string sql = @"SELECT * FROM Customers";
            System.Data.DataTable dt = DBConnect.DB.LoadCSDL(sql);

            ObservableCollection<Entities.Customers> _lib = new ObservableCollection<Entities.Customers>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var customer = new Entities.Customers()
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

            System.Data.DataTable dt = DBConnect.DB.LoadCSDL(sql);

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
            try
            {
                //Lấy OrderID vừa được tạo 
                string sqlReadOrder = @"SELECT * FROM Orders WHERE OrderDate = '" + txtOrderDate.Text + "' ";
                System.Data.DataTable dtOrder = DBConnect.DB.LoadCSDL(sqlReadOrder);
                orderID = (int)dtOrder.Rows[0]["OrderId"];

                if ((int)dtOrder.Rows[0]["TotalAmount"] == 0)
                {
                    var result = MessageBox.Show("Đơn hàng của bạn chưa được lưu, bạn có thật sự muốn thoát", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes) //Bấm thoát sẽ không khởi tạo hóa đơn
                    {
                        //Xóa đi đơn hàng rỗng vừa tạo trong database
                        string sql_deleteOrder = @"DELETE FROM Orders WHERE OrderID = " + orderID;
                        DBConnect.DB.Query(sql_deleteOrder);
                        NavigateBackToOrdersWindow?.Invoke(this, EventArgs.Empty); //Quay về màn hình trước đó
                    }
                    else
                        return;
                }
                else
                    NavigateBackToOrdersWindow?.Invoke(this, EventArgs.Empty); //Quay về màn hình trước đó
            }
            catch 
            {
                NavigateBackToOrdersWindow?.Invoke(this, EventArgs.Empty); //Quay về màn hình trước đó
            }
          
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
        private int orderID;
        private int TotalAmount = 0;
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //Kiểm tra ô combobox khách hàng đã điền hay chưa
            if (txtCustomer.Text.Length == 0)
            {
                txtErrorCustomer.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                txtErrorCustomer.Visibility = Visibility.Hidden;
            }

            //Kiểm tra ô combobox sản phẩm đã điền hay chưa
            if (txtProduct.Text.Length == 0)
            {
                txtErrorProduct.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                txtErrorProduct.Visibility = Visibility.Hidden;
            }

            //Kiểm tra số lượng mặt hàng có đáp ứng đủ hay không
            //Lấy số lượng sách hiện tại
            Product? selectedProduct = txtProduct.SelectedItem as Product;
            if (selectedProduct != null)
            {
                string sql_GetQuantityProduct = @"SELECT * FROM Products WHERE ProductId = " + selectedProduct.BookId;
                System.Data.DataTable dtProduct = DBConnect.DB.LoadCSDL(sql_GetQuantityProduct);
                int totalQuantity = (int)dtProduct.Rows[0]["Quantity"];
                if (toINT(txtQuantity.Text) > totalQuantity)
                {
                    var notify = MessageBox.Show($"Số lượng sách hiện tại còn {totalQuantity}. Không đủ để thêm vào", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
                return;

            var Result = MessageBox.Show("Bạn có chắc chắn muốn lưu đơn hàng này ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (Result == MessageBoxResult.Yes)
            {
                // Lấy dữ liệu từ combobox Customer
                ComboBox customerComboBox = txtCustomer as ComboBox;
                if (customerComboBox == null)
                {
                    return;
                }

                var selectedCustomer = customerComboBox.SelectedItem as Entities.Customers;
                if (selectedCustomer == null)
                {
                    return;
                }

                int customerID = selectedCustomer.CustomerID;

                //Tạo 1 đơn hàng mới
                             
                if (txtCustomer.IsEnabled == true)
                {
                    string sqlCreateOrder;
                    sqlCreateOrder = @"INSERT INTO Orders (CustomerID, OrderDate, Status, TotalAmount) VALUES (" + customerID + ", '" + txtOrderDate.Text + "', N'Đang xử lí', " + 0 + ")";
                    DBConnect.DB.Query(sqlCreateOrder);
                }
               
                //Lấy OrderID vừa được tạo 
                string sqlReadOrder = @"SELECT * FROM Orders WHERE OrderDate = '" + txtOrderDate.Text + "' ";
                System.Data.DataTable dtOrder = DBConnect.DB.LoadCSDL(sqlReadOrder);
                orderID = (int)dtOrder.Rows[0]["OrderId"];

                //--------------------------------------------------------------------------------------------------------------
                //Xử lí không cho thay đổi dữ liệu khách hàng
                txtCustomer.IsEnabled = false;
                AddCustomer.IsEnabled = false;
                AddCustomer.Visibility = Visibility.Hidden;

                //Tạo 1 chi tiết đơn hàng
                //Product? selectedProduct = txtProduct.SelectedItem as Product;

                if (selectedProduct != null)
                {
                    var orderdetail = new OrderDetail
                    {
                        OrderId = orderID,
                        ProductId = selectedProduct.BookId,
                        ProductName = selectedProduct.BookName,
                        UnitPrice = toINT(txtUnitPrice.Text),
                        Quantity = toINT(txtQuantity.Text),
                    };
                    orderdetail.Price = orderdetail.UnitPrice * orderdetail.Quantity;
                    _lib.Add(orderdetail);

                    //Cập nhật ô tổng tiền
                    TotalAmount = TotalAmount + orderdetail.Price;
                    txtTotalAmount.Text = TotalAmount.ToString();
                }
                else
                    return;

                DetailOrderList.ItemsSource = _lib;
                txtQuantity.Text = "1";



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

        private void deleteOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            var reusult = MessageBox.Show("Bạn có chắc chắn muốn xóa mặt hàng này", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (reusult == MessageBoxResult.Yes)
            {
                var button = sender as Button;
                if (button != null)
                {
                    var itemToDelete = button.DataContext as OrderDetail;
                    if (itemToDelete != null)
                        _lib.Remove(itemToDelete);
                    else
                        return;
                }
                else
                    return;               
              
            }
            else
                return;
            
        }

        private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_lib.Count == 0)
            {
                MessageBox.Show("Đơn hàng rỗng. Khởi tạo không thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Bạn có chắc chắn muốn lưu đơn hàng này", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (Result == MessageBoxResult.Yes)
            {                
                    //Tạo 1 chi tiết đơn hàng
                    string sqlCreateOrderDetail;
                    for (int i = 0; i < _lib.Count; i++)
                    {
                        //Thêm chi tiết đơn hàng vào CSDL
                        sqlCreateOrderDetail = @"INSERT INTO OrderDetails (OrderID, ProductID, Quantity) VALUES (" + _lib[i].OrderId + ", " + _lib[i].ProductId + ", " + _lib[i].Quantity + ")";
                        DBConnect.DB.Query(sqlCreateOrderDetail);


                        //Lấy số lượng sách hiện tại
                        string sql_GetQuantityProduct = @"SELECT * FROM Products WHERE ProductId = " + _lib[i].ProductId;
                        System.Data.DataTable dtProduct = DBConnect.DB.LoadCSDL(sql_GetQuantityProduct);
                        int totalQuantity = (int)dtProduct.Rows[0]["Quantity"];

                        //Cập nhật lại số lượng sách có trong kho
                        int finalQuantity = totalQuantity - _lib[i].Quantity;
                        string sql_UpdateQuantityProduct = @"UPDATE Products
                                                             SET Quantity = " + finalQuantity +
                                                             " WHERE ProductID = " + _lib[i].ProductId;
                        DBConnect.DB.Query(sql_UpdateQuantityProduct);
                    }

                    var ResultOK = MessageBox.Show("Đã lưu đơn hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (ResultOK == MessageBoxResult.OK)
                    {
                        NavigateBackToOrdersWindow?.Invoke(this, EventArgs.Empty); //Gọi sự kiện nút BackButton để quay lại màn hình Order
                    }
            }
            else
                return;
           
        }
    }
}
