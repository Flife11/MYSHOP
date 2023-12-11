using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Order
{
    /// <summary>
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
        }
        BindingList<Book> _orderBooks = new BindingList<Book>();
        public Book BookSelected { get; set; }
        public double _totalPrice = 0;
        private void addProDuctToOrder(object sender, RoutedEventArgs e)
        {
            Book _book = BookSelected.Clone();
            string _amount = amountBook.Text;
            if(int.TryParse(_amount, out int result))
            {
                int quantity = result;
                if(quantity > _book.Availability)
                {
                    MessageBox.Show("Lỗi: Số lượng vượt quá giới hạn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    _book.Availability = quantity;
                    _orderBooks.Add(_book);
                    listProductOfOrder.ItemsSource = _orderBooks;
                    _totalPrice = _totalPrice + quantity*_book.Price;
                    TotalPrice.Text = _totalPrice.ToString();
                }
            }
            else
            {
                MessageBox.Show("Lỗi: Giá trị không phải là số nguyên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void SaveOrderClick(object sender, RoutedEventArgs e)
        {
            int sumQuantity = 0;
            foreach(Book _book in _orderBooks)
            {
                sumQuantity += _book.Availability;
            }
            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now;
            string _date= currentDate.ToString("yyyy-MM-dd HH:mm:ss");
            ElementOrder _order = new ElementOrder()
            {
                Id = 1,
                Date = _date,
                Quantity = sumQuantity,
                Price = _totalPrice
            };

            MainWindow._listOrder.Add(_order);
            Close();
        }
        public void OrderWindow_Load(object sender, RoutedEventArgs e)
        {
            getListCateGory();
            TotalPrice.Text=_totalPrice.ToString();
        }
        private void DeleteOrderClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void getListCateGory()
        {
            string connectionString = "Server=LAPTOP-K39M1QD9;Database=MyShop;Trusted_Connection=yes;TrustServerCertificate=True;";
            SqlConnection connection = null;
            try
            {
                var categories = await Task.Run(() =>
                {
                    using (connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                        string query = "SELECT * FROM Category";
                        using (var command = new SqlCommand(query, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                var _cateList = new List<Category>();

                                while (reader.Read())
                                {
                                    var category = new Category
                                    {
                                        Id = (int)reader["ID"],
                                        Name = reader["Name"].ToString(),
                                        // Gán các thuộc tính khác của bảng Shop tương ứng
                                    };

                                    _cateList.Add(category);
                                }

                                return _cateList;
                            }
                        }
                    }
                });

                // Hiển thị dữ liệu hoặc thực hiện các thao tác khác với danh sách sản phẩm
                dropBoxTypeBook.ItemsSource = categories.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            finally
            {
                // Đảm bảo rằng kết nối được đóng ngay cả khi có ngoại lệ
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Kết nối đã được đóng.");
                }
            }
        }

        private async void LoadListProductWithCategory(object sender, SelectionChangedEventArgs e)
        {
            if(dropBoxTypeBook.SelectedItem==null)
            {
                return;
            }    
            Category _category= (Category)dropBoxTypeBook.SelectedItem;
            string _nameCategory=_category.Name;

            string connectionString = "Server=LAPTOP-K39M1QD9;Database=MyShop;Trusted_Connection=yes;TrustServerCertificate=True;";
            SqlConnection connection = null;
            try
            {
                var books = await Task.Run(() =>
                {
                    using (connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                        string query = $"SELECT  Id, Title, Category, Image,Availability,Price  FROM book Where Category = '{_nameCategory}' ";
                        using (var command = new SqlCommand(query, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                var _bookList = new BindingList<Book>();

                                while (reader.Read())
                                {
                                    
                                    var book = new Book
                                    {
                                        Id = (int)reader["ID"],
                                        Availability = (int)reader["Availability"],
                                        Price = (double)reader["Price"],
                                        Title = reader["Title"].ToString(),
                                        // Gán các thuộc tính khác của bảng Shop tương ứng
                                        Category = reader["Category"].ToString(),
                                        ImageUrl = reader["Image"].ToString(),

                                    };

                                    _bookList.Add(book);
                                }
                                System.Threading.Thread.Sleep(500);
                                return _bookList;
                            }
                        }
                    }
                });

                // Hiển thị dữ liệu hoặc thực hiện các thao tác khác với danh sách sản phẩm
                listProductOfType.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
            finally
            {
                // Đảm bảo rằng kết nối được đóng ngay cả khi có ngoại lệ
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Kết nối đã được đóng.");
                }
            }
        }
        private void SelectProduct(object sender, SelectionChangedEventArgs e)
        {
            BookSelected = (Book)listProductOfType.SelectedItem;
            DataContext = BookSelected;
        }

        private void DeleteBookInOrder(object sender, RoutedEventArgs e)
        {
            if(listProductOfOrder.SelectedItems!=null)
            {
                Book _book = (Book)listProductOfOrder.SelectedItem;
                _orderBooks.Remove( _book );
                _totalPrice =_totalPrice- _book.Price * _book.Availability;
                TotalPrice.Text= _totalPrice.ToString();
            }
        }
    }
}
