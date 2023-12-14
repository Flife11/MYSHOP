using Microsoft.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Order
{
    /// <summary>
    /// Interaction logic for OrderInformationWindow.xaml
    /// </summary>
    public partial class OrderInformationWindow : Window
    {
        public OrderInformationWindow(ElementOrder order)
        {
            InitializeComponent();
            _order = order;
        }
        ElementOrder _order = new ElementOrder();

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        double _totalPrice = 0;
        private async void DetailOrderLoaded(object sender, RoutedEventArgs e)
        {
          
            try
            {
                var _books = await Task.Run(() =>
                {
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"select bk.Image,bk.ID, ordt.Quantity, bk.Title, bk.Price*ordt.Quantity as Price\r\nfrom OrderDetail ordt join book bk on ordt.Book= bk.ID\r\nwhere ordt.[Order]='{_order.Id}'";
                    using (var command = new SqlCommand(query, MainWindow.connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var listbook = new BindingList<Book>();
                            while (reader.Read())
                            {
                                Book _order = new Book()
                                {
                                    Id = (int)reader["ID"],
                                    Availability = (int)reader["Quantity"],
                                    Price = (double)reader["Price"],
                                    Title = reader["Title"].ToString(),
                                    // Gán các thuộc tính khác của bảng Shop tương ứng
                                    ImageUrl = reader["Image"].ToString(),
                                    // Gán các thuộc tính khác của bảng Shop tương ứng
                                };
                                listbook.Add(_order);
                            }
                            System.Threading.Thread.Sleep(500);
                            return listbook;
                        }
                    }
                });
                listBook.ItemsSource = _books;
                foreach (var book in _books)
                {
                    _totalPrice = _totalPrice + book.Price;
                }
                totalPrice.Text = _totalPrice.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            
        }

        private async void DeleteOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var _bookOrder = await Task.Run(() =>
                {
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"delete from OrderDetail where OrderDetail.[Order]='{_order.Id}'";
                    using (SqlCommand command = new SqlCommand(query, MainWindow.connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return 1;
                });

                var _deleteOrder = await Task.Run(() =>
                {
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"delete from [Order] where [Order].ID='{_order.Id}'";
                    using (SqlCommand command = new SqlCommand(query, MainWindow.connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return 1;
                });

                MainWindow._listOrder.Remove(_order);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            
        }

        private void EditOrderClick(object sender, RoutedEventArgs e)
        {
            EditOrderWindow _editWindow = new EditOrderWindow(_order);
            bool? _bool = _editWindow.ShowDialog();
            this.Close();
            if (_bool != null)
            {

            }
            else
            {

            }
        }
    }
}
