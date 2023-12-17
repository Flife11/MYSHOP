using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Order
{
    class Order : IGUI
    {
        public Order() { }
        public Order(IBus bus) 
        {
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new Order(bus);
        }

        public override UserControl GetMainWindow()
        {
            return new UserControl1(_bus);
        }

        public override string Name()
        {
            return "order";
        }
    }
}
