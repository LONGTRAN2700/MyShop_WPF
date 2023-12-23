using BookStoreManagement.DBConnect;
using DocumentFormat.OpenXml.Spreadsheet;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
//using LiveCharts;
//using LiveCharts.Wpf;

namespace BookStoreManagement.Statistic
{
    /// <summary>
    /// Interaction logic for StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow : UserControl
    {
      
        public StatisticWindow()
        {
            InitializeComponent();
            
        }

        public SeriesCollection? ColumnData { get; set; }
        public ColumnSeries? columnSeries { get; set; }

        public string[]? Labels {get; set; }
        public Func<int, string>? Values { get; set; }

        private int yearNow = DateTime.Now.Year;
        public void ColumnChart()
        {
            columnSeries = new ColumnSeries
            {
                Title = "DOANH THU HÀNG NĂM ",
                Values = new ChartValues<int>(),
            };

            for (int i = 2020; i <= yearNow; i++)
            {
                columnSeries.Values.Add(GetRevenueInYear(i));
                AddLabels(i.ToString());
            }

            columnChartName.DataContext = columnSeries;

            Values = value => value.ToString("N");

            DataContext = this;
        }

        private void AddLabels (string labels)
        {
            // Khởi tạo Labels nếu nó chưa được khởi tạo
            if (Labels == null)
            {
                Labels = new string[0];
            }

            // Thêm một giá trị mới
            string newValue = labels;

            // Tạo một mảng mới với kích thước lớn hơn
            string[] newLabels = new string[Labels.Length + 1];
            Labels.CopyTo(newLabels, 0);
            newLabels[newLabels.Length - 1] = newValue;

            // Gán lại Labels
            Labels = newLabels;
        }
        private int GetRevenueInYear(int year)
        {
            string sql = $@"SELECT COALESCE(SUM(TotalAmount), 0)
                            FROM Orders
                            WHERE Status = N'Đã giao thành công' AND DATEPART(YEAR, OrderDate) = {year}";
            return DBConnect.DB.QueryResult(sql);
        }
        private void Statistic_Loaded(object sender, RoutedEventArgs e)
        {
            ColumnChart();
        }

    }
}
