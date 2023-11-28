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
using System.Windows.Shapes;
using System.Globalization;

namespace BookStoreManagement
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new registerWindow();
            screen.Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(txtUsername.Text.Trim() != "")
            {
                txtErrorUsername.Width = 0;
                if (txtPassword.Password.Trim() != "")
                {
                    LoginSuccess();
                }
                else
                {
                    txtErrorPassword.Width = 350;
                }
            }
            else
            {
                txtErrorUsername.Width = 350;
            }
        }

        private void LoginSuccess()
        {
            string sql = @"SELECT * FROM USERS";
            DataTable dt = DBConnect.DB.LoadCSDL(sql);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (txtUsername.Text.Trim() == (string)dt.Rows[i]["Username"])
                {
                    if (txtPassword.Password.Trim() == (string)dt.Rows[i]["PasswordHash"])
                    {
                        var mainscreen = new MainWindow();
                        mainscreen.Show();
                        this.Close();
                        return;
                    }               
                }
                
            }
            MessageBox.Show("Tài khoản hoặc mật khẩu bạn nhập không chính xác\nVui lòng nhập lại",
                            "Lỗi",
                             MessageBoxButton.OK,
                             MessageBoxImage.Error);
            return;
        }
    
    }
}
