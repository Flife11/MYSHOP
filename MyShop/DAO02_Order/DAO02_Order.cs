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
            connection.Open();
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
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        public override void ConnectDB(string userName, string password)
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
            throw new NotImplementedException();
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
