using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BookStoreManagement.UI.Config
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (isCheckEmptyData() == false)
                return;
            if (isCheckConfig() == true)
            {
                //Lưu thông tin vào local
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["database"].Value = txtDatabase.Text;
                config.AppSettings.Settings["server"].Value = txtServername.Text;

                config.Save(ConfigurationSaveMode.Minimal);

                ConfigurationManager.RefreshSection("appSettings");

                //Lấy thông tin 
                GlobalData.servername = txtServername.Text.Trim();
                GlobalData.database = txtDatabase.Text.Trim();
                var LoginScreen = new LoginWindow();
                LoginScreen.Show();
                this.Close();
            }
            else
                return;
        }

        private bool isCheckConfig()
        {
            string server = txtServername.Text;
            string database = txtDatabase.Text;

            string sConnect = @$"Data Source={server}; 
                           Initial Catalog={database}; 
                           Integrated Security=True";
            SqlConnection cn = new SqlConnection(sConnect);
            try
            {
                cn.Open();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kết nối đến Database thất bại !! Nguyên nhân: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        private bool isCheckEmptyData()
        {
            bool dk1 = txtServername.Text.Trim().Length == 0;
            bool dk2 = txtDatabase.Text.Trim().Length == 0;


            if (dk1 || dk2 )
            {
                MessageBox.Show("Hãy nhập đầy đủ vào các ô", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return true;
        }

        private void Config_Loaded(object sender, RoutedEventArgs e)
        {
            
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var serverConfig = config.AppSettings.Settings["server"].Value;
            var databaseConfig =  config.AppSettings.Settings["database"].Value;

            if (serverConfig != "")
            {
                txtServername.Text = serverConfig;
            }

            if (databaseConfig != "")
            {
                txtDatabase.Text = databaseConfig;
            }

        }
    }
}
