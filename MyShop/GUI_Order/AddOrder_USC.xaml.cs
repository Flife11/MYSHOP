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
    /// Interaction logic for AddOrder_USC.xaml
    /// </summary>
    public partial class AddOrder_USC : UserControl
    {
        IBus _bus;
        public AddOrder_USC(IBus bus)
        {
            _bus = bus;
            InitializeComponent();
        }
    }
}
