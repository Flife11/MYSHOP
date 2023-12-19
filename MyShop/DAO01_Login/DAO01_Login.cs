using Entity;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace DAO01_Login
{
    public class DAO01_Login : IDAO
    {
        public async override Task<bool> ConnectDB(string userName, string password)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ConfigurationManager.AppSettings["Server"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["Database"];
            builder.UserID = userName;
            builder.Password = password;
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

                return _connection;
            });

            if (connection != null )
            {
                DB.Instance.ConnectionString = connectionString;
                connection.Close();
                return true;
            } 
            return false;
        }

        public override Tuple<string, string> LoadServerFromConfig()
        {
            string server = ConfigurationManager.AppSettings["Server"];
            string database = ConfigurationManager.AppSettings["Database"];
            return Tuple.Create(server, database);
        }

        public override Tuple<string, string> LoadUserFromConfig()
        {
            var passwordIn64 = ConfigurationManager.AppSettings["Password"];
            string password = "";

            if (passwordIn64.Length != 0)
            {
                var entropyIn64 = ConfigurationManager.AppSettings["Entropy"];

                var cyperTextInBytes = Convert.FromBase64String(passwordIn64);
                var entropyInBytes = Convert.FromBase64String(entropyIn64);

                var passwordInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes,
                    DataProtectionScope.CurrentUser);
                password = Encoding.UTF8.GetString(passwordInBytes);

                
            }
            return Tuple.Create(ConfigurationManager.AppSettings["Username"], password);
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void SaveServerToConfig(string server, string database)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Server"].Value = server;
            config.AppSettings.Settings["Database"].Value = database;
            config.Save(ConfigurationSaveMode.Minimal);
        }

        public override void SaveUserToConfig(string userName, string password)
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var cypherText = ProtectedData.Protect(passwordInBytes, entropy,
                DataProtectionScope.CurrentUser);
            var passwordIn64 = Convert.ToBase64String(cypherText);
            var entropyIn64 = Convert.ToBase64String(entropy);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Username"].Value = userName;
            config.AppSettings.Settings["Password"].Value = passwordIn64;
            config.AppSettings.Settings["Entropy"].Value = entropyIn64;
            config.Save(ConfigurationSaveMode.Minimal);

            ConfigurationManager.RefreshSection("appSettings");
        }

        public override string Name()
        {
            return "login";
        }

        public override Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset)
        {
            throw new NotImplementedException();
        }

        public override Task<List<ElementOrder>> getListOrderPage(int _offset)
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

        public override Task<List<Category>> getListCateGory()
        {
            throw new NotImplementedException();
        }

        public override void insertOrderDetail(Book _book, int IDOrder)
        {
            throw new NotImplementedException();
        }

        public override Task<int> insertOrder(string _date, ElementOrder _order)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> GetBookByCategory(string _nameCategory)
        {
            throw new NotImplementedException();
        }

        public override int GetNextId(string tableName, string idColumnName)
        {
            throw new NotImplementedException();
        }

        public override Task<BindingList<Book>> GetDetailOrder(int Id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteOrder(ElementOrder _order)
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

        public override void deleteInOrderDetail(Book _book, int IDOrder)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Category> selectAllCategory()
        {
            throw new NotImplementedException();
        }

        public override BindingList<Category> readCategoryFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Book> readBookFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void insertCategoryAndBook(BindingList<Category> categories, BindingList<Book> books)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BindingList<Book>, int> searchBook(string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BindingList<Book>, int> selectBookByCategory(string name, string _sortBy, string _sortOption, string _searchText, int _currentPage, int _rowsPerPage, int _minPrice, int _maxPrice)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCategory(string Name, int Id)
        {
            throw new NotImplementedException();
        }

        public override bool SaveCategory(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public override void AddBook(string title, string price, string description, string category, string image, string availability)
        {
            throw new NotImplementedException();
        }

        public override void EditBook(Book editBook, int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteBook(int Id)
        {
            throw new NotImplementedException();
        }

        public override void EditCategory(Category editCat, int ID, string Name)
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

        public override Tuple<List<string>, List<int>> BooksAndQuantity()
        {
            throw new NotImplementedException();
        }
    }
}
