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
            string sql;
            sql = @"INSERT INTO Customers (Name, Sex, Address, Email, Phone) VALUES
                    (N'" + txtName.Text + "', N'" + txtSex.Text + "', '" + txtAddress.Text + "', '" + txtEmail.Text + "', '" + txtPhone.Text + "')";

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
    }
}
