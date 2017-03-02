using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;


namespace MelenchBot
{
    class Program
    {
        static void Main(string[] args)
        {
            //MelenchBot mlbot = new MelenchBot("irc.twitch.tv", 6667,
            //    "melenchbot", "oauth:c35iurnatd0w8m4f4c8v6mkdqvi0p1");
            //mlbot.jointRoom("masterikis");

            //while(true)
            //{

            //}

            MelenchBot mlbot = new MelenchBot(
                theServerAddress: "irc.chat.twitch.tv",
                thePort: 6667,
                theUserName: "MelenchBot",
                theNickName: "Melench-Bot",
                theChannel: "#masterikis",
                thePassword: "oauth:c35iurnatd0w8m4f4c8v6mkdqvi0p1"
            );

            Console.WriteLine("Avant connexion.");
            mlbot.connection();
            mlbot.joinChannel();

            while (true)
            {
                mlbot.inspectTheChat();
            }
        }
    }
}
