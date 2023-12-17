using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Product
{
    class DetailBook_GUI : IGUI
    {
        Book detailBook = new Book();
        IBus _bus;
        public DetailBook_GUI() { }
        public DetailBook_GUI(IBus bus, Book detailBook)
        {
            this.detailBook = detailBook;
            _bus = bus;
        }

        public override IGUI CreateNew(IBus bus)
        {
            throw new NotImplementedException();
        }

        public override UserControl GetMainWindow()
        {
            return new DetailBook_USC(_bus, detailBook);
        }

        public override string Name()
        {
            throw new NotImplementedException();
        }
    }
}
