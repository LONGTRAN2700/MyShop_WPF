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

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
