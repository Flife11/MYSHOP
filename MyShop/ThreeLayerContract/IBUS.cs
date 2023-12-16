using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeLayerContract
{
    public abstract class IBus
    {
        protected IDAO _dao;
        //------------Login----------//
        public abstract IBus CreateNew(IDAO dao);
        public abstract void ConnectDB(string userName, string password);
        public abstract void SaveUserToConfig(string userName, string password);
        public abstract void SaveServerToConfig(string server, string database);
        public abstract Tuple<string, string> LoadUserFromConfig();
        public abstract Tuple<string, string> LoadServerFromConfig();
        //------------Order----------//
        public abstract Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset);
        public abstract Task<List<ElementOrder>> getListOrderPage(int _offset);
        public abstract Task<Tuple<List<ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset);
        public abstract Task<List<ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset);
        public abstract string Name();
    }
}
