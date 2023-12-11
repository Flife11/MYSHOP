using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Login
{
    class Config : IGUI
    {
        public Config() { }
        public Config(IBus bus)
        {
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new Config(bus);
        }

        public override UserControl GetMainWindow()
        {
            return new configServer(_bus);
        }

        public override string Name()
        {
            return "config";
        }
    }
}
