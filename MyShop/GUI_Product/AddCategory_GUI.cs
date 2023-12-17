using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Product
{
    class AddCategory_GUI : IGUI
    {
        public AddCategory_GUI() { }
        public AddCategory_GUI(IBus bus) 
        {
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new AddCategory_GUI(bus);
        }

        public override UserControl GetMainWindow()
        {
            return new AddCategory_USC(_bus);
        }

        public override string Name()
        {
            return "addCategoryProduct";
        }
    }
}
