using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ThreeLayerContract;

namespace GUI_DashB
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        IBus _bus;
        public UserControl1(IBus bus)
        {
            _bus = bus;
            InitializeComponent();            
        }
        BindingList<Book> books;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Tạo kết nối đến database
            /*string strCon = "Data Source=.;Initial Catalog=MyShop;Integrated Security=True;TrustServerCertificate = True;";
            DB.Instance.ConnectionString = strCon;
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;   */

            //Thêm các ngày vào tham số
            // data trả về lần luot là rowsCount, weekCount, monthCount
            var data = _bus.LoadDashInfor();

            //curProduct_tb.Text = data[0];

            //sumWeek_tb.Text = data[1];

            //sumMonth_tb.Text = data[2];

            ////Sách sắp hết hàng
            //var books = _bus.SoldingOutPr_Lv();                     
            //SoldingOutPr_Lv.ItemsSource = books;
        }
        private void drawChart_btn(object sender, RoutedEventArgs e)
        {
            //DateTime? _beginDate = beginDate.SelectedDate;
            //DateTime? _endDate = endDate.SelectedDate;
            //var data = _bus.ChartInfor(_beginDate, _endDate);

            /*ViewModel.XAxes[0].Name = data.Item1;
            ViewModel.Series[0].Values = data.Item2.ToArray();
            ViewModel.XAxes[0].Labels = data.Item3.ToArray();
            ViewModel.YAxes[0].Name = "Revenue";*/

            // Doanh thu           

            //Loại sách & số lượng
            var res = _bus.BooksAndQuantity();
            /*ViewModel.Series2[0].Values = res.Item1.ToArray();
            ViewModel.XAxes2[0].Labels = res.Item2.ToArray();     */       
        }
    }
}
