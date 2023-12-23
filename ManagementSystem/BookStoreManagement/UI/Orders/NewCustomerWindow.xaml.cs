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
using System.Windows.Shapes;

namespace BookStoreManagement.Orders
{
    /// <summary>
    /// Interaction logic for NewCustomerWindow.xaml
    /// </summary>
    public partial class NewCustomerWindow : Window
    {
        public NewCustomerWindow()
        {
            InitializeComponent();
        }

        private void registButton_Click(object sender, RoutedEventArgs e)
        {
            if (isEmptyData() == true)
            {
                return;
            }

            string sql;
            sql = $@"INSERT INTO Customers (Name, Sex, Address, Email, Phone) VALUES
                    (N'{txtName.Text}', N'{txtSex.Text}', N'{ txtAddress.Text}', N'{txtEmail.Text}', N'{txtPhone.Text}')";

            var Result = MessageBox.Show("Bạn có chắc chắn muốn đăng kí", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (Result == MessageBoxResult.Yes)
            {
                DBConnect.DB.Query(sql);
                DialogResult = true;
                this.Close();
            }
            else
                return;           
        }

        //Kiem tra co o nao chua nhap vao
        private bool isEmptyData()
        {
            bool Result = false;

            bool dk1 = txtName.Text.Trim().Length == 0;
            bool dk2 = txtSex.Text.Trim().Length == 0;
            bool dk3 = txtPhone.Text.Trim().Length == 0;
            bool dk4 = txtEmail.Text.Trim().Length == 0;
            bool dk5 = txtAddress.Text.Trim().Length == 0;

            if (dk1)
            {
                txtErrorName.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorName.Visibility = Visibility.Hidden;
            if (dk2)
            {
                txtErrorSex.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorSex.Visibility = Visibility.Hidden;

            if (dk3)
            {
                txtErrorPhone.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorPhone.Visibility = Visibility.Hidden;

            if (dk4)
            {
                txtErrorEmail.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorEmail.Visibility = Visibility.Hidden;

            if (dk5)
            {
                txtErrorAddress.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorAddress.Visibility = Visibility.Hidden;
            return Result;
        }
    }
}
