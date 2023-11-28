using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.Entities
{
    public class Product : INotifyPropertyChanged
    {
        private int bookId;
        private string bookName;
        private int price;
        private string author;
        private int quantity;
        private int categoryId;
        private string categoryName;
        private string description;
        private string imageUrl;

        public int BookId
        {
            get => bookId;
            set
            {
                if (bookId != value)
                {
                    bookId = value;
                    OnPropertyChanged(nameof(BookId));
                }
            }
        }

        public string BookName
        {
            get => bookName;
            set
            {
                if (bookName != value)
                {
                    bookName = value;
                    OnPropertyChanged(nameof(BookName));
                }
            }
        }

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

        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public int CategoryId
        {
            get => categoryId;
            set
            {
                if (categoryId != value)
                {
                    categoryId = value;
                    OnPropertyChanged(nameof(CategoryId));
                }
            }
        }

        public string CategoryName
        {
            get => categoryName;
            set
            {
                if (categoryName != value)
                {
                    categoryName = value;
                    OnPropertyChanged(nameof(CategoryName));
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

        public string ImageURL
        {
            get => imageUrl;
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    OnPropertyChanged(nameof(ImageURL));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
