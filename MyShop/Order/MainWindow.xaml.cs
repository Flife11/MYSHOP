using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Order
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
        public static BindingList<ElementOrder> _listOrder = new BindingList<ElementOrder>();
        private void addOrderButton(object sender, RoutedEventArgs e)
        {
            AddOrderWindow _addOrder = new AddOrderWindow();
            bool? _bool = _addOrder.ShowDialog();
            if (_bool != null)
            {

            }
            else
            {

            }

        }
        private int _offset = 0;
        private bool isSearching = false;
        string _dateFrom;
        string _dateTo;
        private void SearchOrder(object sender, RoutedEventArgs e)
        {
            if(!dateFrom.SelectedDate.HasValue)
            {
                MessageBox.Show("Lỗi: Vui lòng nhập ngày bắt đầu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return ;
            }
            if (!dateTo.SelectedDate.HasValue)
            {
                MessageBox.Show("Lỗi: Vui lòng nhập ngày kết thúc!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _offset = 0;
             _dateFrom= dateFrom.SelectedDate.Value.ToString("MM-dd-yyyy");
             _dateTo=dateTo.SelectedDate.Value.ToString("MM-dd-yyyy");
            getListOrderBySearch(_dateFrom, _dateTo);
            isSearching = true;
        }
        Pages _page = new Pages()
        {
            _currentPage = 1,
            _pageSize = 2
        };
        public static SqlConnection connection = null;
        private async void OrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=LAPTOP-K39M1QD9;Database=MyShop;Trusted_Connection=yes;TrustServerCertificate=True;";
            connection = new SqlConnection(connectionString);
            connection.Open();

            getListOrder(connection);
           
        }
        private async void getListOrder(SqlConnection connection)
        {
            try
            {
                var _orders = await Task.Run(() =>
                {
                    int over = _offset * 17;
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"SELECT od.ID, SUM(bk.Price * ordt.Quantity) AS TotalPrice, Sum(ordt.Quantity) as Quantity, od.[Date]\r\nFROM OrderDetail ordt\r\nJOIN [Order] od ON ordt.[Order] = od.ID\r\nJOIN Book bk ON ordt.Book = bk.ID\r\nGROUP BY od.ID,od.[Date]\r\nORDER BY od.[Date] DESC \r\nOFFSET {over} ROWS \r\nFETCH NEXT 17 ROWS ONLY;";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var listorder = new List<ElementOrder>();
                            while (reader.Read())
                            {
                                ElementOrder _order = new ElementOrder()
                                {
                                    Id = (int)reader["ID"],
                                    Date = reader["Date"].ToString(),
                                    Quantity = (int)reader["Quantity"],
                                    Price = (double)reader["TotalPrice"]
                                    // Gán các thuộc tính khác của bảng Shop tương ứng
                                };
                                listorder.Add(_order);
                            }
                            System.Threading.Thread.Sleep(500);
                            return listorder;
                        }
                    }
                });
                _listOrder.Clear();
                foreach (var order in _orders)
                {
                    _listOrder.Add(order);
                }

                var _counts = await Task.Run(() =>
                {
                    int over = _offset * 17;
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng 
                    string query = "SELECT Count(*) ID \r\nfrom [Order]";
                    using (var command = new SqlCommand(query, connection))
                    {
                        int rowCount = (int)command.ExecuteScalar();
                        System.Threading.Thread.Sleep(500);

                        return rowCount;
                    }
                });
                int count = _counts / 17 + (_counts % 17 == 0 ? 0 : 1);
                _page._pageSize = count;
                if (_page._currentPage == 1)
                {
                    previousPage.IsEnabled = false;
                }
                else
                {
                    previousPage.IsEnabled = true;
                }
                if (_page._currentPage == _page._pageSize)
                {
                    nextPage.IsEnabled = false;
                }
                else
                {
                    nextPage.IsEnabled = true;

                }
                DataContext = _page;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            finally
            {
                listOrder.ItemsSource = _listOrder;
            }
        }

        private async void getListOrderPage(SqlConnection connection)
        {
            try
            {
                var _orders = await Task.Run(() =>
                {
                    int over = _offset * 17;
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"SELECT od.ID, SUM(bk.Price * ordt.Quantity) AS TotalPrice, Sum(ordt.Quantity) as Quantity, od.[Date]\r\nFROM OrderDetail ordt\r\nJOIN [Order] od ON ordt.[Order] = od.ID\r\nJOIN Book bk ON ordt.Book = bk.ID\r\nGROUP BY od.ID,od.[Date]\r\nORDER BY od.[Date] DESC \r\nOFFSET {over} ROWS \r\nFETCH NEXT 17 ROWS ONLY;";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var listorder = new List<ElementOrder>();
                            while (reader.Read())
                            {
                                ElementOrder _order = new ElementOrder()
                                {
                                    Id = (int)reader["ID"],
                                    Date = reader["Date"].ToString(),
                                    Quantity = (int)reader["Quantity"],
                                    Price = (double)reader["TotalPrice"]
                                    // Gán các thuộc tính khác của bảng Shop tương ứng
                                };
                                listorder.Add(_order);
                            }
                            System.Threading.Thread.Sleep(500);
                            return listorder;
                        }
                    }
                });
                _listOrder.Clear();
                foreach (var order in _orders)
                {
                    _listOrder.Add(order);
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            finally
            {
                listOrder.ItemsSource = _listOrder;
            }
        }


        private void PreviousPageClick(object sender, RoutedEventArgs e)
        {
            if (_page._currentPage>1)
            {
                _page._currentPage--;
                _offset--;
                if(isSearching)
                {
                    getListOrderBySearchPage(_dateFrom,_dateTo);
                }
                else
                {
                    getListOrderPage(connection);

                }
                if (_page._currentPage ==1)
                {
                    previousPage.IsEnabled = false;
                }
                nextPage.IsEnabled = true;
            }

        }

        private void NextPageClick(object sender, RoutedEventArgs e)
        {

            if (_page._currentPage <_page._pageSize)
            {
                _page._currentPage++;
                _offset++;
                if (isSearching)
                {
                    getListOrderBySearchPage(_dateFrom, _dateTo);
                }
                else
                {
                    getListOrderPage(connection);

                }
                if (_page._currentPage == _page._pageSize)
                {
                    nextPage.IsEnabled = false;
                }
                previousPage.IsEnabled = true;

            }

        }
        private async void getListOrderBySearch(string dateFrom, string dateTo)
        {
           
            try
            {
                var _orders = await Task.Run(() =>
                {
                    int over = _offset * 17;
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"SELECT od.ID, SUM(bk.Price * ordt.Quantity) AS TotalPrice, Sum(ordt.Quantity) as Quantity, od.[Date]\r\nFROM OrderDetail ordt\r\nJOIN [Order] od ON ordt.[Order] = od.ID\r\nJOIN Book bk ON ordt.Book = bk.ID \r\n Where od.[Date]>='{dateFrom}' and od.[Date]<='{dateTo}' \r\nGROUP BY od.ID,od.[Date]\r\nORDER BY od.[Date] DESC \r\nOFFSET {over} ROWS \r\nFETCH NEXT 17 ROWS ONLY;";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var listorder = new List<ElementOrder>();
                            while (reader.Read())
                            {
                                ElementOrder _order = new ElementOrder()
                                {
                                    Id = (int)reader["ID"],
                                    Date = reader["Date"].ToString(),
                                    Quantity = (int)reader["Quantity"],
                                    Price = (double)reader["TotalPrice"]
                                    // Gán các thuộc tính khác của bảng Shop tương ứng
                                };
                                listorder.Add(_order);
                            }
                            System.Threading.Thread.Sleep(500);
                            return listorder;
                        }
                    }
                });
                _listOrder.Clear();
                foreach (var order in _orders)
                {
                    _listOrder.Add(order);
                }
                // Count
                var _counts = await Task.Run(() =>
                {
                    int over = _offset * 17;
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"SELECT Count(*) quantity\r\nfrom [Order] Where [Order].Date>= '{dateFrom}' and [Order].Date<= '{dateTo}'";
                    using (var command = new SqlCommand(query, connection))
                    {
                        int rowCount = (int)command.ExecuteScalar();
                        System.Threading.Thread.Sleep(500);

                        return rowCount;
                    }
                });
                int count = _counts / 17 + (_counts % 17 == 0 ? 0 : 1);
                _page._pageSize = count;
                _page._currentPage = 1;
                if (_page._currentPage == 1)
                {
                    previousPage.IsEnabled = false;
                }
                else
                {
                    previousPage.IsEnabled = true;
                }
                if (_page._currentPage == _page._pageSize)
                {
                    nextPage.IsEnabled = false;
                }
                else
                {
                    nextPage.IsEnabled = true;

                }
                DataContext = _page;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            finally
            {
                
                listOrder.ItemsSource = _listOrder;
            }
        }


        private async void getListOrderBySearchPage(string dateFrom, string dateTo)
        {

            try
            {
                var _orders = await Task.Run(() =>
                {
                    int over = _offset * 17;
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"SELECT od.ID, SUM(bk.Price * ordt.Quantity) AS TotalPrice, Sum(ordt.Quantity) as Quantity, od.[Date]\r\nFROM OrderDetail ordt\r\nJOIN [Order] od ON ordt.[Order] = od.ID\r\nJOIN Book bk ON ordt.Book = bk.ID \r\n Where od.[Date]>='{dateFrom}' and od.[Date]<='{dateTo}' \r\nGROUP BY od.ID,od.[Date]\r\nORDER BY od.[Date] DESC \r\nOFFSET {over} ROWS \r\nFETCH NEXT 17 ROWS ONLY;";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var listorder = new List<ElementOrder>();
                            while (reader.Read())
                            {
                                ElementOrder _order = new ElementOrder()
                                {
                                    Id = (int)reader["ID"],
                                    Date = reader["Date"].ToString(),
                                    Quantity = (int)reader["Quantity"],
                                    Price = (double)reader["TotalPrice"]
                                    // Gán các thuộc tính khác của bảng Shop tương ứng
                                };
                                listorder.Add(_order);
                            }
                            System.Threading.Thread.Sleep(500);
                            return listorder;
                        }
                    }
                });
                _listOrder.Clear();
                foreach (var order in _orders)
                {
                    _listOrder.Add(order);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            finally
            {

                listOrder.ItemsSource = _listOrder;
            }
        }


        private void ViewDetailOrder(object sender, RoutedEventArgs e)
        {
            if(listOrder.SelectedItem!=null)
            {
                ElementOrder _order= (ElementOrder)listOrder.SelectedItem;
                OrderInformationWindow _detailOrder = new OrderInformationWindow(_order);
                bool? _bool = _detailOrder.ShowDialog();
                if (_bool != null)
                {

                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("Lỗi: Vui lòng chọn Order", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}