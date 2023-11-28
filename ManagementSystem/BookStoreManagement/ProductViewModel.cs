using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private int price;
        private string author;
        private string booktype;
        private string description;
        // ... thêm các thuộc tính khác theo nhu cầu

        public int Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public string Author
        {
            get => author;
            set
            {
                if (author != value)
                {
                    author = value;
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        public string Booktype
        {
            get => booktype;
            set
            {
                if (booktype != value)
                {
                    booktype = value;
                    OnPropertyChanged(nameof(Booktype));
                }
            }
        }
        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        // ... implement các getter và setter cho các thuộc tính khác

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
