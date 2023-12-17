using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Product
{
    internal class AddBook_GUI : IGUI
    {
        BindingList<Category> _categories = new BindingList<Category>();
        public AddBook_GUI() { }
        public AddBook_GUI(IBus bus, BindingList<Category> categories)
        {            
            _categories = categories;
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new AddBook_GUI(bus, _categories);
        }

        public override UserControl GetMainWindow()
        {
            return new AddBook_USC(_bus, _categories);
        }

        public override string Name()
        {
            return "addBookProduct";
        }
    }
}
