using Enity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace MY_SHOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dllReader = new DLLReader();

            var dao = dllReader.GetDao("login");
            var bus = dllReader.GetBus("login");
            var gui = dllReader.GetGUI("login");
            bus = bus.CreateNew(dao);
            gui = gui.CreateNew(bus);

            this.Hide();

            var program = new Shop(gui);
            program.Show();

        }
    }
}
