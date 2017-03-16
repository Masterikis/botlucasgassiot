using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot.DataBase
{
    /// <summary>
    /// Classe abstraite pour tout communication avec la base de données.
    /// Chaque communication avec une table aura sa propre classe héritante de celle-ci.
    /// </summary>
    public abstract class DBCommunication
    {

        protected DBConnection dbConnection;

        public DBCommunication()
        {
            dbConnection = DBConnection.Instance();
        }

        /// <summary>
        /// Permet d'exécuter une requete.
        /// </summary>
        /// <param name="theQuery"></param>
        /// <returns></returns>
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
