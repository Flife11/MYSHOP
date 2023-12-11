using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace BUS01_Login
{
    public class BUS01_Login : IBus
    {
        public BUS01_Login() { }    
        public BUS01_Login(IDAO dao) 
        {
            _dao = dao;
        }

        public override void ConnectDB(string userName, string password)
        {
            _dao.ConnectDB(userName, password);
        }        

        public override IBus CreateNew(IDAO dao)
        {
            return new BUS01_Login(dao);
        }

        public override Tuple<string, string> LoadServerFromConfig()
        {
            return _dao.LoadServerFromConfig();
        }

        public override Tuple<string, string> LoadUserFromConfig()
        {
            return _dao.LoadUserFromConfig();
        }

        public override void SaveServerToConfig(string server, string database)
        {
            _dao.SaveServerToConfig(server, database);
        }

        public override void SaveUserToConfig(string userName, string password)
        {
            _dao.SaveUserToConfig(userName, password);
        }

        public override string Name()
        {
            return "login";
        }
    }
}
