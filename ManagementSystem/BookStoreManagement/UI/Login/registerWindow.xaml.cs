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
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreManagement
{
    /// <summary>
    /// Interaction logic for registerWindow.xaml
    /// </summary>
    public partial class registerWindow : Window
    {
        public registerWindow()
        {
            InitializeComponent();
        }

        private void registButton_Click(object sender, RoutedEventArgs e)
        {
            if (isCoincideUsername() == true)
            {
                return;
            }
            if (isEmptyData() == true)
            {
                return;
            }
            else
            {
                Entities.User user = new Entities.User();
                user.Username = txtUsername.Text;
                user.Password = txtPassword.Password;
                user.Fullname = txtFullname.Text;
                user.Sex = txtSex.Text;
                user.Phone = txtPhone.Text;
                user.Email = txtEmail.Text;

                string sql = $@"INSERT INTO USERS (Username, PasswordHash, Fullname, Sex, Phone, Email) 
                               VALUES (N'{user.Username}', '{user.Password}', N'{user.Fullname}', N'{user.Sex}', '{user.Phone}', '{user.Email}');";  
                DBConnect.DB.Query(sql);

                MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            

        }
        
        //Kiem tra username co bi trung trong CSDL
        private bool isCoincideUsername()
        {
            string sql = @"SELECT * FROM USERS";
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            for (int i = 0; i < dt.Rows.Count; i++ )
            {
                if (txtUsername.Text.Trim() == (string)dt.Rows[i]["Username"])
                {
                    txtErrorUsername.Text = "*Username đã tồn tại";
                    txtErrorUsername.Visibility = Visibility.Visible;
                    return true;
                }
            }
            txtErrorUsername.Text = "*Chưa điền thông tin";
            txtErrorUsername.Visibility = Visibility.Hidden;
            return false;
        }
     
        //Kiem tra co o nao chua nhap vao
        private bool isEmptyData()
        {
            bool Result = false;

            bool dk1 = txtUsername.Text.Trim().Length == 0;
            bool dk2 = txtPassword.Password.Trim().Length == 0;
            bool dk3 = txtFullname.Text.Trim().Length == 0;
            bool dk4 = txtSex.Text.Trim().Length == 0;
            bool dk5 = txtPhone.Text.Trim().Length == 0;
            bool dk6 = txtEmail.Text.Trim().Length == 0;

            if (dk1)
            {
                txtErrorUsername.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorUsername.Visibility = Visibility.Hidden;

            if (dk2)
            {
                txtErrorPassword.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorPassword.Visibility = Visibility.Hidden;

            if (dk3)
            {
                txtErrorFullname.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorFullname.Visibility = Visibility.Hidden;

            if (dk4)
            {
                txtErrorSex.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorSex.Visibility = Visibility.Hidden;

            if (dk5)
            {
                txtErrorPhone.Visibility = Visibility.Visible;
                Result = true;
            }
            else
                txtErrorPhone.Visibility = Visibility.Hidden;

            if (dk6)
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
