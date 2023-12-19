using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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

namespace BookStoreManagement
{
    /// <summary>
    /// Interaction logic for InforUserWindow.xaml
    /// </summary>
    public partial class InforUserWindow : Window
    {
        private string username;
        public InforUserWindow(string Username)
        {
            InitializeComponent();
            username = Username;
        }

        private void InforUser_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInforUser();
        }

        private void LoadInforUser()
        {
            Entities.User user = new Entities.User();
            string sql = @$"SELECT * FROM USERS WHERE Username = N'{username}'";
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            user.UserId = (int)dt.Rows[0]["UserId"];
            user.Fullname = (string)dt.Rows[0]["Fullname"];
            user.Sex = (string)dt.Rows[0]["Sex"];
            user.Email = (string)dt.Rows[0]["Email"];
            user.Phone = (string)dt.Rows[0]["Phone"];

            this.DataContext = user;
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
                string sql_updateUser = @$"UPDATE Users
                                              SET Fullname = N'{txtName.Text}',
                                                  Sex = N'{txtSex.Text}',
                                                  Phone = N'{txtPhone.Text}',
                                                  Email = N'{txtEmail.Text}'
                                              WHERE Username = N'{username}'";
                DBConnect.DB.Query(sql_updateUser);
                MessageBox.Show("Cập nhật thông tin thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //Hàm kiểm tra xem có ô nào chưa điền
        private bool isEmptyData()
        {
            bool Result = false;

            bool dk1 = txtName.Text.Trim().Length == 0;
            bool dk2 = txtSex.Text.Trim().Length == 0;
            bool dk3 = txtPhone.Text.Trim().Length == 0;
            bool dk4 = txtEmail.Text.Trim().Length == 0;

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

            return Result;
        }

       
    }
}
