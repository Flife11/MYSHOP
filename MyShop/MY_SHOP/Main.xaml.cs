using Entity;
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
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        IGUI _gui;
        List<string> navbar = new List<string>() {"product", "order", "dashb" };
        public Main()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dllReader = new DLLReader();
            foreach (var nav in navbar)
            {
                var dao = dllReader.GetDao(nav);
                var bus = dllReader.GetBus(nav);
                var gui = dllReader.GetGUI(nav);
                bus = bus.CreateNew(dao);
                gui = gui.CreateNew(bus);

                var control = gui.GetMainWindow();
                var tmp = Dashboard;
                if (nav=="dashb") { tmp = Dashboard; }
                if (nav=="order") { tmp = Order; }
                if (nav=="product") { tmp = Product; }

                tmp.Children.Add(control);
                //tmp.Width = control.Width;
                //tmp.Height = control.Height;

                tmp.DataContext = control;
            }            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();            
        }
    }
}
