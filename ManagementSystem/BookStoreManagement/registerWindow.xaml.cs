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

                string sql = @"INSERT INTO USERS VALUES (N'" + user.Username + "', '" + user.Password + "', N'" + user.Fullname + "', N'" + user.Sex + "', '" + user.Phone + "', '" + user.Email + "');";  
                DBConnect.DB.Query(sql);

                MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    txtErrorUsername.Width = 107;
                    return true;
                }
            }
            txtErrorUsername.Text = "*Chưa điền thông tin";
            txtErrorUsername.Width = 0;
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
                txtErrorUsername.Width = 107;
                Result = true;
            }
            else
                txtErrorUsername.Width = 0;

            if (dk2)
            {
                txtErrorPassword.Width = 107;
                Result = true;
            }
            else
                txtErrorPassword.Width = 0;

            if (dk3)
            {
                txtErrorFullname.Width = 107;
                Result = true;
            }
            else
                txtErrorFullname.Width = 0;

            if (dk4)
            {
                txtErrorSex.Width = 107;
                Result = true;
            }
            else
                txtErrorSex.Width = 0;

            if (dk5)
            {
                txtErrorPhone.Width = 107;
                Result = true;
            }
            else
                txtErrorPhone.Width = 0;

            if (dk6)
            {
                txtErrorEmail.Width = 107;
                Result = true;
            }
            else
                txtErrorEmail.Width = 0;

            return Result;
        }
    }
}
