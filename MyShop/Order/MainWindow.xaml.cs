using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Order
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
        public static BindingList<ElementOrder> _listOrder = new BindingList<ElementOrder>();
        private void addOrderButton(object sender, RoutedEventArgs e)
        {
            AddOrderWindow _addOrder = new AddOrderWindow();
            bool? _bool = _addOrder.ShowDialog();
            if (_bool != null)
            {

            }
            else
            {

            }

        }

        private void SearchOrder(object sender, RoutedEventArgs e)
        {

        }

        private void OrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listOrder.ItemsSource = _listOrder;

        }
    }
}