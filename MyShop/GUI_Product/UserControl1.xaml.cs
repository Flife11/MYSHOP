using Enity;
using Entity;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThreeLayerContract;

namespace GUI_Product
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
        BindingList<Category> _categories;
        BindingList<Book> _books;
        int _rowsPerPage = 10;
        int _totalPages = -1;
        int _totalItems = -1;
        int _currentPage = 1;

        int _minPrice = 0;
        int _maxPrice = int.MaxValue;
        string _searchText = "";

        string _sortBy = "ID";
        string _sortOption = "ASC";
        string _categoryName = "";

        private void selectAllCategory()
        {
            _categories.Clear();
            var data = _bus.selectAllCategory();
            _categories = data;
            CategoryListBox.ItemsSource = _categories;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            //tao bang category va book

            _categories = new BindingList<Category>();
            _books = new BindingList<Book>();

            selectAllCategory();

            CategoryListBox.ItemsSource = _categories;
        }
        private async void Import_Button_Click(object sender, RoutedEventArgs e)
        {
            var categories = new BindingList<Category>();
            var books = new BindingList<Book>();

            //Doc category
            string filePath = @"..\..\..\..\Category.csv"; //cho nay sua sau
            categories = _bus.readCategoryFile(filePath);

            //Doc book
            filePath = @"..\..\..\..\book.csv";
            books = _bus.readBookFile(filePath);

            //insert
            _bus.insertCategoryAndBook(categories, books);
            foreach (var category in categories)
            {
                _categories.Add(category);
            }
            CategoryListBox.ItemsSource = _categories;

        }

        private void Category_OnDialogClosed(object sender, MaterialDesignThemes.Wpf.DialogClosedEventArgs eventArgs)
        {

        }

        private void Category_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {

        }

        private void pagingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic info = pagingComboBox.SelectedItem;
            if (info != null)
            {
                if (info?.Page != _currentPage)
                {
                    _currentPage = info?.Page;
                    selectBookByCategory(_categoryName);
                }
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (pagingComboBox.SelectedIndex > 0)
            {
                pagingComboBox.SelectedIndex--;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (pagingComboBox.SelectedIndex >= 0)
            {
                pagingComboBox.SelectedIndex++;
            }
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sortSelection = sortComboBox.SelectedItem as ComboBoxItem;
            var optionSelection = optionComboBox.SelectedItem as ComboBoxItem;


            if (sortSelection != null)
            {
                _sortBy = sortSelection.Content.ToString();
            }
            if (optionSelection != null)
            {
                _sortOption = optionSelection.Content.ToString();
                if (_sortOption == "Ascending")
                {
                    _sortOption = "ASC";
                }
                else if (_sortOption == "Descending")
                {
                    _sortOption = "DESC";
                }

            }

            // sort ascending
            if (_sortOption == "ASC")
            {
                if (_sortBy == "ID")
                    _books = new BindingList<Book>(_books.OrderBy(c => c.id).ToList());
                else if (_sortBy == "Title")
                    _books = new BindingList<Book>(_books.OrderBy(c => c.title).ToList());
                else if (_sortBy == "Price")
                    _books = new BindingList<Book>(_books.OrderBy(c => c.price).ToList());
                else if (_sortBy == "Availability")
                    _books = new BindingList<Book>(_books.OrderBy(c => c.availability).ToList());
            }

            else if (_sortOption == "DESC")
            {
                if (_sortBy == "ID")
                    _books = new BindingList<Book>(_books.OrderByDescending(c => c.id).ToList());
                else if (_sortBy == "Title")
                    _books = new BindingList<Book>(_books.OrderByDescending(c => c.title).ToList());
                else if (_sortBy == "Price")
                    _books = new BindingList<Book>(_books.OrderByDescending(c => c.price).ToList());
                else if (_sortBy == "Availability")
                    _books = new BindingList<Book>(_books.OrderByDescending(c => c.availability).ToList());
            }
            booksListView.ItemsSource = _books;
        }

        private void searchBook()
        {
            _books.Clear();
            var data = _bus.searchBook(_sortBy, _sortOption, _searchText, _currentPage, _rowsPerPage, _minPrice, _maxPrice);
            int count = data.Item2;
            _books = data.Item1;

            if (count != _totalItems)
            {
                _totalItems = count;
                _totalPages = (_totalItems / _rowsPerPage) +
                    (((_totalItems % _rowsPerPage) == 0) ? 0 : 1);

                // Tạo thông tin phân trang cho combobox
                var pageInfos = new List<object>();

                for (int i = 1; i <= _totalPages; i++)
                {
                    pageInfos.Add(new
                    {
                        Page = i,
                        Total = _totalPages
                    });
                };

                pagingComboBox.ItemsSource = pageInfos;
                pagingComboBox.SelectedIndex = 0;
            }
        }        

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Text = "";
            MinPrice.Text = "";
            MaxPrice.Text = "";

            _searchText = "";
            _minPrice = 0;
            _maxPrice = int.MaxValue;

            var listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                var selectedItem = listBox.SelectedItem as Category; // replace YourItemType with the actual type of your items
                _categoryName = selectedItem.Name;
                //select book
                selectBookByCategory(_categoryName);

                booksListView.ItemsSource = _books;
            }
        }
        private void selectBookByCategory(string name)
        {
            _books.Clear();
            var data = _bus.selectBookByCategory(name, _sortBy, _sortOption, _searchText, _currentPage, _rowsPerPage, _minPrice, _maxPrice);
            int count = data.Item2;
            _books = data.Item1;
            booksListView.ItemsSource = _books.ToArray();

           if (count != _totalItems)
            {
                _totalItems = count;
                _totalPages = (_totalItems / _rowsPerPage) +
                    (((_totalItems % _rowsPerPage) == 0) ? 0 : 1);

                // Tạo thông tin phân trang cho combobox
                var pageInfos = new List<object>();

                for (int i = 1; i <= _totalPages; i++)
                {
                    pageInfos.Add(new
                    {
                        Page = i,
                        Total = _totalPages
                    });
                };

                pagingComboBox.ItemsSource = pageInfos;
                pagingComboBox.SelectedIndex = 0;
            }
        }

        private void ApplyFilter_Button_Click(object sender, RoutedEventArgs e)
        {
            string minPricestring = MinPrice.Text;
            if (minPricestring != "")
            {
                _minPrice = Int32.Parse(minPricestring);
            }
            else
            {
                _minPrice = 0;
            }


            string maxPricestring = MaxPrice.Text;
            if (maxPricestring != "")
            {
                _maxPrice = Int32.Parse(maxPricestring);
            }
            else
            {
                _maxPrice = int.MaxValue;
            }

            _searchText = SearchTextBox.Text;

            searchBook();
            booksListView.ItemsSource = _books;
        }

        private void Book_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedBook = (Book)booksListView.SelectedItem;
            var detailBook_GUI = new DetailBook_GUI(_bus, selectedBook);
            var screen = new PopupProduct(detailBook_GUI);

            screen.ShowDialog();
        }

        //delete and edit category
        private void DeleteCategory_Button_Click(object sender, RoutedEventArgs e)
        {

            var selectedCategory = (Category)CategoryListBox.SelectedItem;
            if (selectedCategory == null)
            {
                MessageBox.Show("Please select a category to delete");
                return;
            }

            _bus.DeleteCategory(selectedCategory.Name, selectedCategory.ID);           

            _books.Clear();
            _categories.Remove(selectedCategory);
            selectBookByCategory(_categoryName);
        }

        //Category _backupCat;
        private void EditCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            Category _backupCat;
            var category = (Category)CategoryListBox.SelectedItem;
            if (category == null)
            {
                MessageBox.Show("Please select a category to edit");
                return;
            }

            _backupCat = (Category)category.Clone();
            var editCategory_GUI = new EditCategory_GUI(_bus, category);
            var screen = new PopupProduct(editCategory_GUI);
            //var screen = new EditCategoryWindow(category);

            if (screen.ShowDialog().Value == true)
            {
                //_bus.EditCategory();
                selectAllCategory();
            }
            else
            {
                _backupCat.CopyProperties(category);
            }

        }
        //delete and edit book
        private void DeleteBook_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = (Book)booksListView.SelectedItem;
            if (selectedBook == null)
            {
                MessageBox.Show("Please select a book to delete");
                return;
            }

            _bus.DeleteBook(selectedBook.Id);            

            _books.Remove(selectedBook);
            booksListView.ItemsSource = _books;

        }
        private void EditBook_Button_Click(object sender, RoutedEventArgs e)
        {
            Book _backupBook;
            var book = (Book)booksListView.SelectedItem;
            if (book == null)
            {
                MessageBox.Show("Please select a book to edit");
                return;
            }
            _backupBook = (Book)book.Clone();

            var editBook_GUI = new EditBook_GUI(_bus, _categories, book);
            var screen = new PopupProduct(editBook_GUI);
            if (screen.ShowDialog().Value == true)
            {
                /*_bus.EditBook(screen.editBook.Title, screen.editBook.Title);
                var sql = $"UPDATE book SET Title='{screen.editBook.Title}', Price={screen.editBook.Title}, Description='{screen.editBook.Description}', Category='{screen.editBook.Category}', Image='{screen.editBook.Image}', Availability={screen.editBook.availability} WHERE ID = {_backupBook.id}";
                var command = new SqlCommand(sql, DB.Instance.Connection);
                command.ExecuteNonQuery();

                screen.editBook.CopyProperties(book);*/
                selectBookByCategory(_categoryName);
                //comment
            }
            else
            {
                _backupBook.CopyProperties(book);
            }
        }

        private void AddCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            var AddCategory = new AddCategory_GUI(_bus);            
            var screen = new PopupProduct(AddCategory);
            screen.ShowDialog();

            selectAllCategory();
        }

        private void AddBook_Button_Click(object sender, RoutedEventArgs e)
        {
            var AddBook = new AddBook_GUI(_bus, _categories);
            var screen = new PopupProduct(AddBook);            
            screen.ShowDialog();

            selectBookByCategory(_categoryName);
        }
    }
}
