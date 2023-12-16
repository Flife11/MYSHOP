using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enity
{
    public class DB
    {
        private static DB _instance = null;
        public bool Connect = false;
        private SqlConnection _connection = null;

        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(ConnectionString);
                    _connection.Open();
                }

                return _connection;
            }
        }      

        public bool isConnected() { return Connect; }
        public string ConnectionString { get; set; } = "";


        public static DB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DB();
                }

                return _instance;
            }
        }
    }
}
