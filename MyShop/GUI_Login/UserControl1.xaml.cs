using Entity;
using MaterialDesignThemes.Wpf;
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

        private async void Login_CLick(object sender, RoutedEventArgs e)
        {            
            string userName = Username.Text;
            string password = Password.Password;
            bool? remember = rememberMe.IsChecked;

            bool connected = await _bus.ConnectDB(userName, password);

            if (connected==true)
            {                
                if (remember==true)
                {
                    _bus.SaveUserToConfig(userName, password);
                }
                Window parentWindow = Window.GetWindow(this);
                parentWindow.Close();
            } else
            {

            }
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            var config_GUI = new Config(_bus);
            var program = new config(config_GUI);
            program.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tuple<string, string> res = _bus.LoadUserFromConfig();
            Username.Text = res.Item1;
            Password.Password = res.Item2;
        }
    }
}
