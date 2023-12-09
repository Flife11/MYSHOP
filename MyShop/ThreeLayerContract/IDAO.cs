using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeLayerContract
{
    public abstract class IDAO
    {
        public abstract void Save();
        public abstract void  ConnectDB(string userName, string password);
    }
}
