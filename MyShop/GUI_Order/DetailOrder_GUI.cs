using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Order
{
    class DetailOrder_GUI : IGUI
    {
        public ElementOrder _order;
        public DetailOrder_GUI() { }
        public DetailOrder_GUI(IBus bus, ElementOrder order)
        {
            _order = order;
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new DetailOrder_GUI(bus, _order);
        }

        public override UserControl GetMainWindow()
        {
            return new DetailOrder_USC(_bus, _order);
        }

        public override string Name()
        {
            return "orderDetail";
        }
    }
}
