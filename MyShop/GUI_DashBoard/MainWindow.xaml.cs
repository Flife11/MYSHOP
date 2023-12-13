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
using LiveCharts;
using LiveCharts.Wpf;

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
            string strCon = "Data Source=.;Initial Catalog=MyShop;Integrated Security=True;TrustServerCertificate = True;";
            DB.Instance.ConnectionString = strCon;
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;
            command.CommandText = "select count(ID) from book";
            var rowsCount = command.ExecuteScalar();
            curProduct_tb.Text = rowsCount.ToString();

            command.CommandText = "select * from[Order] where Date between '2023-12-01' and '2023-12-31'";
            var monthCount = command.ExecuteScalar();
            sumMonth_tb.Text = monthCount.ToString();

            command.CommandText = " select * from book where Availability < 5 and Availability >0 order by Availability";
            var reader = command.ExecuteReader();
            books = new BindingList<Book>();

            while (reader.Read())
            {
                string _title = (string)reader["Title"];
                string _category = (string)reader["Category"];
                int _availability = (int)reader["Availability"];
                var book = new Book() { Title = _title, Category = _category, Availability = _availability };
                books.Add(book);
            }
            reader.Close();
            SoldingOutPr_Lv.ItemsSource = books;
        }

        private void drawChart_btn(object sender, RoutedEventArgs e)
        {
            DateTime? _beginDate = beginDate.SelectedDate;
            DateTime? _endDate = endDate.SelectedDate;
            int _type = type_cb.SelectedIndex;
            MessageBox.Show(_type.ToString());
        }
    }
}
