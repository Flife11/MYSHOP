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
    }
}
