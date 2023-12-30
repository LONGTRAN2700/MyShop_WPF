using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement
{
    public class CustomerEventArgs : EventArgs
    {
        public Entities.Customers Customer { get; private set; }

        public CustomerEventArgs(Entities.Customers customer)
        {
            Customer = customer;
        }
    }
}
