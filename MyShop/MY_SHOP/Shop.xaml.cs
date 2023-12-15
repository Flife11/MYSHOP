using Enity;
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
using System.Windows.Shapes;
using ThreeLayerContract;

namespace MY_SHOP
{
    /// <summary>
    /// Interaction logic for Shop.xaml
    /// </summary>
    public partial class Shop : Window
    {
        IGUI _gui;
        public Shop(IGUI gui)
        {
            _gui = gui;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var control = _gui.GetMainWindow();

            Content.Children.Add(control);
            Content.Width = control.Width;
            Content.Height = control.Height;

            MainWindow.DataContext = control;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (DB.Instance.ConnectionString != null)
            {
                var main = new Main();
                this.Close();
                main.Show();
            } else
            {
                Application.Current.Shutdown();
            }
        }
    }
}
