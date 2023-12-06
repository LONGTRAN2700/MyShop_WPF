using BookStoreManagement.Entities;
using DocumentFormat.OpenXml.Bibliography;
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

namespace BookStoreManagement.Orders
{
    /// <summary>
    /// Interaction logic for DetailOrderWindow.xaml
    /// </summary>
    public partial class DetailOrderWindow : UserControl
    {
        public DetailOrderWindow()
        {
            InitializeComponent();
        }

        private Order? orderItem;
        public void SetOrder(Order order)
        {
            // Thiết lập đối tượng Order cho DetailOrder
            this.DataContext = order; // Điều này cho phép bạn sử dụng ràng buộc dữ liệu trong DetailOrder

            orderItem = order;
        }

        //Sự kiện cho nút backButton quay về màn hình trước đó
        public event EventHandler? NavigateBackToOrdersWindow;
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackToOrdersWindow?.Invoke(this, EventArgs.Empty);
        }

        private Brush oldBackground;
        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateOrder.Content.ToString() == "Sửa")
            {
                UpdateOrder.Content = "Lưu";
                oldBackground = UpdateOrder.Background;
                UpdateOrder.Background = new SolidColorBrush(Colors.Aqua);

                //Cho phép nhập liệu vào
                txtStatus.IsEnabled = true;
            }
            else if (UpdateOrder.Content.ToString() == "Lưu") //Quay lại về format ban đầu và cập nhật dữ liệu vào database
            {
                var Result = MessageBox.Show("Bạn có chắc chắc muốn cập nhật", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (Result == MessageBoxResult.Yes)
                {
                    UpdateOrder.Content = "Sửa";
                    UpdateOrder.Background = oldBackground;
                    txtStatus.IsEnabled = false;

                    //Cập nhật lại dữ liệu trong database
                    if (orderItem != null)
                    {
                        string sql_updateStatus = @"UPDATE Orders 
                                            SET Status = N'" + txtStatus.Text + "' " +
                                               "WHERE OrderID = " + orderItem.OrderId;
                        DBConnect.DB.Query(sql_updateStatus);
                    }
                    else
                        return;
                }
                else
                    return;
            }
            else
                return;
        }

        private void DetailOrder_Loaded(object sender, RoutedEventArgs e)
        {
            BindingList<OrderDetail> _lib = new BindingList<OrderDetail>();
            if (orderItem != null)
            {
                string sql = @"SELECT * FROM Products P, OrderDetails OD
                           WHERE P.ProductId = OD.ProductId AND OrderID = " + orderItem.OrderId;
                DataTable dt = DBConnect.DB.LoadCSDL(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var orderdetail = new OrderDetail
                    {
                        ProductName = (string)dt.Rows[i]["ProductName"],
                        UnitPrice = (int)dt.Rows[i][2],
                        Quantity = (int)dt.Rows[i][11],
                        Price = (int)dt.Rows[i][12],
                    };
                    _lib.Add(orderdetail);
                }
                DetailOrderList.ItemsSource = _lib;
            }
            else
                return;
            
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            string sql_deleteDetailsList, sql_deleteOrder;
            var Result = MessageBox.Show("Bạn có chắc chắn muốn xóa đơn hàng này", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (Result == MessageBoxResult.Yes)
            {
                if (orderItem != null)
                {
                    sql_deleteDetailsList = @"DELETE FROM OrderDetails
                                              WHERE OrderID = " + orderItem.OrderId;
                    DBConnect.DB.Query(sql_deleteDetailsList);

                    sql_deleteOrder = @"DELETE FROM Orders
                                      WHERE OrderID = " + orderItem.OrderId;
                    DBConnect.DB.Query(sql_deleteOrder);

                    MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigateBackToOrdersWindow?.Invoke(this, EventArgs.Empty); //Quay về lại màn hình trước đó
                }
                else
                    return;
            }
            else
                return;
            
            

        }
    }
}
