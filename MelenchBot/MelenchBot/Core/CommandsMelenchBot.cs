using MelenchBot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot
{
    /// <summary>
    /// Permet de gérer les commandes.
    /// </summary>
    public class CommandsMelenchBot
    {
        private MelenchBot melenchBot;
        private string[] getUserBans = { "!lsbanusers", "!lsbu" , "!allbanusers"};

        public CommandsMelenchBot(MelenchBot theMelenchBot)
        {
            melenchBot = theMelenchBot;
        }

        /// <summary>
        /// Permet d'analyser une commande et de lancer cette derniere si celle-ci existe
        /// </summary>
        /// <param name="theMsgReceived"></param>
        public void analyzeCommand(MessageReceived theMsgReceived)
        {
            string command = theMsgReceived.userMessage;
            if (theMsgReceived.itsMyCreator) analyzeCommandCreator(command);
            if(command.IndexOf(" ") == -1)
            {
                if (getUserBans.Any(s => command.ToLower().Contains(s))) lsUsersBans();
                if (command.ToLower().Contains("!melenchbot") && !theMsgReceived.itsMyCreator) melenchBot.writeMessage("Qu'est-ce que t'as toi ?");
            }
            else
            {
                if ((command.Contains("!unban") || command.Contains("!ban")) && !theMsgReceived.itsMyCreator) melenchBot.writeMessage(theMsgReceived.userName + " => Ya des baffes qui se perdent ...");
            }
        }

        /// <summary>
        /// Permet d'analyser une commande de l'admin et de lancer cette derniere si celle-ci existe
        /// </summary>
        /// <param name="theMsgReceived"></param>
        public void analyzeCommandCreator(string theCommand)
        {
            if (theCommand.Contains("!melenchbot")) melenchBot.writeMessage("Ils peuvent toujours nommer Donald à l'Europe; c’est quand même Picsou qui commande.");
                if (theCommand.Contains("!unban")) unBanUser(theCommand.Substring(theCommand.IndexOf(" ") + 1));
                if (theCommand.Contains("!ban")) banUser(theCommand.Substring(theCommand.IndexOf(" ") + 1));

        }

        /// <summary>
        /// Permet de debannir un utilisateur.
        /// </summary>
        /// <param name="theUserName"></param>
        private void unBanUser(string theUserName)
        {
            melenchBot.lsUsers.getUser(theUserName).unbanUser();
            melenchBot.unbanUser(theUserName);
            melenchBot.writeMessage(theUserName + " => C'est bon, reviens du goulag !");
        }

        /// <summary>
        /// Permet de bannir un utilisateur.
        /// </summary>
        /// <param name="theUserName"></param>
        private void banUser(string theUserName)
        {
            melenchBot.writeMessage(theUserName + " ==> Au goulag le capitaliste !");
            melenchBot.banUser(theUserName);
            User user = melenchBot.lsUsers.getUser(theUserName);
            user.reinitWarnings();
            user.banUser();

        }

        /// <summary>
        /// Permet d'obtenir la liste des utilisateurs bannis.
        /// </summary>
        private void lsUsersBans()
        {
            string msg = "Liste des utilisateurs bannis :";
            Console.WriteLine(melenchBot.lsUsers.getAllBanUsers().Count);
            foreach(User u in melenchBot.lsUsers.getAllBanUsers())
            {
                msg += " - " + u.userName;
            }
            melenchBot.writeMessage(msg);
        }
    }
}
