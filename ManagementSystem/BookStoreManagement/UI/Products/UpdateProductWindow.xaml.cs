using BookStoreManagement.Entities;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreManagement.Products
{
    /// <summary>
    /// Interaction logic for UpdateProductWindow.xaml
    /// </summary>
    public partial class UpdateProductWindow : Window
    {
        public UpdateProductWindow()
        {
            InitializeComponent();
        }
        public void SetProduct(Product product)
        {
            // Thiết lập đối tượng Product cho UpdateProduct
            this.DataContext = product; // Điều này cho phép bạn sử dụng ràng buộc dữ liệu trong UpdateProduct
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            var messResult = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn lưu lại sự thay đổi này", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messResult == MessageBoxResult.Yes)
            {
                DialogResult = true;
                this.Close();
            }
            else
                return;
            
        }

        private void UpdateWindow_Loaded(object sender, RoutedEventArgs e)
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
            txtCategoryName.ItemsSource = _categoryList;
        }
    }
}
