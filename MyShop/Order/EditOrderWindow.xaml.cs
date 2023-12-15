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
    /// Interaction logic for EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        public EditOrderWindow(ElementOrder order)
        {
            InitializeComponent();
            _order = order;
        }
        ElementOrder _order= new ElementOrder();

        BindingList<Book> _orderBooks = new BindingList<Book>();
        public Book BookSelected { get; set; }
        public double _totalPrice = 0;
        private void addProDuctToOrder(object sender, RoutedEventArgs e)
        {
            if (listProductOfType.SelectedItem == null)
            {
                MessageBox.Show("Lỗi: Vui lòng chọn Book", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Book _book = BookSelected.Clone();
            string _amount = amountBook.Text;
            if (int.TryParse(_amount, out int result))
            {
                int quantity = result;
                if (quantity > _book.Availability)
                {
                    MessageBox.Show("Lỗi: Số lượng vượt quá giới hạn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    _book.Availability = quantity;
                    _book.Price = quantity * _book.Price;
                    bool checkInListBook = false;
                    foreach (Book bookItem in _orderBooks)
                    {
                        if (bookItem.Id == _book.Id)
                        {
                            checkInListBook = true;
                            bookItem.Price = bookItem.Price + _book.Price;
                            bookItem.Availability = bookItem.Availability + _book.Availability;
                            break;
                        }
                    }
                    if (!checkInListBook)
                    {
                        _orderBooks.Add(_book);
                    }
                    listProductOfOrder.ItemsSource = _orderBooks;
                    _totalPrice = _totalPrice + _book.Price;
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
            int total = 0;
            foreach(Book _book in _orderBooks)
            {
                total=total + _book.Availability;
                deleteInOrderDetail(MainWindow.connection, _book, _order.Id);
                insertOrderDetail(MainWindow.connection, _book, _order.Id);
            }
            MainWindow._listOrder[MainWindow.index].Price = _totalPrice;
            MainWindow._listOrder[MainWindow.index].Quantity = total;

            Close();
        }
        public void EditOrder_Loaded(object sender, RoutedEventArgs e)
        {
            getListCateGory();
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void getListCateGory()
        {

            try
            {
                var categories = await Task.Run(() =>
                {
                    string query = "SELECT * FROM Category";
                    using (var command = new SqlCommand(query, MainWindow.connection))
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
                });

                // Hiển thị dữ liệu hoặc thực hiện các thao tác khác với danh sách sản phẩm
                dropBoxTypeBook.ItemsSource = categories.ToList();
                loadListProduct(MainWindow.connection);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }

        }
        private async void loadListProduct(SqlConnection connection)
        {
            try
            {
                var _books = await Task.Run(() =>
                {
                    string query = $"select bk.Image,bk.ID, ordt.Quantity, bk.Title, bk.Price*ordt.Quantity as Price\r\nfrom OrderDetail ordt join book bk on ordt.Book= bk.ID\r\nwhere ordt.[Order]='{_order.Id}'";
                    using (var command = new SqlCommand(query,connection))
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
                foreach (var book in _books)
                {
                    _orderBooks.Add(book);
                    _totalPrice = _totalPrice + book.Price;
                }
                TotalPrice.Text = _totalPrice.ToString();
                listProductOfOrder.ItemsSource = _orderBooks;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}");
            }
           
        }
        private async void LoadListProductWithCategory(object sender, SelectionChangedEventArgs e)
        {
            if (dropBoxTypeBook.SelectedItem == null)
            {
                return;
            }
            Category _category = (Category)dropBoxTypeBook.SelectedItem;
            string _nameCategory = _category.Name;


            try
            {
                var books = await Task.Run(() =>
                {
                    string query = $"SELECT  Id, Title, Category, Image,Availability,Price  FROM book Where Category = '{_nameCategory}' ";
                    using (var command = new SqlCommand(query, MainWindow.connection))
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
                            System.Threading.Thread.Sleep(1000);
                            return _bookList;
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

        }
        private void SelectProduct(object sender, SelectionChangedEventArgs e)
        {
            BookSelected = (Book)listProductOfType.SelectedItem;
            DataContext = BookSelected;
        }

        private void DeleteBookInOrder(object sender, RoutedEventArgs e)
        {
            if (listProductOfOrder.SelectedItems != null)
            {
                Book _book = (Book)listProductOfOrder.SelectedItem;
                _orderBooks.Remove(_book);
                _totalPrice = _totalPrice - _book.Price * _book.Availability;
                TotalPrice.Text = _totalPrice.ToString();
            }
        }
     
        public int GetNextId(SqlConnection connection, string tableName, string idColumnName)
        {
            // Tìm giá trị ID lớn nhất hiện tại trong bảng
            string maxIdQuery = $"SELECT MAX({idColumnName}) FROM {tableName}";
            using (SqlCommand maxIdCommand = new SqlCommand(maxIdQuery, connection))
            {
                object maxId = maxIdCommand.ExecuteScalar();
                int nextId = (maxId == DBNull.Value) ? 1 : ((int)maxId + 1);
                return nextId;
            }
        }
        public void insertOrderDetail(SqlConnection connection, Book _book, int IDOrder)
        {
            string insertQuery = "INSERT INTO [OrderDetail] (ID,[Order],Book,Quantity) VALUES (@Value1,@Value2,@Value3,@Value4)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                // Thêm các tham số cho truy vấn
                int insertedId = GetNextId(connection, "OrderDetail", "ID");
                command.Parameters.AddWithValue("@Value1", $"{insertedId}");
                command.Parameters.AddWithValue("@Value2", $"{IDOrder}");
                command.Parameters.AddWithValue("@Value3", $"{_book.Id}");
                command.Parameters.AddWithValue("@Value4", $"{_book.Availability}");

                // Lấy giá trị ID vừa được insert
                command.ExecuteScalar();

                // Sử dụng giá trị ID nếu cần
            }
            string updateQuery = $"Update book\r\nset Availability= Availability - {_book.Availability}\r\nwhere ID={_book.Id}";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public void deleteInOrderDetail(SqlConnection connection, Book _book, int IDOrder)
        {

            string updateQuery = $"Update book\r\nset Availability= Availability + {_book.Availability}\r\nwhere ID={_book.Id}";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string deleteQuery = $"Delete from OrderDetail where Book={_book.Id} and [Order]= {IDOrder}";

            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

    }
}
