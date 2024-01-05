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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Configuration;
using System.Security.Cryptography;
using BookStoreManagement.UI.Config;

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



        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new SignUpWindow();
            if (screen.ShowDialog() == true)
            {
                //
            }
            else
                return;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(txtUsername.Text.Trim() != "")
            {
                txtErrorUsername.Visibility = Visibility.Hidden;
                if (txtPassword.Password.Trim() != "")
                {
                    LoginSuccess();
                }
                else
                {
                    txtErrorPassword.Visibility = Visibility.Visible;
                }
            }
            else
            {
                txtErrorUsername.Visibility = Visibility.Visible;
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
                        if (rememberMe.IsChecked == true)
                        {
                            SavePass(txtUsername.Text, txtPassword.Password);
                        }
                        var mainscreen = new MainWindow(txtUsername.Text.Trim());
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

        //Mã hóa mật khẩu
        private void SavePass(string username, string password)
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var cypherText = ProtectedData.Protect(passwordInBytes, entropy,
                DataProtectionScope.CurrentUser);
            var passwordIn64 = Convert.ToBase64String(cypherText);
            var entropyIn64 = Convert.ToBase64String(entropy);

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["username"].Value = username;
            config.AppSettings.Settings["password"].Value = passwordIn64;
            config.AppSettings.Settings["entropy"].Value = entropyIn64;
            config.Save(ConfigurationSaveMode.Minimal);

            ConfigurationManager.RefreshSection("appSettings");
        }

        //Giải mã password
        private void UnmaskPass()
        {
            var passwordIn64 = ConfigurationManager.AppSettings["password"];

            if (passwordIn64.Length != 0)
            {
                var entropyIn64 = ConfigurationManager.AppSettings["entropy"];

                var cyperTextInBytes = Convert.FromBase64String(passwordIn64);
                var entropyInBytes = Convert.FromBase64String(entropyIn64);

                var passwordInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes,
                    DataProtectionScope.CurrentUser);
                var password = Encoding.UTF8.GetString(passwordInBytes);
                txtPassword.Password = password;

                txtUsername.Text = ConfigurationManager.AppSettings["username"];
            }
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            //Nếu mật khẩu được lưu sẽ tự động hiển thị tài khoản mật khẩu ra màn hình cho việc đăng nhập lần sau
            UnmaskPass();
        }

        private void FixButton_Click(object sender, RoutedEventArgs e)
        {
            var configScreen = new ConfigWindow();
            configScreen.Show();
            this.Close();
        }
    }
    
}
