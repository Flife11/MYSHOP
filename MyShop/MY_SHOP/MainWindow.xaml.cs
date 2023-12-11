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
            var guis = new List<IGUI>();
            var buses = new List<IBus>();
            var daos = new List<IDAO>();

            // Get list of DLL files in main executable folder
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(exePath);
            FileInfo[] fis = new DirectoryInfo(folder).GetFiles("*.dll");

            // Load all assemblies from current working directory
            foreach (FileInfo fileInfo in fis)
            {
                var domain = AppDomain.CurrentDomain;
                Assembly assembly = domain.Load(AssemblyName.GetAssemblyName(fileInfo.FullName));

                // Get all of the types in the dll
                Type[] types = assembly.GetTypes();

                // Only create instance of concrete class that inherits from IGUI, IBus or IDao
                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        if (typeof(IGUI).IsAssignableFrom(type))
                            guis.Add(Activator.CreateInstance(type) as IGUI);
                        else if (typeof(IBus).IsAssignableFrom(type))
                            buses.Add(Activator.CreateInstance(type) as IBus);
                        else if (typeof(IDAO).IsAssignableFrom(type))
                            daos.Add(Activator.CreateInstance(type) as IDAO);
                    }
                }
            }

            IGUI gui = null;
            foreach (var g in guis)
            {
                if (g.Name()=="login")
                {
                    gui = g as IGUI;
                    break;
                }
            }
            IBus bus = null;
            foreach (var b in buses)
            {
                if (b.Name() == "login")
                {
                    bus = b as IBus;
                    break;
                }
            }
            IDAO dao = null;
            foreach (var d in daos)
            {
                if (d.Name() == "login")
                {
                    dao = d as IDAO;
                    break;
                }
            }

            bus = bus.CreateNew(dao);
            gui = gui.CreateNew(bus);

            this.Hide();

            var program = new Shop(gui);
            program.Show();
        }
    }
}
