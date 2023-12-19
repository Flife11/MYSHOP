using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using ThreeLayerContract;

namespace GUI_Product
{
    class EditCategory_GUI : IGUI
    {
        Category _category = new Category();
        IBus _bus;
        public EditCategory_GUI() { }
        public EditCategory_GUI(IBus bus, Category category)
        {
            _bus = bus;
            _category = category;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new EditCategory_GUI(bus, _category);
        }

        public override UserControl GetMainWindow()
        {
            return new EditCategory_USC(_bus, _category);
        }

        public override string Name()
        {
            return "editCategory";
        }
    }
}
