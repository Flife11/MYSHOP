using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Order
{
    class AddOrder_GUI : IGUI
    {
        public AddOrder_GUI() { }
        public AddOrder_GUI(IBus bus) 
        {
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new AddOrder_GUI(bus);
        }

        public override UserControl GetMainWindow()
        {
            return new AddOrder_USC(_bus);
        }

        public override string Name()
        {
            return "addOrder";
        }
    }
}
