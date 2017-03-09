using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace MelenchBot.DataBase
{
    /// <summary>
    /// Basé sur : http://stackoverflow.com/questions/21618015/how-to-connect-to-mysql-database
    /// </summary>
    public class DBConnection
    {
        private DBConnection() { }

        private MySqlConnection connection = null;

        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection instance = null;

        public static DBConnection Instance()
        {
            if (instance == null) instance = new DBConnection();
            return instance;
        }

        public bool IsConnect()
        {
            if(Connection == null)
            {
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", ConfigurationManager.AppSettings["dbAddress"],  ConfigurationManager.AppSettings["dbName"], ConfigurationManager.AppSettings["dbUser"], ConfigurationManager.AppSettings["dbPwd"]);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }
            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
