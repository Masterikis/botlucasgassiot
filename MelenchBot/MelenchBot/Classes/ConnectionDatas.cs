using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ConnectionDatas(string theServerAddress, int thePort, string theUserName, string theNickName, string theChannel, string thePassword, int theMaxRetries = 3)
        {
            serverAddress = theServerAddress;
            port = thePort;
            userName = theUserName;
            nickName = theNickName;
            channel = theChannel;
            password = thePassword;
            maxRetries = theMaxRetries;
        }
    }
}
