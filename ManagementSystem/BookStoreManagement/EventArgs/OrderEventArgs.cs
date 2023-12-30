using BookStoreManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement
{
    public class OrderEventArgs : EventArgs
    {
        public Order Order { get; private set; }

        public OrderEventArgs(Order order)
        {
            Order = order;
        }
    }
}
