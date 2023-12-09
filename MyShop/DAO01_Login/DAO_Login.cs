using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace DAO01_Login
{
    public class DAO_Login : IDAO
    {
        public override async void ConnectDB(string userName, string password)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".";
            builder.InitialCatalog = "MyShop";
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
         
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }
}
