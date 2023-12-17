using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace ThreeLayerContract_2
{
    public abstract class IBus_Login : IBUS
    {                
        public abstract void ConnectDB(string userName, string password);
        public abstract void SaveUserToConfig(string userName, string password);
        public abstract void SaveServerToConfig(string server, string database);
        public abstract Tuple<string, string> LoadUserFromConfig();
        public abstract Tuple<string, string> LoadServerFromConfig();        
    }
}
