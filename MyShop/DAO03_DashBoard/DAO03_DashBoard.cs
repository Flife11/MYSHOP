using Entity;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace DAO03_DashBoard
{
    public class DAO03_DashBoard : IDAO
    {
        public override Tuple<List<string>, List<int>> BooksAndQuantity(DateTime? _beginDate, DateTime? _endDate)
        {
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;
            command.Parameters.Add("@beginDate", System.Data.SqlDbType.DateTime);
            command.Parameters.Add("@endDate", System.Data.SqlDbType.DateTime);
            command.Parameters["@beginDate"].Value = _beginDate.Value.ToShortDateString();
            command.Parameters["@endDate"].Value = _endDate.Value.ToShortDateString();
            command.CommandText = "select Category, count(*) as quantity\r\nfrom book join OrderDetail on book.ID = OrderDetail.Book join [Order] on OrderDetail.[Order] = [Order].ID\r\nwhere [Date] > @beginDate and [Date] < @endDate\r\ngroup by Category";
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                List<string> stringValues = new List<string>();
                List<int> intValues = new List<int>();
                while (reader.Read())
                {
                    string key = reader.GetValue(0).ToString();
                    int value = reader.GetInt32(1);
                    stringValues.Add(key);
                    intValues.Add(value);
                }
                reader.Close();
                return Tuple.Create(stringValues, intValues);
            }
            reader.Close();
            return Tuple.Create(new List<string>(), new List<int>());
        }
        public override Tuple<List<string>, List<float>> ChartInfor(DateTime? _beginDate, DateTime? _endDate, TimeSpan? dateDiff)
        {
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;
            command.Parameters.Add("@beginDate", System.Data.SqlDbType.DateTime);
            command.Parameters.Add("@endDate", System.Data.SqlDbType.DateTime);
            command.Parameters["@beginDate"].Value = _beginDate.Value.ToShortDateString();
            command.Parameters["@endDate"].Value = _endDate.Value.ToShortDateString();

            if (dateDiff.Value.Days >= 40 && dateDiff.Value.Days < 120 && _beginDate.Value.Year == _endDate.Value.Year)
            {
                command.CommandText = "select DATEPART(WEEK,[Date]),sum([Price]) as total from [Order], [OrderDetail], [book]\r\nwhere [Order].[ID]= [OrderDetail].[Order] and [book].[ID]=[OrderDetail].[Book] and [Order].[Date] > @beginDate and [Date] < @endDate \r\ngroup by DATEPART(WEEK,[Date])";                
            }
            else if (dateDiff.Value.Days < 40)
            {
                command.CommandText = "select FORMAT([Date],'dd-MM-yyyy'),sum([Price]) as total from [Order], [OrderDetail], [book]\r\nwhere [Order].[ID]= [OrderDetail].[Order] and [book].[ID]=[OrderDetail].[Book] and [Order].[Date] > @beginDate and [Date] < @endDate \r\ngroup by FORMAT([Date],'dd-MM-yyyy')";                
            }
            else if (dateDiff.Value.Days < 1000)
            {
                command.CommandText = "SELECT\r\n    FORMAT([Order].[Date], 'yyyy-MM') AS YearMonth,\r\n    SUM([book].[Price]) AS Total\r\n" +
                    "FROM\r\n    [Order]\r\n    JOIN [OrderDetail] ON [Order].[ID] = [OrderDetail].[Order]\r\n    JOIN [book] ON [book].[ID] = [OrderDetail].[Book]\r\n" +
                    "WHERE\r\n    [Order].[Date] > @beginDate AND [Order].[Date] < @endDate\r\nGROUP BY\r\n    FORMAT([Order].[Date], 'yyyy-MM')\r\nORDER BY\r\n    FORMAT([Order].[Date], 'yyyy-MM');";                
            }

            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                List<string> stringValues = new List<string>();
                List<float> floatValues = new List<float>();
                while (reader.Read())
                {
                    string key = reader.GetValue(0).ToString();
                    float value = (float)reader.GetDouble(1);
                    stringValues.Add(key);
                    floatValues.Add(value);
                }
                reader.Close();
                return Tuple.Create(stringValues, floatValues);

            }
            reader.Close();
            return Tuple.Create(new List<string>(), new List<float>());
        }
        public override List<String> LoadDashInfor(DateTime curDate, DateTime beginMonth_Date, DateTime previousMonday)
        {
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;

            command.Parameters.AddWithValue("@curDate", curDate);
            command.Parameters.AddWithValue("@beginMonth", beginMonth_Date);
            command.Parameters.AddWithValue("@previousMonday", previousMonday);

            //Số sản phẩm đang bán
            command.CommandText = "select count(ID) from book";
            var rowsCount = command.ExecuteScalar();

            //Tổng đơn hàng trong tuần
            command.CommandText = "select count(*) from[Order] where Date between @previousMonday and @curDate";
            var weekCount = command.ExecuteScalar();

            //Tổng đơn hàng trong tháng
            command.CommandText = "select count(*) from[Order] where Date between @beginMonth and @curDate";
            var monthCount = command.ExecuteScalar();

            return new List<String>() { rowsCount.ToString(), weekCount.ToString(), monthCount.ToString() };
        }
        public override BindingList<Book> SoldingOutPr_Lv()
        {
            BindingList<Book> books = new BindingList<Book>();
            var command = new SqlCommand();
            command.Connection = DB.Instance.Connection;
            command.CommandText = " select * from book where Availability < 5 and Availability >0 order by Availability";
            var reader = command.ExecuteReader();

            var _count = 0;
            while (reader.Read() && _count < 5)
            {
                _count++;
                string _image = (string)reader["Image"];
                string _title = (string)reader["Title"];
                string _category = (string)reader["Category"];
                int _availability = (int)reader["Availability"];
                var book = new Book() { ImageUrl = _image, Title = _title, Category = _category, Availability = _availability };
                books.Add(book);
            }
            reader.Close();
            return books;
        }

        public override void AddBook(string title, string price, string description, string category, string image, string availability)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> ConnectDB(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void DeleteBook(int Id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCategory(string Name, int Id)
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

        public override void EditBook(Book editBook, int id)
        {
            throw new NotImplementedException();
        }

        public override void EditCategory(Category editCat, int ID, string Name)
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

        public override void insertCategoryAndBook(BindingList<Category> categories, BindingList<Book> books)
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
            return "dashb";
        }

        public override BindingList<Book> readBookFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Category> readCategoryFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override bool SaveCategory(string CategoryName)
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

        public override Tuple<BindingList<Book>, int> searchBook(string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Category> selectAllCategory()
        {
            throw new NotImplementedException();
        }

        public override Tuple<BindingList<Book>, int> selectBookByCategory(string name, string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }
    }
}
