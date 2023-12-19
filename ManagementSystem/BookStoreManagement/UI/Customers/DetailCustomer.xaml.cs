using System;
using System.Collections.Generic;
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

namespace BookStoreManagement.Customers
{
    /// <summary>
    /// Interaction logic for DetailCustomer.xaml
    /// </summary>
    public partial class DetailCustomer : UserControl
    {
        public DetailCustomer()
        {
            InitializeComponent();
        }

        private Entities.Customers? customerItem;
        public void SetCustomer(Entities.Customers customer)
        {
            // Thiết lập đối tượng Order cho DetailCustomer
            this.DataContext = customer; // Điều này cho phép bạn sử dụng ràng buộc dữ liệu trong DetailCustomer

            customerItem = customer;
        }

        //Sự kiện cho nút backButton quay về màn hình trước đó
        public event EventHandler? NavigateBackToCustomersWindow;
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackToCustomersWindow?.Invoke(this, EventArgs.Empty);
        }

       

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            //Kiểm tra xem có ô nào chưa điền
            if (isEmptyData() == true)
            {
                return;
            }

            //Hộp thoại
            var result = MessageBox.Show("Bạn có chắc chắn muốn lưu lại", "Thông báo", 
                                          MessageBoxButton.YesNo, MessageBoxImage.Information);
            
            if (result == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                string sql_updateCustomer = @$"UPDATE Customers
                                              SET Name = N'{txtCustomerName.Text}',
                                                  Sex = N'{txtSex.Text}',
                                                  Address = N'{txtAddress.Text}',
                                                  Phone = N'{txtPhone.Text}',
                                                  Email = N'{txtEmail.Text}'
                                              WHERE CustomerID = {txtCustomerID.Text}";
                DBConnect.DB.Query(sql_updateCustomer);
                MessageBox.Show("Cập nhật thông tin thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        //Hàm kiểm tra xem có ô nào chưa điền
        private bool isEmptyData()
        {
            bool Result = false;

            bool dk1 = txtCustomerName.Text.Trim().Length == 0;
            bool dk2 = txtSex.Text.Trim().Length == 0;
            bool dk3 = txtAddress.Text.Trim().Length == 0;
            bool dk4 = txtPhone.Text.Trim().Length == 0;
            bool dk5 = txtEmail.Text.Trim().Length == 0;

            if (dk1)
            {
                txtErrorCustomerName.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorCustomerName.Visibility = Visibility.Hidden;

            if (dk2)
            {
                txtErrorSex.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorSex.Visibility = Visibility.Hidden;

            if (dk3)
            {
                txtErrorAddress.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorAddress.Visibility = Visibility.Hidden;

            if (dk4)
            {
                txtErrorPhone.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorPhone.Visibility = Visibility.Hidden;

            if (dk5)
            {
                txtErrorEmail.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorEmail.Visibility = Visibility.Hidden;

            return Result;
        }
    }
}

