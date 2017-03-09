using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot.DataBase
{
    public abstract class DBCommunication
    {

        protected DBConnection dbConnection;

        public DBCommunication()
        {
            dbConnection = DBConnection.Instance();
        }

        protected MySqlDataReader executeQuery(string theQuery)
        {
            if (dbConnection.IsConnect())
            {
                var cmd = new MySqlCommand(theQuery, dbConnection.Connection);
                return cmd.ExecuteReader();
            }
            return null;
        }

    }
}
