using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace BUS02_Order
{
    public class BUS02_Order : IBus
    {
        public BUS02_Order() { }
        public BUS02_Order(IDAO dao)
        {
            _dao = dao;
        }
        public override async Task<Tuple<List<ElementOrder>, int>> getListOrder(int _offset)
        {
            var data = await _dao.getListOrder(_offset);
            return data;

        }
        public override async Task<List<ElementOrder>> getListOrderPage(int _offset)
        {
            var data = await _dao.getListOrderPage(_offset);
            return data;
        }
        public override async Task<Tuple<List<ElementOrder>, int>> getListOrderBySearch(string dateFrom, string dateTo, int _offset)
        {
            var data = await _dao.getListOrderBySearch(dateFrom, dateTo, _offset);
            return data;
        }
        public override async Task<List<ElementOrder>> getListOrderBySearchPage(string dateFrom, string dateTo, int _offset)
        {
            var data = await _dao.getListOrderBySearchPage(dateFrom, dateTo, _offset);
            return data;
        }
        public override string Name()
        {
            return "Order";
        }
        public override void ConnectDB(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override IBus CreateNew(IDAO dao)
        {
            return new BUS02_Order(dao);
        }

        public override Tuple<string, string> LoadServerFromConfig()
        {
            throw new NotImplementedException();
        }

        public override Tuple<string, string> LoadUserFromConfig()
        {
            throw new NotImplementedException();
        }        

        public override void SaveServerToConfig(string server, string database)
        {
            throw new NotImplementedException();
        }

        public override void SaveUserToConfig(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
