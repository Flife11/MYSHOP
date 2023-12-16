using Enity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Windows.Markup;

namespace GUI_DashBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        BindingList<Book> books;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Tạo kết nối đến database
            string strCon = "Data Source=.;Initial Catalog=MyShop;Integrated Security=True;TrustServerCertificate = True;";
            DB.Instance.ConnectionString = strCon;
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;

            //Chọn ngày bán hàng gần nhất làm ngày hiện tại
            DateTime curDate = new DateTime(2023,12,10);
            DateTime beginMonth_Date = new DateTime(curDate.Year,curDate.Month,1);  //Ngày đầu tháng

            int diff = (7 + (curDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime previousMonday = curDate.AddDays(-1*diff).Date;                //Ngày đầu tuần

            //Thêm các ngày vào tham số
            command.Parameters.AddWithValue("@curDate", curDate);
            command.Parameters.AddWithValue("@beginMonth", beginMonth_Date);
            command.Parameters.AddWithValue("@previousMonday", previousMonday);


            //Số sản phẩm đang bán
            command.CommandText = "select count(ID) from book";
            var rowsCount = command.ExecuteScalar();
            curProduct_tb.Text = rowsCount.ToString();

            //Tổng đơn hàng trong tuần
            command.CommandText = "select count(*) from[Order] where Date between @previousMonday and @curDate";
            var weekCount = command.ExecuteScalar();
            sumWeek_tb.Text = weekCount.ToString();

            //Tổng đơn hàng trong tháng
            command.CommandText = "select count(*) from[Order] where Date between @beginMonth and @curDate";
            var monthCount = command.ExecuteScalar();
            sumMonth_tb.Text = monthCount.ToString();

            //Sách sắp hết hàng
            command.CommandText = " select * from book where Availability < 5 and Availability >0 order by Availability";
            var reader = command.ExecuteReader();
            books = new BindingList<Book>();

            var _count = 0;
            while (reader.Read() && _count<5)
            {
                _count++;
                string _image = (string)reader["Image"];
                string _title = (string)reader["Title"];
                string _category = (string)reader["Category"];
                int _availability = (int)reader["Availability"];
                var book = new Book() {Image = _image, Title = _title, Category = _category, Availability = _availability };
                books.Add(book);
            }
            reader.Close();
            SoldingOutPr_Lv.ItemsSource = books;
        }
        private void drawChart_btn(object sender, RoutedEventArgs e)
        {
            DateTime? _beginDate = beginDate.SelectedDate;
            DateTime? _endDate = endDate.SelectedDate;
            var dateDiff = _endDate-_beginDate;
            
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;
            command.Parameters.Add("@beginDate", System.Data.SqlDbType.DateTime);
            command.Parameters.Add("@endDate", System.Data.SqlDbType.DateTime);
            command.Parameters["@beginDate"].Value= _beginDate.Value.ToShortDateString();
            command.Parameters["@endDate"].Value= _endDate.Value.ToShortDateString();

            // Doanh thu
            if (dateDiff.Value.Days >= 40 && dateDiff.Value.Days < 120 && _beginDate.Value.Year == _endDate.Value.Year) {
                command.CommandText = "select DATEPART(WEEK,[Date]),sum([Price]) as total from [Order], [OrderDetail], [book]\r\nwhere [Order].[ID]= [OrderDetail].[Order] and [book].[ID]=[OrderDetail].[Book] and [Order].[Date] > @beginDate and [Date] < @endDate \r\ngroup by DATEPART(WEEK,[Date])";
                ViewModel.XAxes[0].Name = "Tuần";
            }
            else if (dateDiff.Value.Days < 40)
            {
                command.CommandText = "select FORMAT([Date],'dd-MM-yyyy'),sum([Price]) as total from [Order], [OrderDetail], [book]\r\nwhere [Order].[ID]= [OrderDetail].[Order] and [book].[ID]=[OrderDetail].[Book] and [Order].[Date] > @beginDate and [Date] < @endDate \r\ngroup by FORMAT([Date],'dd-MM-yyyy')";
                ViewModel.XAxes[0].Name = "Ngày";
            }
            else if (dateDiff.Value.Days < 1000)
            {
                command.CommandText = "SELECT\r\n    FORMAT([Order].[Date], 'yyyy-MM') AS YearMonth,\r\n    SUM([book].[Price]) AS Total\r\n" +
                    "FROM\r\n    [Order]\r\n    JOIN [OrderDetail] ON [Order].[ID] = [OrderDetail].[Order]\r\n    JOIN [book] ON [book].[ID] = [OrderDetail].[Book]\r\n" +
                    "WHERE\r\n    [Order].[Date] > @beginDate AND [Order].[Date] < @endDate\r\nGROUP BY\r\n    FORMAT([Order].[Date], 'yyyy-MM')\r\nORDER BY\r\n    FORMAT([Order].[Date], 'yyyy-MM');";
                    ViewModel.XAxes[0].Name = "Tháng";
            }

            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                List<string> stringValues = new List<string>();
                List<float> floatValues = new List<float>();
                while (reader.Read())
                {
                    string key = reader.GetValue(0).ToString();
                    float value = (float)reader.GetDouble(1);
                    stringValues.Add(key);
                    floatValues.Add(value);
                }
                ViewModel.Series[0].Values = floatValues.ToArray();
                ViewModel.XAxes[0].Labels = stringValues.ToArray();
                ViewModel.YAxes[0].Name = "Doanh thu";
            }
            reader.Close();

            //Loại sách & số lượng
            command.CommandText = "select Category, count(*) as quantity\r\nfrom book join OrderDetail on book.ID = OrderDetail.Book join [Order] on OrderDetail.[Order] = [Order].ID\r\nwhere [Date] > @beginDate and [Date] < @endDate\r\ngroup by Category";
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                List<string> stringValues = new List<string>();
                List<int> intValues = new List<int>();
                while (reader.Read())
                {
                    string key = reader.GetValue(0).ToString();
                    int value = reader.GetInt32(1);
                    stringValues.Add(key);
                    intValues.Add(value);
                }
                ViewModel.Series2[0].Values = intValues.ToArray();
                ViewModel.XAxes2[0].Labels = stringValues.ToArray();
            }
            reader.Close();
        }
    }
}
