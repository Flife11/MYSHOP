using Entity;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

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
                    Id = id,
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


                    _categories.Add(new Category() { Id = ID, Name = Name });
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
    }
}
