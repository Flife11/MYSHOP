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
    class EditBook_GUI : IGUI
    {
        public BindingList<Category> _categories = new BindingList<Category>();
        public Book editBook;
        public EditBook_GUI() { }
        public EditBook_GUI(IBus bus, BindingList<Category> categories, Book book) 
        {
            _categories = categories;
            editBook = book;
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            throw new NotImplementedException();
        }

        public override UserControl GetMainWindow()
        {
            return new EditBook_USC(_bus, _categories, editBook);
        }

        public override string Name()
        {
            throw new NotImplementedException();
        }
    }
}
