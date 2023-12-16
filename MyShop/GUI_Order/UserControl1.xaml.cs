using ControlzEx.Standard;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        IBus _bus = null;
        Pages _page = new Pages()
        {
            _currentPage = 1,
            _pageSize = 2
        };
        private int _offset = 0;
        private bool isSearching = false;
        string _dateFrom;
        string _dateTo;
        public UserControl1(IBus bus)
        {
            _bus = bus;
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
        private async void SearchOrder(object sender, RoutedEventArgs e)
        {
            if (!dateFrom.SelectedDate.HasValue)
            {
                MessageBox.Show("Lỗi: Vui lòng nhập ngày bắt đầu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!dateTo.SelectedDate.HasValue)
            {
                MessageBox.Show("Lỗi: Vui lòng nhập ngày kết thúc!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _offset = 0;
            _dateFrom = dateFrom.SelectedDate.Value.ToString("MM-dd-yyyy");
            _dateTo = dateTo.SelectedDate.Value.ToString("MM-dd-yyyy");

            var data = await _bus.getListOrderBySearch(_dateFrom, _dateTo, _offset);
            ///----------
            _listOrder.Clear();
            foreach (var order in data.Item1)
            {
                _listOrder.Add(order);
            }
            _page._pageSize = data.Item2;
            _page._currentPage = 1;
            if (_page._currentPage == 1)
            {
                previousPage.IsEnabled = false;
            }
            else
            {
                previousPage.IsEnabled = true;
            }
            if (_page._currentPage == _page._pageSize)
            {
                nextPage.IsEnabled = false;
            }
            else
            {
                nextPage.IsEnabled = true;

            }
            listOrder.ItemsSource = _listOrder;
            DataContext = _page;
            //---------

            isSearching = true;
        }                
        private async void OrderWindow_Loaded(object sender, RoutedEventArgs e)
        {            
            var data = _bus.getListOrder(_offset);
            listOrder.ItemsSource = _listOrder;
            DataContext = _page;
        }        

        private async void PreviousPageClick(object sender, RoutedEventArgs e)
        {
            if (_page._currentPage > 1)
            {
                _page._currentPage--;
                _offset--;
                if (isSearching)
                {
                    var _orders = await _bus.getListOrderBySearchPage(_dateFrom, _dateTo, _offset);
                    _listOrder.Clear();
                    foreach (var order in _orders)
                    {
                        _listOrder.Add(order);
                    }
                    listOrder.ItemsSource = _listOrder;
                }
                else
                {
                    var _orders = await _bus.getListOrderPage(_offset);
                    _listOrder.Clear();
                    foreach (var order in _orders)
                    {
                        _listOrder.Add(order);
                    }
                    listOrder.ItemsSource = _listOrder;
                }
                if (_page._currentPage == 1)
                {
                    previousPage.IsEnabled = false;
                }
                nextPage.IsEnabled = true;
            }

        }

        private async void NextPageClick(object sender, RoutedEventArgs e)
        {

            if (_page._currentPage < _page._pageSize)
            {
                _page._currentPage++;
                _offset++;
                if (isSearching)
                {
                    var _orders = await _bus.getListOrderBySearchPage(_dateFrom, _dateTo, _offset);
                    _listOrder.Clear();
                    foreach (var order in _orders)
                    {
                        _listOrder.Add(order);
                    }
                    listOrder.ItemsSource = _listOrder;
                }
                else
                {                    
                    var _orders = await _bus.getListOrderPage(_offset);
                    _listOrder.Clear();
                    foreach (var order in _orders)
                    {
                        _listOrder.Add(order);
                    }
                    listOrder.ItemsSource = _listOrder;
                }
                if (_page._currentPage == _page._pageSize)
                {
                    nextPage.IsEnabled = false;
                }
                previousPage.IsEnabled = true;

            }

        }                
        public static int index = 0;
        private void ViewDetailOrder(object sender, RoutedEventArgs e)
        {
            if (listOrder.SelectedItem != null)
            {
                ElementOrder _order = (ElementOrder)listOrder.SelectedItem;
                OrderInformationWindow _detailOrder = new OrderInformationWindow(_order);
                index = (int)listOrder.SelectedIndex;
                bool? _bool = _detailOrder.ShowDialog();
                if (_bool != null)
                {

                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("Lỗi: Vui lòng chọn Order", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
