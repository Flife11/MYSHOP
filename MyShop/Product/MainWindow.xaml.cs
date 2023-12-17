using Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
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
using DocumentFormat.OpenXml;  
using DocumentFormat.OpenXml.Packaging;  
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlClient;
using System.Formats.Tar;
using System.DirectoryServices;
using System.Configuration;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Data;
using MaterialDesignThemes.Wpf;
using System.Xml.Linq;

namespace Product
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        BindingList<Category> _categories;
        BindingList<book> _books;
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

            var sql = "SELECT * FROM Category";
            var command = new SqlCommand(sql, DB.Instance.Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = (int)reader["ID"];
                string name = (string)reader["Name"];

                var category = new Category()
                {
                    ID = id,
                    Name = name
                };
                _categories.Add(category);
            }
            reader.Close();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //connect database
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".";
            builder.InitialCatalog = "MyShop";
            builder.UserID = "sa";
            builder.Password = "0902894946orenji";
            builder.TrustServerCertificate = true;

            string connectionString = builder.ConnectionString;
            var connection = new SqlConnection(connectionString);

            connection = await Task.Run(() => {
                var _connection = new SqlConnection(connectionString);

                try
                {
                    _connection.Open();
                }
                catch (Exception ex)
                {
                    _connection = null;
                }

                // Test khi chạy quá nhanh
                //System.Threading.Thread.Sleep(2000);
                return _connection;
            });

            if (connection != null)
            {
                DB.Instance.ConnectionString = connectionString;
            }

            //tao bang category va book

            _categories = new BindingList<Category>();
            _books = new BindingList<book>();

            selectAllCategory();

            CategoryListBox.ItemsSource = _categories;

            
        }
        private async void Import_Button_Click(object sender, RoutedEventArgs e)
        {

            var categories = new BindingList<Category>();
            var books = new BindingList<book>();

            //Doc category
            string filePath = @"..\..\..\..\..\Category.csv"; //cho nay sua sau
            using (StreamReader sr = new StreamReader(filePath))
            {
                bool isFirstRow = true;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    var cells1 = line.Split(',');
                    int ID = int.Parse(cells1[0]);
                    string Name = cells1[1];


                    categories.Add(new Category() { ID = ID, Name = Name });
                }
                

            }

            //Doc book
            filePath = @"..\..\..\..\..\book.csv";

            using (StreamReader sr2 = new StreamReader(filePath))
            {
                bool isFirstRow = true;
                string line;
                while ((line = sr2.ReadLine()) != null)

                {
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    var cells = line.Split(';');
                    int ID = int.Parse(cells[0]);
                    string Title = cells[1];
                    float Price = float.Parse(cells[2]);
                    string Description = cells[3];
                    for (int i = 4; i< cells.Length -3; i++ )
                    {
                        Description += cells[i];
                    }

                    string Category = cells[cells.Length -3];
                    string Image = cells[cells.Length - 2];
                    int Availibility = int.Parse(cells[cells.Length -1]);


                    books.Add(new book() { ID = ID, Title = Title, Price = Price, Description = Description, Category = Category, Image = Image, Availability = Availibility });
                }

                
            }
           
            //insert
            try {
                for (int i = 0; i < categories.Count; i++)
                {
                    var sql = "INSERT INTO Category (ID, Name) VALUES (@ID, @Name)";
                    var command = new SqlCommand(sql, DB.Instance.Connection);
                    command.Parameters.AddWithValue("@ID", categories[i].ID);
                    command.Parameters.AddWithValue("@Name", categories[i].Name);

                    command.ExecuteNonQuery();
                }
                foreach (var category in categories)
                {
                    _categories.Add(category);
                }
                CategoryListBox.ItemsSource = _categories;

                for (int i = 0; i < books.Count; i++)
                {
                    var sql = "INSERT INTO book VALUES (@ID, @Title, @Price, @Description, @Category, @Image, @Availability)";
                    var command = new SqlCommand(sql, DB.Instance.Connection);
                    command.Parameters.AddWithValue("@ID", books[i].ID);
                    command.Parameters.AddWithValue("@Title", books[i].Title);
                    command.Parameters.AddWithValue("@Price", books[i].Price);
                    command.Parameters.AddWithValue("@Description", books[i].Description);
                    command.Parameters.AddWithValue("@Category", books[i].Category);
                    command.Parameters.AddWithValue("@Image", books[i].Image);
                    command.Parameters.AddWithValue("@Availability", books[i].Availability);

                    command.ExecuteNonQuery();

                }
                //foreach (var book in books)
                //{
                //    _books.Add(book);
                //}
                //booksListView.ItemsSource = _books;
            } catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }

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
            

            if(sortSelection != null)
            {
                _sortBy = sortSelection.Content.ToString();
            }
            if(optionSelection != null)
            {
                _sortOption = optionSelection.Content.ToString();
                if(_sortOption == "Ascending")
                {
                    _sortOption = "ASC";
                }
                else if(_sortOption == "Descending")
                {
                    _sortOption = "DESC";
                }
       
            }

            // sort ascending
            if(_sortOption == "ASC")
            {
                if (_sortBy == "ID")
                    _books = new BindingList<book>(_books.OrderBy(c => c.ID).ToList());
                else if (_sortBy == "Title")
                    _books = new BindingList<book>(_books.OrderBy(c => c.Title).ToList());
                else if (_sortBy == "Price")
                    _books = new BindingList<book>(_books.OrderBy(c => c.Price).ToList());
                else if (_sortBy == "Availability")
                    _books = new BindingList<book>(_books.OrderBy(c => c.Availability).ToList());
            }
                
            else if (_sortOption == "DESC")
            {
                if (_sortBy == "ID")
                    _books = new BindingList<book>(_books.OrderByDescending(c => c.ID).ToList());
                else if (_sortBy == "Title")
                    _books = new BindingList<book>(_books.OrderByDescending(c => c.Title).ToList());
                else if (_sortBy == "Price")
                    _books = new BindingList<book>(_books.OrderByDescending(c => c.Price).ToList());
                else if (_sortBy == "Availability")
                    _books = new BindingList<book>(_books.OrderByDescending(c => c.Availability).ToList());
            }
            booksListView.ItemsSource = _books;
        }

        private void searchBook()
        {
            _books.Clear();

            var sql = $"SELECT *, count(*) over() as Total FROM book " +
                $"WHERE Title LIKE @Keyword AND Price >= @minPrice and Price <= @maxPrice " +
                $"ORDER BY {_sortBy} {_sortOption} " +
                $"OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.Add("@Skip", SqlDbType.Int)
                .Value = (_currentPage - 1) * _rowsPerPage;
            command.Parameters.Add("@Take", SqlDbType.Int)
                .Value = _rowsPerPage;
            command.Parameters.Add("@minPrice", SqlDbType.Int)
                .Value = _minPrice;
            command.Parameters.Add("@maxPrice", SqlDbType.Int)
                .Value = _maxPrice;
            //var keyword = SearchTextBox.Text;
            command.Parameters.Add("@Keyword", SqlDbType.Text)
                .Value = $"%{_searchText}%";

            var reader = command.ExecuteReader();

            int count = -1;
            while (reader.Read())
            {
                int id = (int)reader["ID"];
                string title = (string)reader["Title"];
                double price = (double)reader["Price"];
                string description = (string)reader["Description"];
                string category = (string)reader["Category"];
                string image = (string)reader["Image"];
                int availability = (int)reader["Availability"];

                var bookitem = new book()
                {
                    ID = id,
                    Title = title,
                    Price = (float)price,
                    Description = description,
                    Category = category,
                    Image = image,
                    Availability = availability
                };
                _books.Add(bookitem);
                count = (int)reader["Total"];
            }
            reader.Close();

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
            
            var sql = $"SELECT *, count(*) over() as Total FROM book " +
                $"WHERE Category = '{name}' AND Title LIKE @Keyword AND Price >= @minPrice and Price <= @maxPrice " +
                $"ORDER BY {_sortBy} {_sortOption} " +
                $"OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.Add("@Skip", SqlDbType.Int)
                .Value = (_currentPage - 1) * _rowsPerPage;
            command.Parameters.Add("@Take", SqlDbType.Int)
                .Value = _rowsPerPage;
            command.Parameters.Add("@minPrice", SqlDbType.Int)
                .Value = _minPrice;
            command.Parameters.Add("@maxPrice", SqlDbType.Int)
                .Value = _maxPrice;
            //var keyword = SearchTextBox.Text;
            command.Parameters.Add("@Keyword", SqlDbType.Text)
                .Value = $"%{_searchText}%";

            var reader = command.ExecuteReader();

            int count = -1;
            while (reader.Read())
            {
                int id = (int)reader["ID"];
                string title = (string)reader["Title"];
                double price = (double)reader["Price"];
                string description = (string)reader["Description"];
                string category = (string)reader["Category"];
                string image = (string)reader["Image"];
                int availability = (int)reader["Availability"];

                var bookitem = new book()
                {
                    ID = id,
                    Title = title,
                    Price = (float)price,
                    Description = description,
                    Category = category,
                    Image = image,
                    Availability = availability
                };
                _books.Add(bookitem);
                count = (int)reader["Total"];
            }
            reader.Close();

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
            var selectedBook = (book)booksListView.SelectedItem;
            var screen = new DetailBookWindow(selectedBook);

            screen.ShowDialog();
            
        }

        //delete and edit category
        private void DeleteCategory_Button_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedCategory = (Category)CategoryListBox.SelectedItem;
            if(selectedCategory == null)
            {
                MessageBox.Show("Please select a category to delete");
                return;
            }

            var sql = $"DELETE FROM book where Category = '{selectedCategory.Name}'";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();

            sql = $"DELETE FROM Category where ID = {selectedCategory.ID}";
            command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();

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
            var screen = new EditCategoryWindow(category);           

            if (screen.ShowDialog()!.Value == true)
            {
                var sql = $"UPDATE Category SET Name='{screen.editCategory.Name}' WHERE ID = {_backupCat.ID}";
                var command = new SqlCommand(sql, DB.Instance.Connection);
                command.ExecuteNonQuery();

                sql = $"UPDATE book SET Category='{screen.editCategory.Name}' WHERE Category = '{_backupCat.Name}'";
                command = new SqlCommand(sql, DB.Instance.Connection);
                command.ExecuteNonQuery();

                screen.editCategory.CopyProperties(category);
            }
            else
            {
                _backupCat.CopyProperties(category);
            }

        }
        //delete and edit book
        private void DeleteBook_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = (book)booksListView.SelectedItem;
            if(selectedBook == null)
            {
                MessageBox.Show("Please select a book to delete");
                return;
            }

            var sql = $"DELETE FROM book where ID = {selectedBook.ID}";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();

            _books.Remove(selectedBook);
        }
        private void EditBook_Button_Click(object sender, RoutedEventArgs e)
        {
            book _backupBook;
            var book = (book)booksListView.SelectedItem;
            if (book == null)
            {
                MessageBox.Show("Please select a book to edit");
                return;
            }
            _backupBook = (book)book.Clone();
            var screen = new EditBookWindow(_categories, book);
            if (screen.ShowDialog()!.Value == true)
            {
                var sql = $"UPDATE book SET Title='{screen.editBook.Title}', Price={screen.editBook.Price}, Description='{screen.editBook.Description}', Category='{screen.editBook.Category}', Image='{screen.editBook.Image}', Availability={screen.editBook.Availability} WHERE ID = {_backupBook.ID}";
                var command = new SqlCommand(sql, DB.Instance.Connection);
                command.ExecuteNonQuery();

                screen.editBook.CopyProperties(book);
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
            var screen = new AddCategoryWindow();
            screen.ShowDialog();

            selectAllCategory();
        }

        private void AddBook_Button_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddBookWindow(_categories);
            screen.ShowDialog();

            selectBookByCategory(_categoryName);
        }
    }
}
