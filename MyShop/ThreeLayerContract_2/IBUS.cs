using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeLayerContract
{
    public abstract class IBus
    {
        protected IDAO _dao;
        public abstract IBus CreateNew(IDAO dao);
        public abstract void ConnectDB(string userName, string password);
        public abstract void SaveUserToConfig(string userName, string password);
        public abstract void SaveServerToConfig(string server, string database);
        public abstract Tuple<string, string> LoadUserFromConfig();
        public abstract Tuple<string, string> LoadServerFromConfig();
        public abstract string Name();
    }
}
