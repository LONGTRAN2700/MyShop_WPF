using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Shapes;

namespace BookStoreManagement.Products
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : UserControl
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (isEmptyData() == true)
            {
                return;
            }
            else
            {
                string sql = @"INSERT INTO Products(ProductName, Price, Quantity, CategoryId, Description, ImageURL, Author)
                         VALUES(N' " + txtName.Text + "', " + ConvertTextToInteger(txtPrice.Text) + ", " + ConvertTextToInteger(txtQuantity.Text) + ", " + getCategoryID(txtCategory.Text) + ", N'" + txtDescription.Text + "', N'Images/Products/book.jpg', N'" + txtAuthor.Text + "' )";

                var messResult = MessageBox.Show("Bạn có chắc muốn thêm sản phẩm này", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messResult == MessageBoxResult.Yes)
                {
                    DBConnect.DB.Query(sql);
                    NavigateBackToProductsWindow?.Invoke(this, EventArgs.Empty);
                }
                else
                    return;
            }
           
        }

        //Kiem tra co o nao chua nhap vao
        private bool isEmptyData()
        {
            bool Result = false;

            bool dk1 = txtName.Text.Trim().Length == 0;
            bool dk2 = txtAuthor.Text.Trim().Length == 0;
            bool dk3 = txtPrice.Text.Trim().Length == 0;
            bool dk4 = txtCategory.Text.Trim().Length == 0;
            bool dk5 = txtQuantity.Text.Trim().Length == 0;
            bool dk6 = txtDescription.Text.Trim().Length == 0;

            if (dk1)
            {
                txtErrorName.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorName.Visibility = Visibility.Hidden;

            if (dk2)
            {
                txtErrorAuthor.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorAuthor.Visibility = Visibility.Hidden;

            if (dk3 || isINT(txtPrice.Text) == false)
            {
                txtErrorPrice.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorPrice.Visibility = Visibility.Hidden;

            if (dk4)
            {
                txtErrorCategory.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorCategory.Visibility = Visibility.Hidden;

            if (dk5 || isINT(txtQuantity.Text) == false)
            {
                txtErrorQuantity.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorQuantity.Visibility = Visibility.Hidden;

            if (dk6)
            {
                txtErrorDescription.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorDescription.Visibility = Visibility.Hidden;

            return Result;
        }

    //Hàm lấy CategoryID từ CategoryName
    private int getCategoryID(string categoryName)
        {
            string sql = @"SELECT * 
                           FROM  ProductCategories
                           WHERE CategoryName = N'" + categoryName + "'";

            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            return (int)dt.Rows[0]["CategoryId"];
        }

        //Hàm chuyển dạng chuỗi thành số nguyên
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

        //Hàm kiểm tra xem có phải là số nguyên dương hay không
        private bool isINT(string text)
        {
            int soNguyen;
            bool laSoNguyen = int.TryParse(text, out soNguyen);
            if (soNguyen <= 0)
                return false;
            return laSoNguyen;
        }

        //Sự kiện của nút BackButton
        public event EventHandler? NavigateBackToProductsWindow;
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackToProductsWindow?.Invoke(this, EventArgs.Empty);
        }

        private void AddProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = @"SELECT * FROM ProductCategories";
            DataTable dt = DBConnect.DB.LoadCSDL(sql);
            List<string> _categoryList = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string categoryName;
                categoryName = (string)dt.Rows[i]["CategoryName"];
                _categoryList.Add(categoryName);
            }
            txtCategory.ItemsSource = _categoryList;
        }
    }
}
