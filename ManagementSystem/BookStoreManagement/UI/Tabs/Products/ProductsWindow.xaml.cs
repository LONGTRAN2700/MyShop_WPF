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
using System.Data;
using System.ComponentModel;
using BookStoreManagement.Entities;
using ControlzEx.Theming;
using static System.Net.Mime.MediaTypeNames;
using ClosedXML.Excel;
using Microsoft.Win32;
using System.Globalization;

namespace BookStoreManagement.Products
{
    /// <summary>
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : UserControl
    {
        public ProductsWindow()
        {
            InitializeComponent();
        }

        private int currentPage = 1;
        private int itemsPerPage = GlobalData.itemsPerPageProducts; // Thay đổi số lượng mục trên mỗi trang 
        private int totalItems = 0;

        private string typeSort = "";

        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
            SortProduct.Text = "Mặc định";
            PriceSearch.Text = "Mặc định";
            typeSort = "ProductId"; //Sắp xếp theo mặc định (dùng ProductId)
            LoadPage(typeSort);

            UpdatePageInfo(); // Gọi phương thức cập nhật thông tin trang
        }

        private void LoadDataBase(BindingList<Product> _lib, string sql)
        {
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var book = new Product()
                {
                    BookName = (string)dt.Rows[i]["ProductName"],
                    ImageURL = (string)dt.Rows[i]["ImageURL"],
                    Price = (int)dt.Rows[i]["Price"],
                    BookId = (int)dt.Rows[i]["ProductId"],
                    Description = (string)dt.Rows[i]["Description"],
                    Author = (string)dt.Rows[i]["Author"],
                    CategoryName = (string)dt.Rows[i]["CategoryName"],
                    CategoryId = (int)dt.Rows[i]["CategoryId"],
                    Quantity = (int)dt.Rows[i]["Quantity"],
                };
                _lib.Add(book);
            }
        }

        private void LoadPage(string typeSort)
        {
            BindingList<Product> _lib = new BindingList<Product>();

            string orderByColumn = typeSort;

            int startPrice = 0;
            int endPrice = 0;

            isPriceSearch(ref startPrice,ref endPrice);

            // Tạo truy vấn SQL với phân trang (không điều kiện)
            string sql;
            string countSql;
            if (PriceSearch.Text == "Mặc định")
            {
                sql = $@"SELECT * FROM Products P,  ProductCategories C
                     Where  P.CategoryId = C.CategoryId
                     ORDER BY {orderByColumn} 
                     OFFSET {(currentPage - 1) * itemsPerPage} ROWS 
                     FETCH NEXT {itemsPerPage} ROWS ONLY";
                countSql = $@"SELECT COUNT(*) FROM Products";
            }
           else
            {
                sql = $@"SELECT * FROM Products P,  ProductCategories C
                     Where  P.CategoryId = C.CategoryId AND P.Price > {startPrice} AND P.Price <= {endPrice}
                     ORDER BY {orderByColumn}
                     OFFSET {(currentPage - 1) * itemsPerPage} ROWS 
                     FETCH NEXT {itemsPerPage} ROWS ONLY";

                countSql = $@"SELECT COUNT(*) FROM Products P,  ProductCategories C
                                    Where P.CategoryId = C.CategoryId AND 
                                    P.Price > {startPrice} AND P.Price <= {endPrice}";
            }

            LoadDataBase(_lib, sql);           
            
            totalItems = DBConnect.DB.QueryResult(countSql);
            BookList.ItemsSource = _lib;
        }

        private void isPriceSearch(ref int start,ref int end)
        {
            if (PriceSearch.Text == "0-50k")
            {
                start = 0;
                end = 50000;
                return;
            }
            if (PriceSearch.Text == "50k-100k")
            {
                start = 50000;
                end = 100000;
                return;
            }
            if (PriceSearch.Text == "100k-200k")
            {
                start = 100000;
                end = 200000;
                return;
            }
            if (PriceSearch.Text == "200k-300k")
            {
                start = 200000;
                end = 300000;
                return;
            }
            if (PriceSearch.Text == ">300k")
            {
                start = 300000;
                end = 999999999;
                return;
            }
        }

        private void UpdatePageInfo()
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            PageInfo.Text = $"{currentPage}/{totalPages}";
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage(typeSort);
                UpdatePageInfo();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            // Ở đây bạn cần tính tổng số trang bằng cách chia tổng số mục cho số mục trên mỗi trang
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage(typeSort);
                UpdatePageInfo();
            }
        }


        //Tính năng tìm kiếm sách 
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var character in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            BindingList<Product> _lib = new BindingList<Product>();
            string sql = $@"SELECT * 
                            FROM Products P,  ProductCategories C
                            Where  P.CategoryId = C.CategoryId";

            LoadDataBase(_lib, sql);
            
            //string searchText = SearchTextBox.Text.ToLower();

            string searchText = RemoveDiacritics(SearchTextBox.Text.ToLower());

            if (string.IsNullOrWhiteSpace(searchText))
            {
                SuggestionsPopup.IsOpen = false;
                return;
            }

            //var filteredItems = _lib.Where(book =>
            //    book.BookName.ToLower().Contains(searchText)).ToList();

            var filteredItems = _lib.Where(book =>
                  RemoveDiacritics(book.BookName.ToLower()).Contains(searchText)).ToList();


            if (filteredItems.Any())
            {
                SuggestionsListBox.ItemsSource = filteredItems;
                SuggestionsPopup.IsOpen = true;
            }
            else
            {
                SuggestionsPopup.IsOpen = false;
            }
        }

        public event EventHandler<ProductEventArgs>? NavigateToDetailProduct;

        private void detaiProductButton_Click(object sender, RoutedEventArgs e)
        {
            SuggestionsPopup.IsOpen = false;

            //Lấy đối tượng Product từ DataContext của Button
            var product = ((FrameworkElement)sender).DataContext as Product;

            if (product != null)
            {
                NavigateToDetailProduct?.Invoke(this, new ProductEventArgs(product));
            }
        }


        //Tính năng sắp xếp theo điều kiện
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;


            if ((string)SortProduct.Text == "Mặc định")
            {
                typeSort = "ProductId";
                LoadPage(typeSort);
                UpdatePageInfo();
            }

            if ((string)SortProduct.Text == "Giá tăng dần")
            {
                typeSort = "Price";
                LoadPage(typeSort);
                UpdatePageInfo();
            }

            if ((string)SortProduct.Text == "Giá giảm dần")
            {
                typeSort = "Price DESC";
                LoadPage(typeSort);
                UpdatePageInfo();
            }
        }

        //Sự kiện chuyển sang màn hình Thêm sản phẩm

        public event EventHandler? NavigateToAddProduct;
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigateToAddProduct?.Invoke(this, EventArgs.Empty);
        }


        //Nút bấm Export dữ liệu ra file excel
        private void ExportProduct_Click(object sender, RoutedEventArgs e)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Products");

            // Tiêu đề cho các cột
            worksheet.Cell("A1").Value = "Book ID";
            worksheet.Cell("B1").Value = "Book Name";          
            worksheet.Cell("C1").Value = "Price";        
            worksheet.Cell("D1").Value = "Author";
            worksheet.Cell("E1").Value = "Quantity";
            worksheet.Cell("F1").Value = "Category ID";
            worksheet.Cell("G1").Value = "Category Name";
            worksheet.Cell("H1").Value = "Description";
            worksheet.Cell("I1").Value = "Image URL";

            int currentRow = 2; // Bắt đầu từ hàng thứ 2 trong Excel (sau tiêu đề)

            List<Product> _lib = new List<Product>();
            string sql = @"SELECT* FROM Products P, ProductCategories C
                           Where  P.CategoryId = C.CategoryId";
            DataTable dt = DBConnect.DB.LoadCSDL(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var book = new Product()
                {
                    BookName = (string)dt.Rows[i]["ProductName"],
                    ImageURL = (string)dt.Rows[i]["ImageURL"],
                    Price = (int)dt.Rows[i]["Price"],
                    BookId = (int)dt.Rows[i]["ProductId"],
                    Description = (string)dt.Rows[i]["Description"],
                    Author = (string)dt.Rows[i]["Author"],
                    CategoryId = (int)dt.Rows[i]["CategoryId"],
                    CategoryName = (string)dt.Rows[i]["CategoryName"],
                    Quantity = (int)dt.Rows[i]["Quantity"],
                };
                _lib.Add(book);
            }
            foreach (var product in _lib)
            {
                worksheet.Cell(currentRow, 1).Value = product.BookId;
                worksheet.Cell(currentRow, 2).Value = product.BookName;
                worksheet.Cell(currentRow, 3).Value = product.Price;
                worksheet.Cell(currentRow, 4).Value = product.Author;
                worksheet.Cell(currentRow, 5).Value = product.Quantity;
                worksheet.Cell(currentRow, 6).Value = product.CategoryId;
                worksheet.Cell(currentRow, 7).Value = product.CategoryName;
                worksheet.Cell(currentRow, 8).Value = product.Description;
                worksheet.Cell(currentRow, 9).Value = product.ImageURL;
                currentRow++;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx",
                FileName = "ProductsData.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                workbook.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Data exported successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        //Sự kiện Import dữ liệu từ file Excel vào
        private void ImportProduct_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                BindingList<Product> _lib = new BindingList<Product>();

                var workbook = new XLWorkbook(openFileDialog.FileName);
                var worksheet = workbook.Worksheet(1); // Giả sử dữ liệu nằm ở sheet đầu tiên

                // Code để đọc dữ liệu từ worksheet và thêm vào _lib
                int row = 2; // Bắt đầu từ hàng thứ 2 (bỏ qua tiêu đề)

                while (true)
                {
                    var cell = worksheet.Cell(row, 1);
                    if (cell.IsEmpty()) break; // Dừng nếu hàng trống

                    var product = new Product
                    {
                        BookId = cell.GetValue<int>(),
                        BookName = worksheet.Cell(row, 2).GetValue<string>(),
                        Price = worksheet.Cell(row, 3).GetValue<int>(),
                        Author = worksheet.Cell(row, 4).GetValue<string>(),
                        Quantity = worksheet.Cell(row, 5).GetValue<int>(),
                        CategoryId = worksheet.Cell(row, 6).GetValue<int>(),
                        CategoryName = worksheet.Cell(row, 7).GetValue<string>(),
                        Description = worksheet.Cell(row, 8).GetValue<string>(),
                        ImageURL = worksheet.Cell(row, 9).GetValue<string>()
                    };

                    _lib.Add(product);
                    row++;
                }
                BookList.ItemsSource = _lib;
                MessageBox.Show("Data imported successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
