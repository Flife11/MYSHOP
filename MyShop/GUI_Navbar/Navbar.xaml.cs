using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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

namespace GUI_Navbar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Navbar : Window
    {
        public Navbar()
        {
            InitializeComponent();
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Login_CLick(object sender, RoutedEventArgs e)
        {
            string userName = Username.Text;
            string password = Password.Password;


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
                    MessageBox.Show(ex.Message);
                    _connection = null;
                }

                return _connection;
            });
            

            if (connection != null)
            {
                
                if (rememberMe.IsChecked == true)
                {
                    MessageBox.Show(
                   $"Success"
               );
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

                DB.Instance.ConnectionString = connectionString;

                //var screen = new MainWindow();
                //screen.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    $"Cannot connect"
                );
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var passwordIn64 = ConfigurationManager.AppSettings["Password"];

            if (passwordIn64.Length != 0)
            {
                var entropyIn64 = ConfigurationManager.AppSettings["Entropy"];

                var cyperTextInBytes = Convert.FromBase64String(passwordIn64);
                var entropyInBytes = Convert.FromBase64String(entropyIn64);

                var passwordInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes,
                    DataProtectionScope.CurrentUser);
                var password = Encoding.UTF8.GetString(passwordInBytes);
                Password.Password = password;

                Username.Text = ConfigurationManager.AppSettings["Username"];
            }
        }
    }
}
