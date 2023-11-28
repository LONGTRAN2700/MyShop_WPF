using BookStoreManagement.Entities;
using System;
using System.Collections.Generic;
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

namespace BookStoreManagement.Products
{
    /// <summary>
    /// Interaction logic for DetailProductWindow.xaml
    /// </summary>
    public partial class DetailProductWindow : UserControl
    {
        public DetailProductWindow()
        {
            InitializeComponent();
        }

        public void SetProduct(Product product)
        {
            // Thiết lập đối tượng Product cho DetailProduct
            this.DataContext = product; // Điều này cho phép bạn sử dụng ràng buộc dữ liệu trong DetailProduct
        }

        private void DetailProduct_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public event EventHandler? NavigateBackToProductsWindow;

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackToProductsWindow?.Invoke(this, EventArgs.Empty);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as Product;

            if(product != null)
            {
                ProductViewModel? viewModel = this.DataContext as ProductViewModel;
                var updateProduct = new UpdateProductWindow
                {
                    DataContext = viewModel
                };
                updateProduct.SetProduct(product);

                if (updateProduct.ShowDialog() == true)
                {
                    string sql = @"UPDATE Products
                                   SET Author = N'" + txtAuthor.Text + "', " +
                                     " Description = N'" + txtDescription.Text + "', " +
                                     " Price = " + ConvertTextToInteger(txtPrice.Text) + ", " +
                                     " CategoryId = " + getCategoryID(txtCategoryName.Text) + ", " +
                                     " Quantity = " + ConvertTextToInteger(txtQuantity.Text) +
                                   " WHERE ProductId = " + txtBookID.Text;
                    DBConnect.DB.Query(sql);
                }
                else
                    return;              
            }          
        }
        private int getCategoryID(string categoryName)
        {
            string sql = @"SELECT * 
                           FROM  ProductCategories
                           WHERE CategoryName = N'" + categoryName + "'";

            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            return (int)dt.Rows[0]["CategoryId"];
        }

        private int ConvertTextToInteger(string text)
        {
            // Loại bỏ các ký tự không phải số (ví dụ: dấu phân cách hàng nghìn, ký tự tiền tệ)
            var numberString = Regex.Replace(text, @"[^\d]", "");

            // Chuyển đổi chuỗi thành số nguyên
            if (int.TryParse(numberString, out int number))
            {
                return number;
            }
            else
            {
                // Xử lý lỗi hoặc trả về giá trị mặc định
                return 0; // Hoặc bạn có thể ném ra một ngoại lệ
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            var messResult = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này","Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messResult == MessageBoxResult.Yes) 
            {
                string sql = @"DELETE FROM PRODUCTS 
                                      WHERE ProductId = " + txtBookID.Text;
                DBConnect.DB.Query(sql);
                NavigateBackToProductsWindow?.Invoke(this, EventArgs.Empty); //Gọi chức năng của Back_Button 
            }
            else
            {
                return;
            }
            
        }
    }
}
