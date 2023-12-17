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
    internal class EditOrder_GUI : IGUI
    {
        public ElementOrder _order;
        public EditOrder_GUI() { }
        public EditOrder_GUI(IBus bus, ElementOrder order)
        {
            _order = order;
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new EditOrder_GUI(bus, _order);
        }

        public override UserControl GetMainWindow()
        {
            return new EditOrder_USC(_bus, _order);
        }

        public override string Name()
        {
            return "orderEdit";
        }
    }
}
