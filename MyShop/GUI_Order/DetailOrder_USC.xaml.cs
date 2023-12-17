using Models;
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

namespace GUI_Order
{
    /// <summary>
    /// Interaction logic for DetailOrder_USC.xaml
    /// </summary>
    public partial class DetailOrder_USC : UserControl
    {
        IBus _bus;
        ElementOrder _order = new ElementOrder();
        public DetailOrder_USC(IBus bus, ElementOrder order)
        {
            _bus = bus;
            _order = order;
            InitializeComponent();
        }        

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
        double _totalPrice = 0;
        private async void DetailOrderLoaded(object sender, RoutedEventArgs e)
        {
            var _books = await _bus.GetDetailOrder(_order.Id);
            listBook.ItemsSource = _books;
            foreach (var book in _books)
            {
                _totalPrice = _totalPrice + book.Price;
            }
            totalPrice.Text = _totalPrice.ToString();

        }

        private async void DeleteOrder(object sender, RoutedEventArgs e)
        {
            _bus.DeleteOrder(_order);
            UserControl1._listOrder.Remove(_order);
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();            
        }

        private void EditOrderClick(object sender, RoutedEventArgs e)
        {
            var _editWindow = new EditOrder_GUI(_bus, _order);
            var program = new OrderPopup(_editWindow);            
            bool? _bool = program.ShowDialog();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            if (_bool != null)
            {

            }
            else
            {

            }
        }
    }
}
