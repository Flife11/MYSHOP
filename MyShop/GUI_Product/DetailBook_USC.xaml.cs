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

namespace GUI_Product
{
    /// <summary>
    /// Interaction logic for DetailBook_USC.xaml
    /// </summary>
    public partial class DetailBook_USC : UserControl
    {
        Book detailBook = new Book();
        IBus _bus;
        public DetailBook_USC(IBus bus, Book book)
        {
            _bus = bus;
            detailBook = book;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = detailBook;
        }
    }
}
