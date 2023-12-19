using BookStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStoreManagement.Customers
{
    /// <summary>
    /// Interaction logic for CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : UserControl
    {
        public CustomersWindow()
        {
            InitializeComponent();
        }

        private void Customer_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPage();
            UpdatePageInfo();
        }

        private void LoadDataBase(BindingList<Entities.Customers> _lib, string sql)
        {
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var customer = new Entities.Customers()
                {
                    CustomerID = (int)dt.Rows[i]["CustomerID"],
                    Name = (string)dt.Rows[i]["Name"],
                    Sex = (string)dt.Rows[i]["Sex"],
                    Address = (string)dt.Rows[i]["Address"],
                    Email = (string)dt.Rows[i]["Email"],
                    Phone = (string)dt.Rows[i]["Phone"],

                };
                _lib.Add(customer);
            }
        }

        //--------------Phân trang-----------------//
        private int currentPage = 1;
        private int itemsPerPage = 5; // Thay đổi số lượng mục trên mỗi trang theo ý bạn
        private int totalItems = 0;

        //Cập nhật lại số trang hiện tại
        private void UpdatePageInfo()
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            PageInfo.Text = $"{currentPage}/{totalPages}";
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            //tính tổng số trang bằng cách chia tổng số mục cho số mục trên mỗi trang
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage();
                UpdatePageInfo();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage();
                UpdatePageInfo();
            }
        }
        private void LoadPage()
        {
            BindingList<Entities.Customers> _lib = new BindingList<Entities.Customers>();

            // Tạo truy vấn SQL với phân trang (không điều kiện)
            string sql;
            string countSql;
           
            sql = $@"SELECT * FROM CUSTOMERS
                 ORDER BY CustomerID 
                 OFFSET {(currentPage - 1) * itemsPerPage} ROWS 
                 FETCH NEXT {itemsPerPage} ROWS ONLY";

            countSql = $@"SELECT COUNT(*) FROM CUSTOMERS";
            
            LoadDataBase(_lib, sql);

            totalItems = DBConnect.DB.QueryResult(countSql);
            CustomerList.ItemsSource = _lib;
        }

        //Xử lí sự kiện khi click vào 1 khách hàng

        public event EventHandler<CustomerEventArgs>? NavigateToDetailCustomer;
        private void detailCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            //Lấy đối tượng Order từ DataContext của Button
            var customer = ((FrameworkElement)sender).DataContext as Entities.Customers;

            if (customer != null)
            {
                NavigateToDetailCustomer?.Invoke(this, new CustomerEventArgs(customer));
            }
            else
                return;
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var screenAddCustomer = new Orders.NewCustomerWindow();
            if (screenAddCustomer.ShowDialog() == true)
            {
                //
            }
            else
            {
                return;
            }
        }

        
    }
}
