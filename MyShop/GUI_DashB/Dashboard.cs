using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_DashB
{
    class Dashboard : IGUI
    {
        public Dashboard() { }
        public Dashboard(IBus bus)
        {
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            throw new NotImplementedException();
        }

        public override UserControl GetMainWindow()
        {
            throw new NotImplementedException();
        }

        public override string Name()
        {
            return "dashb";
        }
    }
}
