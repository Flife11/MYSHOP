using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Product
{
    internal class Product : IGUI
    {        
        public Product() { }
        public Product(IBus bus) 
        {
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new Product(bus);
        }

        public override UserControl GetMainWindow()
        {
            return new UserControl1(_bus);
        }

        public override string Name()
        {
            return "product";
        }
    }
}
