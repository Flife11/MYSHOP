using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThreeLayerContract;

namespace GUI_Login
{
    class Login : IGUI
    {
        public Login() { }
        public Login(IBus bus) 
        { 
            _bus = bus;
        }
        public override IGUI CreateNew(IBus bus)
        {
            return new Login(bus);
        }

        public override UserControl GetMainWindow()
        {
            return new UserControl1(_bus);
        }

        public override string Name()
        {
            return "login";
        }
    }
}
