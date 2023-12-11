using Enity;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public async override void ConnectDB(string userName, string password)
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
                DB.Instance.Connect = true;
            } else { DB.Instance.Connect = false; }
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
    }
}
