using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace MelenchBot
{
    public class ConnectionDatas
    {
        public string serverAddress { get; }
        public int port { get; }
        public string userName { get; }
        public string nickName { get; }
        public string channel { get; }
        public string password { get; }
        public int maxRetries { get; }

        public ConnectionDatas(int theMaxRetries = 3)
        {
            serverAddress = ConfigurationManager.AppSettings["serverAddress"];
            port = int.Parse(ConfigurationManager.AppSettings["serverPort"]);
            userName = ConfigurationManager.AppSettings["userName"];
            nickName = ConfigurationManager.AppSettings["nickName"];
            channel = ConfigurationManager.AppSettings["channel"];
            password = ConfigurationManager.AppSettings["password"];
            maxRetries = theMaxRetries;
        }
    }
}
