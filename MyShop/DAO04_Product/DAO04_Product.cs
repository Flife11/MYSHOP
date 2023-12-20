using Entity;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;
using static System.Net.Mime.MediaTypeNames;

namespace DAO04_Product
{
    public class DAO04_Product : IDAO
    {
        public override BindingList<Category> selectAllCategory()
        {
            var sql = "SELECT * FROM Category";
            var command = new SqlCommand(sql, DB.Instance.Connection);

            var reader = command.ExecuteReader();
            BindingList<Category> _categories = new BindingList<Category>();

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
            return _categories;
        }
        public override BindingList<Category> readCategoryFile(string filePath)
        {
            BindingList < Category > _categories = new BindingList<Category>();
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


                    _categories.Add(new Category() { ID = ID, Name = Name });
                }
            }
            return _categories;
        }

        public override BindingList<Book> readBookFile(string filePath)
        {
            BindingList < Book > books = new BindingList<Book>();
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
                    for (int i = 4; i < cells.Length - 3; i++)
                    {
                        Description += cells[i];
                    }

                    string Category = cells[cells.Length - 3];
                    string Image = cells[cells.Length - 2];
                    int Availibility = int.Parse(cells[cells.Length - 1]);


                    books.Add(new Book() { id = ID, title = Title, price = Price, description = Description, Category = Category, imgurl = Image, availability = Availibility });
                }
            }
            return books;
        }
        public override void insertCategoryAndBook(BindingList<Category> categories, BindingList<Book> books)
        {
            try
            {
                var _categories = new BindingList<Category>();
                for (int i = 0; i < categories.Count; i++)
                {
                    var sql = "INSERT INTO Category (ID, Name) VALUES (@ID, @Name)";
                    var command = new SqlCommand(sql, DB.Instance.Connection);
                    command.Parameters.AddWithValue("@ID", categories[i].ID);
                    command.Parameters.AddWithValue("@Name", categories[i].Name);

                    command.ExecuteNonQuery();
                }                

                for (int i = 0; i < books.Count; i++)
                {
                    var sql = "INSERT INTO book VALUES (@ID, @Title, @Price, @Description, @Category, @Image, @Availability)";
                    var command = new SqlCommand(sql, DB.Instance.Connection);
                    command.Parameters.AddWithValue("@ID", books[i].id);
                    command.Parameters.AddWithValue("@Title", books[i].title);
                    command.Parameters.AddWithValue("@Price", books[i].Price);
                    command.Parameters.AddWithValue("@Description", books[i].description);
                    command.Parameters.AddWithValue("@Category", books[i].category);
                    command.Parameters.AddWithValue("@Image", books[i].imgurl);
                    command.Parameters.AddWithValue("@Availability", books[i].availability);

                    command.ExecuteNonQuery();

                }
                //foreach (var book in books)
                //{
                //    _books.Add(book);
                //}
                //booksListView.ItemsSource = _books;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override Tuple<BindingList<Book>, int> searchBook(string _sortBy, string _sortOption, string _searchText, int _currentPage,
            int _rowsPerPage, int _minPrice, int _maxPrice)
        {
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
            var _books = new BindingList<Book>();
            while (reader.Read())
            {
                int id = (int)reader["ID"];
                string title = (string)reader["Title"];
                double price = (double)reader["Price"];
                string description = (string)reader["Description"];
                string category = (string)reader["Category"];
                string image = (string)reader["Image"];
                int availability = (int)reader["Availability"];

                var bookitem = new Book() { id = id, title = title, price = (float)price, description = description, category = category, imgurl = image, availability = availability };
                _books.Add(bookitem);
                count = (int)reader["Total"];
            }
            reader.Close();
            return Tuple.Create(_books, count);
        }
        public override Tuple<BindingList<Book>, int> selectBookByCategory(string name, string _sortBy, string _sortOption, string _searchText, int _currentPage,
            int _rowsPerPage, int _minPrice, int _maxPrice)
        {
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
            var _books = new BindingList<Book>();
            while (reader.Read())
            {
                int id = (int)reader["ID"];
                string title = (string)reader["Title"];
                double price = (double)reader["Price"];
                string description = (string)reader["Description"];
                string category = (string)reader["Category"];
                string image = (string)reader["Image"];
                int availability = (int)reader["Availability"];

                var bookitem = new Book() { id = id, title = title, price = (float)price, description = description, category = category, imgurl = image, availability = availability };
                _books.Add(bookitem);
                count = (int)reader["Total"];
            }
            reader.Close();
            return Tuple.Create(_books, count);
        }
        public override void DeleteCategory(string Name, int Id)
        {
            var sql = $"DELETE FROM book where Category = '{Name}'";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();

            sql = $"DELETE FROM Category where ID = {Id}";
            command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();
        }
        public override bool SaveCategory(string CategoryName)
        {
            var sql = $"SELECT* FROM Category where Name = '{CategoryName}'";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {                
                reader.Close();
                return true;
            }
            reader.Close();

            //select max id of Category +1 and cast to int
            sql = "SELECT MAX(ID) FROM Category";
            command = new SqlCommand(sql, DB.Instance.Connection);
            reader = command.ExecuteReader();
            reader.Read();
            var id = reader.GetInt32(0) + 1;
            reader.Close();

            sql = $"INSERT INTO Category VALUES({id},'{CategoryName}')";
            command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();
            return false;
        }
        public override void AddBook(string title, string price, string description, string category, string image, string availability)
        {
            var sql = "SELECT MAX(ID) FROM book";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var id = reader.GetInt32(0) + 1;
            reader.Close();

            sql = "INSERT INTO book VALUES (@ID, @Title, @Price, @Description, @Category, @Image, @Availability)";
            command = new SqlCommand(sql, DB.Instance.Connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Price", float.Parse(price));
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@Category", category);
            command.Parameters.AddWithValue("@Image", image);
            command.Parameters.AddWithValue("@Availability", int.Parse(availability));

            command.ExecuteNonQuery();
        }
        public override void EditBook(Book editBook, int id)
        {
            var sql = $"UPDATE book SET Title='{editBook.title}', Price={editBook.price}, " +
                $"Description='{editBook.description}', Category='{editBook.Category}', Image='{editBook.imgurl}', Availability={editBook.availability} WHERE ID = {id}";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();
        }
        public override void DeleteBook(int Id)
        {
            var sql = $"DELETE FROM book where ID = {Id}";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();
        }
        public override void EditCategory(Category editCat, int ID, string Name)
        {
            var sql = $"UPDATE Category SET Name='{editCat.Name}' WHERE ID = {ID}";
            var command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();

            sql = $"UPDATE book SET Category='{editCat.Name}' WHERE Category = '{Name}'";
            command = new SqlCommand(sql, DB.Instance.Connection);
            command.ExecuteNonQuery();
        }
        public override Task<bool> ConnectDB(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void deleteInOrderDetail(Book _book, int IDOrder)
        {
            throw new NotImplementedException();
        }

        public override void DeleteOrder(ElementOrder _order)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> GetBookByCategory(string _nameCategory)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> GetDetailOrder(int Id)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Category>> getListCateGory()
        {
            throw new NotImplementedException();
        }

        public override Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset)
        {
            throw new NotImplementedException();
        }

        public override Task<Tuple<List<ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset)
        {
            throw new NotImplementedException();
        }

        public override Task<List<ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset)
        {
            throw new NotImplementedException();
        }

        public override Task<List<ElementOrder>> getListOrderPage(int _offset)
        {
            throw new NotImplementedException();
        }

        public override int GetNextId(string tableName, string idColumnName)
        {
            throw new NotImplementedException();
        }

        public override Task<int> insertOrder(string _date, ElementOrder _order)
        {
            throw new NotImplementedException();
        }

        public override void insertOrderDetail(Book _book, int IDOrder)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> loadListProduct(int Id)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> LoadListProductWithCategory(string _nameCategory)
        {
            throw new NotImplementedException();
        }

        public override Tuple<string, string> LoadServerFromConfig()
        {
            throw new NotImplementedException();
        }

        public override Tuple<string, string> LoadUserFromConfig()
        {
            throw new NotImplementedException();
        }

        public override string Name()
        {
            return "product";
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void SaveServerToConfig(string server, string database)
        {
            throw new NotImplementedException();
        }

        public override void SaveUserToConfig(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Book> SoldingOutPr_Lv()
        {
            throw new NotImplementedException();
        }

        public override List<string> LoadDashInfor(DateTime curDate, DateTime beginMonth_Date, DateTime previousMonday)
        {
            throw new NotImplementedException();
        }

        public override Tuple<List<string>, List<float>> ChartInfor(DateTime? _beginDate, DateTime? _endDate, TimeSpan? dateDiff)
        {
            throw new NotImplementedException();
        }

        public override Tuple<List<string>, List<int>> BooksAndQuantity(DateTime? _begin, DateTime? _end)
        {
            throw new NotImplementedException();
        }
    }
}
