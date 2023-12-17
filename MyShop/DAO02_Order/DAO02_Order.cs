using Entity;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace DAO02_Order
{
    public class DAO02_Order : IDAO
    {
        public override async Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);            
            try
            {
                connection.Open();
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
                connection.Close();
                return Tuple.Create(_orders, _counts);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async Task<List<ElementOrder>> getListOrderPage(int _offset)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
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
                connection.Close();
                return _orders;                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async Task<Tuple<List<ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
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
                connection.Close();
                return Tuple.Create(_orders, count);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async Task<List<ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
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
                connection.Close();
                return _orders;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        public override async Task<List<Category>> getListCateGory()
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var categories = await Task.Run(() =>
                {
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
                   
                });
                connection.Close();
                return categories;                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public override int GetNextId(string tableName, string idColumnName)
        {
            // Tìm giá trị ID lớn nhất hiện tại trong bảng
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string maxIdQuery = $"SELECT MAX({idColumnName}) FROM {tableName}";
                using (SqlCommand maxIdCommand = new SqlCommand(maxIdQuery, connection))
                {
                    object maxId = maxIdCommand.ExecuteScalar();
                    int nextId = (maxId == DBNull.Value) ? 1 : ((int)maxId + 1);
                    return nextId;
                }
            } catch (Exception ex)
            {
                throw ex;
            }            
        }
        public override void insertOrderDetail(Book _book, int IDOrder)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string insertQuery = "INSERT INTO [OrderDetail] (ID,[Order],Book,Quantity) VALUES (@Value1,@Value2,@Value3,@Value4)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Thêm các tham số cho truy vấn
                    int insertedId = GetNextId("OrderDetail", "ID");
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
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public override async Task<int> insertOrder(string _date, ElementOrder _order)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var orderID = await Task.Run(() =>
                {
                    // Lấy giá trị ID mới
                    int newId = GetNextId("[Order]", "ID");

                    string insertQuery = "INSERT INTO [Order] (ID,Date) OUTPUT INSERTED.ID VALUES (@Value1,@Value2)";
                    int insertedId;
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Thêm các tham số cho truy vấn
                        command.Parameters.AddWithValue("@Value2", $"{_date}");
                        command.Parameters.AddWithValue("@Value1", $"{newId}");

                        // Lấy giá trị ID vừa được insert
                        insertedId = (int)command.ExecuteScalar();


                    }
                    
                    return insertedId;
                });
                _order.Id = orderID;
                connection.Close();
                return orderID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public override async Task<BindingList<Book>> GetBookByCategory(string _nameCategory)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var books = await Task.Run(() =>
                {
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
                            System.Threading.Thread.Sleep(1000);
                            return _bookList;
                        }
                    }
                });
                connection.Close();
                return books;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async Task<BindingList<Book>> GetDetailOrder(int Id)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var _books = await Task.Run(() =>
                {
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"select bk.Image,bk.ID, ordt.Quantity, bk.Title, bk.Price*ordt.Quantity as Price\r\nfrom OrderDetail ordt join book bk on ordt.Book= bk.ID\r\nwhere ordt.[Order]='{Id}'";
                    using (var command = new SqlCommand(query, connection))
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
                connection.Close();
                return _books;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async void DeleteOrder(ElementOrder _order)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var _bookOrder = await Task.Run(() =>
                {
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"delete from OrderDetail where OrderDetail.[Order]='{_order.Id}'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return 1;
                });

                var _deleteOrder = await Task.Run(() =>
                {
                    // Thực hiện truy vấn SQL để lấy các dòng của bảng Shop với Price = 2
                    string query = $"delete from [Order] where [Order].ID='{_order.Id}'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return 1;
                });

                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async Task<BindingList<Book>> loadListProduct(int Id)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var _books = await Task.Run(() =>
                {
                    string query = $"select bk.Image,bk.ID, ordt.Quantity, bk.Title, bk.Price*ordt.Quantity as Price\r\nfrom OrderDetail ordt join book bk on ordt.Book= bk.ID\r\nwhere ordt.[Order]='{Id}'";
                    using (var command = new SqlCommand(query, connection))
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
                connection.Close();
                return _books;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async Task<BindingList<Book>> LoadListProductWithCategory(string _nameCategory)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var books = await Task.Run(() =>
                {
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
                            System.Threading.Thread.Sleep(1000);
                            return _bookList;
                        }
                    }
                });
                connection.Close();
                return books;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void deleteInOrderDetail(Book _book, int IDOrder)
        {
            string connectionString = DB.Instance.ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();

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

        public override Task<bool> ConnectDB(string userName, string password)
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
            return "order";
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
