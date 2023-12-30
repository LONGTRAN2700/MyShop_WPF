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

namespace BookStoreManagement
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private string user;
        public SettingWindow(string username)
        {
            InitializeComponent();
            user = username;
        }

        private void Setting_Loaded(object sender, RoutedEventArgs e)
        {
            countProducts.Text = GlobalData.itemsPerPageProducts.ToString();
            countOrders.Text = GlobalData.itemsPerPageOrders.ToString();
        }

        private void CloseSetting_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.itemsPerPageProducts = toINT(countProducts.Text);
            GlobalData.itemsPerPageOrders = toINT(countOrders.Text);

            string sql = $@"UPDATE USERS
                            SET CountProducts = {GlobalData.itemsPerPageProducts},
                                CountOrders = {GlobalData.itemsPerPageOrders}
                            WHERE Username = N'{user}'";
            DBConnect.DB.Query(sql);
            MessageBox.Show("Đã lưu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        //Hàm chuyển chuỗi về số nguyên
        private int toINT(string text)
        {
            // Chuyển đổi chuỗi thành số nguyên
            if (int.TryParse(text, out int number))
            {
                return number;
            }
            else
            {
                return 0;
            }
        }

        //Nút trừ của Products
        private void subProductButton_Click(object sender, RoutedEventArgs e)
        {
            int count = toINT(countProducts.Text);
            if (count == 1)
                return;
            else
            {
                count--;
                countProducts.Text = count.ToString();
            }
            
        }

        //Nút cộng của Products
        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            int count = toINT(countProducts.Text);
            count++;
            countProducts.Text = count.ToString();
        }


        //Nút trừ của Orders
        private void subOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int count = toINT(countOrders.Text);
            if (count == 1)
                return;
            else
            {
                count--;
                countOrders.Text = count.ToString();
            }
        }

        //Nút cộng của Orders
        private void addOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int count = toINT(countOrders.Text);
            count++;
            countOrders.Text = count.ToString();
        }

    }
}
