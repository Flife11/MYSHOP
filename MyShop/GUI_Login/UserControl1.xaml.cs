using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
using ThreeLayerContract;

namespace GUI_Login
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        IBus _bus;
        public UserControl1(IBus bus)
        {
            _bus = bus;
            InitializeComponent();
        }

        private void Login_CLick(object sender, RoutedEventArgs e)
        {
            string userName = Username.Text;
            string password = Password.Password;


            _bus.ConnectDB(userName, password);
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
