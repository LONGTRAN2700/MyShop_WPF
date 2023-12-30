using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement
{
    public static class GlobalData
    {
        //Số lượng sản phẩm hiển thị ở mỗi trang
        public static int itemsPerPageProducts;

        //Số lượng đơn hàng hiển thị mỗi trang
        public static int itemsPerPageOrders;

        //Thông tin kết nối
        public static string servername;
        public static string database;
    }
}
