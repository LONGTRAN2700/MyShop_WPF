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
using BookStoreManagement.Customers;
using BookStoreManagement.Entities;
using BookStoreManagement.Orders;
using BookStoreManagement.Products;
using BookStoreManagement.Statistic;

namespace BookStoreManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string username;
        public MainWindow(string Username)
        {
            InitializeComponent();
            username = Username;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Tab hiển thị mặc định là DashBoard
            MainContent.Content = new DashboardWindow();
            DashboardButton.Background = new SolidColorBrush(Colors.AliceBlue);

            //Lấy GlobalData.itemsPerPageProducts itemsPerPageOrders đã được lưu của tài khoản đăng nhập
            string sql = $@"SELECT * FROM USERS WHERE Username = N'{username}'";
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            GlobalData.itemsPerPageProducts = (int)dt.Rows[0]["CountProducts"];
            GlobalData.itemsPerPageOrders = (int)dt.Rows[0]["CountOrders"];
        }

        //------------------------------XỬ LÍ TÍNH NĂNG TRANG CHỦ--------------------------------//
        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DashboardWindow();

            DashboardButton.Background = new SolidColorBrush(Colors.AliceBlue);

            OrderButton.Background = new SolidColorBrush(Colors.Transparent);
            ProductButton.Background = new SolidColorBrush(Colors.Transparent);
            CustomerButton.Background = new SolidColorBrush(Colors.Transparent);
            Button.Background = new SolidColorBrush(Colors.Transparent);
        }
        //----------------------------------------------------------------------------------------//


        //------------------------XỬ LÍ TÍNH NĂNG ORDER------------------------------------------//
        //Xử lí bắt sự kiện cho màn hình Chi tiết ĐƠN HÀNG
        private void OrdersWindow_NavigateToDetailOrder(object sender, OrderEventArgs e)
        {

            var order = e.Order;

            var detailOrder = new DetailOrderWindow();
            detailOrder.SetOrder(order);

            // Tạo instance của DetailProduct và đăng ký sự kiện
            detailOrder.NavigateBackToOrdersWindow += Order_NavigateBackToOrdersWindow;

            MainContent.NavigationService.Navigate(detailOrder);
        }


        //Xử lí bắt sự kiện cho màn hình AddOrderWindow (Tạo đơn hàng)
        private void OrdersWindow_NavigateToAddOrder(object sender, EventArgs e)
        {
            var addOrder = new AddOrderWindow();

            // Tạo instance của DetailProduct và đăng ký sự kiện
            addOrder.NavigateBackToOrdersWindow += Order_NavigateBackToOrdersWindow;

            MainContent.NavigationService.Navigate(addOrder);
        }

        //Xử lí bắt sự kiện cho nút Back_Button của AddOrderWindow và DetailOrderWindow
        private void Order_NavigateBackToOrdersWindow(object sender, EventArgs e)
        {
            MainContent.NavigationService.GoBack();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            //Change color background when click to this button
            OrderButton.Background = new SolidColorBrush(Colors.AliceBlue);

            DashboardButton.Background = new SolidColorBrush(Colors.Transparent);
            ProductButton.Background = new SolidColorBrush(Colors.Transparent);
            CustomerButton.Background = new SolidColorBrush(Colors.Transparent);
            Button.Background = new SolidColorBrush(Colors.Transparent);
            //--------------------------------------------------
            // Tạo instance của OrdersWindow và đăng ký sự kiện
            var ordersWindow = new OrdersWindow();

            ordersWindow.NavigateToDetailOrder += OrdersWindow_NavigateToDetailOrder;
            ordersWindow.NavigateToAddOrder += OrdersWindow_NavigateToAddOrder;


            MainContent.Content = ordersWindow;
        }


        //------------------------------XỬ LÍ TÍNH NĂNG SẢN PHẨM--------------------------------//

        //Xử lí bắt sự kiện cho màn hình Chi tiết sản phẩm
        private void ProductsWindow_NavigateToDetailProduct(object sender, ProductEventArgs e)
        {

            var product = e.Product;

            var detailProduct = new DetailProductWindow();
            detailProduct.SetProduct(product);

            // Tạo instance của DetailProduct và đăng ký sự kiện
            detailProduct.NavigateBackToProductsWindow += DetailProduct_NavigateBackToProductsWindow;

            MainContent.NavigationService.Navigate(detailProduct);
        }

        //Xử lí bắt sự kiện cho nút Back_Button
        private void DetailProduct_NavigateBackToProductsWindow(object sender, EventArgs e)
        {
            MainContent.NavigationService.GoBack();
        }

        //Xử lí bắt sự kiện cho màn hình Thêm sản phẩm
        private void ProductsWindow_NavigateToAddProduct(object sender, EventArgs e)
        {
            var addProduct = new AddProductWindow();

            // Tạo instance của DetailProduct và đăng ký sự kiện
            addProduct.NavigateBackToProductsWindow += DetailProduct_NavigateBackToProductsWindow;

            MainContent.NavigationService.Navigate(addProduct);
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        { 
            ProductButton.Background = new SolidColorBrush(Colors.AliceBlue);

            DashboardButton.Background = new SolidColorBrush(Colors.Transparent);
            OrderButton.Background = new SolidColorBrush(Colors.Transparent);
            CustomerButton.Background = new SolidColorBrush(Colors.Transparent);
            Button.Background = new SolidColorBrush(Colors.Transparent);
            //---------------------------------------------------------------

            // Tạo instance của ProductsWindow và đăng ký sự kiện
            var productsWindow = new ProductsWindow();
            productsWindow.NavigateToDetailProduct += ProductsWindow_NavigateToDetailProduct;
            productsWindow.NavigateToAddProduct += ProductsWindow_NavigateToAddProduct;

            MainContent.Content = productsWindow;
        }

        //----------------------------------XỬ LÍ TÍNH NĂNG THỐNG KÊ---------------------------------------//


        private void StatisticButton_Click(object sender, RoutedEventArgs e)
        {
            Button.Background = new SolidColorBrush(Colors.AliceBlue);

            DashboardButton.Background = new SolidColorBrush(Colors.Transparent);
            OrderButton.Background = new SolidColorBrush(Colors.Transparent);
            ProductButton.Background = new SolidColorBrush(Colors.Transparent);
            CustomerButton.Background = new SolidColorBrush(Colors.Transparent);
            //-----------------------------------------------------------------

            var statisticWindow = new StatisticWindow();

            MainContent.Content = statisticWindow;
        }

        //----------------------------XỬ LÍ TÍNH NĂNG KHÁCH HÀNG--------------------------------------------

        //Xử lí bắt sự kiện cho màn hình Chi tiết khách hàng
        private void CustomersWindow_NavigateToDetailCustomer(object sender, CustomerEventArgs e)
        {

            var customer = e.Customer;

            var detailCustomer = new DetailCustomer();
            detailCustomer.SetCustomer(customer);

            // Tạo instance của DetailCustomer
            detailCustomer.NavigateBackToCustomersWindow += Customer_NavigateBackToCustomersWindow;

            MainContent.NavigationService.Navigate(detailCustomer);
        }

        //Xử lí bắt sự kiện cho nút Back_Button DetailCustomer
        private void Customer_NavigateBackToCustomersWindow(object sender, EventArgs e)
        {
            MainContent.NavigationService.GoBack();
        }
        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerButton.Background = new SolidColorBrush(Colors.AliceBlue);

            DashboardButton.Background = new SolidColorBrush(Colors.Transparent);
            OrderButton.Background = new SolidColorBrush(Colors.Transparent);
            ProductButton.Background = new SolidColorBrush(Colors.Transparent);
            Button.Background = new SolidColorBrush(Colors.Transparent);
            //-------------------------------------------------------------------

            var customerWindow = new CustomersWindow();
            customerWindow.NavigateToDetailCustomer += CustomersWindow_NavigateToDetailCustomer;

            MainContent.Content = customerWindow;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationPanel.Width == 120)
            {
                NavigationPanel.Width = 0; // Thu gọn
                MainContent.Margin = new Thickness(80,50, 80, 20); // Điều chỉnh MainContent
                                                                    // Cập nhật Icon của ToggleNavButton nếu cần
            }
            else
            {
                NavigationPanel.Width = 120; // Mở rộng
                MainContent.Margin = new Thickness(140, 50, 20, 20); // Điều chỉnh MainContent
                                                                  // Cập nhật Icon của ToggleNavButton nếu cần
            }
        }

        private void InforButton_Click(object sender, RoutedEventArgs e)
        {
            if (InforBorder.Visibility == Visibility.Collapsed)
            {   
                InforBorder.Visibility = Visibility.Visible;
            }   
            else
            {   
                InforBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new LoginWindow();
            screen.Show();
            this.Close();
        }

        //Hàm cập nhật lại số lượng sản phẩm 
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            var screenSetting = new SettingWindow(username);

            if (screenSetting.ShowDialog() == true)
            {
                //
            }
            else
                return;
        }

        private void InforUserButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InforUserWindow(username);
            if (screen.ShowDialog() == true)
            {
                //nothing
            }
            else return;
        }
    }
}
