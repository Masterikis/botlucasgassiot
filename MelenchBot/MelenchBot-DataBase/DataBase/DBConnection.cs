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

        /// <summary>
        /// Permet de récupérer l'instance de DBConnection.
        /// Impossible d'initialiser une classe DBConnection sans passer par ici, cela permet que le DBConnection soit instancié qu'une seule fois.
        /// Si l'instance n'existe pas, celle si crée donc une nouvelle instance de DBConnection.
        /// </summary>
        /// <returns></returns>
        public static DBConnection Instance()
        {
            if (instance == null) instance = new DBConnection();
            return instance;
        }

        /// <summary>
        /// Permet la connexion à la base de données si celle la n'est pas connectée.
        /// Renvois la connexion.
        /// </summary>
        /// <returns></returns>
        public bool IsConnect()
        {
            if(Connection == null)
            {
                try { 
                    string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", ConfigurationManager.AppSettings["dbAddress"],  ConfigurationManager.AppSettings["dbName"], ConfigurationManager.AppSettings["dbUser"], ConfigurationManager.AppSettings["dbPwd"]);
                    connection = new MySqlConnection(connstring);
                    connection.Open();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Erreur lors de la connexion à la base de données : " + e.Message);
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            return true;
        }

        /// <summary>
        /// Permet de fermer la connexion à la base de données.
        /// </summary>
        public void Close()
        {
            connection.Close();
        }
    }
}
