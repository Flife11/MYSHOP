using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for configServer.xaml
    /// </summary>
    public partial class configServer : UserControl
    {
        IBus _bus;
        public configServer(IBus bus)
        {
            _bus = bus;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tuple<string, string> res = _bus.LoadServerFromConfig();
            server.Text = res.Item1;
            database.Text = res.Item2;
        }

        private void Save_CLick(object sender, RoutedEventArgs e)
        {
            _bus.SaveServerToConfig(server.Text, database.Text);
            Window parent = Window.GetWindow(this);
            parent.Close();
        }
    }
}
