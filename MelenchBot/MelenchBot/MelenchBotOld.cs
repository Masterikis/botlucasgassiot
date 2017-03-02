using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot
{
    public class MelenchBotOld
    {

        private string userName;
        private string channel;

        public TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        public MelenchBotOld(string theIp, int thePort, string theUserName, string thePassword)
        {
            tcpClient = new TcpClient(theIp, thePort);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());
            userName = theUserName;
            outputStream.WriteLine("PASS " + thePassword);
            outputStream.WriteLine("NICK " + userName);
            outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
            outputStream.WriteLine("CAP REQ :twitch.tv/membership");
            outputStream.WriteLine("CAP REQ :twitch.tv/commands");
            outputStream.Flush();
        }

        public void jointRoom(string theChannel)
        {
            channel = theChannel;
            outputStream.WriteLine("JOIN #" + channel);
            outputStream.Flush();
            sendChatMessage("MelenchBot !!!");
        }

        public void sendIrcMessage(string theMessage)
        {
            outputStream.WriteLine(theMessage);
            outputStream.Flush();
        }

        public void sendChatMessage(string theMessage)
        {
            sendIrcMessage(":" + userName + "!" + userName + "@" + userName
                + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + theMessage);
        }

        public string readMessage()
        {
            string message = inputStream.ReadLine();
            return message;
        }
    }
}
