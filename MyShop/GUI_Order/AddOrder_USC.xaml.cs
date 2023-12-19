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

namespace GUI_Order
{
    /// <summary>
    /// Interaction logic for AddOrder_USC.xaml
    /// </summary>
    public partial class AddOrder_USC : UserControl
    {
        IBus _bus;
        public AddOrder_USC(IBus bus)
        {
            _bus = bus;
            InitializeComponent();
        }
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
            int sumQuantity = 0;
            foreach (Book _book in _orderBooks)
            {
                sumQuantity += _book.Availability;
            }
            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now;
            string _date = currentDate.ToString("MM/dd/yyyy HH:mm:ss tt");
            ElementOrder _order = new ElementOrder()
            {
                Id = 1,
                Date = _date,
                Quantity = sumQuantity,
                Price = _totalPrice
            };

            var insertedId = await _bus.insertOrder(_date, _order);
            foreach (Book _book in _orderBooks)
            {
                _bus.insertOrderDetail(_book, insertedId);
            }
            UserControl1._listOrder.Insert(0, _order);

            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
        public async void OrderWindow_Load(object sender, RoutedEventArgs e)
        {
            var categories = await _bus.getListCateGory();
            // Hiển thị dữ liệu hoặc thực hiện các thao tác khác với danh sách sản phẩm
            dropBoxTypeBook.ItemsSource = categories.ToList();
            TotalPrice.Text = _totalPrice.ToString();
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }        

        private async void LoadListProductWithCategory(object sender, SelectionChangedEventArgs e)
        {
            if (dropBoxTypeBook.SelectedItem == null)
            {
                return;
            }
            Category _category = (Category)dropBoxTypeBook.SelectedItem;
            string _nameCategory = _category.Name;


            var books = await _bus.GetBookByCategory(_nameCategory);
            // Hiển thị dữ liệu hoặc thực hiện các thao tác khác với danh sách sản phẩm
            listProductOfType.ItemsSource = books;

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
    }
}
