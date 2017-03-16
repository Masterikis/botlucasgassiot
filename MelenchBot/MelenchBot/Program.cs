using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using MelenchBot.DataBase;
using MelenchBot_DataBase.DataBase;

namespace MelenchBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MelenchBot mlbot = new MelenchBot();
            mlbot.onMessageReceveid += Mlbot_onMessageReceveid;
            mlbot.connect();
            Console.ReadKey();
            mlbot.stop();
        }

        /// <summary>
        /// Permet d'écrire les messages reçus dans la console.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Mlbot_onMessageReceveid(object sender, MessageReceived e)
        {
            Console.WriteLine(e.originalMessage);
        }
    }
}
