﻿using MelenchBot.Classes;
using MelenchBot_DataBase.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private Vote vote;
        private CitationsMorsay citationsMorsay;

        public CommandsMelenchBot(MelenchBot theMelenchBot)
        {
            melenchBot = theMelenchBot;
            citationsMorsay = new CitationsMorsay();
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
                if (command.ToLower().Contains("!melenchbot") && !theMsgReceived.itsMyCreator) melenchBot.writeMessage(theMsgReceived.userName + " => Qu'est-ce que t'as toi ?");
                if (command.ToLower().Contains("!morsay")) morsayCitation();
            }
            else
            {
                if ((command.Contains("!unban") || command.Contains("!ban")) && !theMsgReceived.itsMyCreator) melenchBot.writeMessage(theMsgReceived.userName + " => Ya des baffes qui se perdent ...");
                if (theMsgReceived.userMessage.ToLower().Contains("!vote ") && !theMsgReceived.itsMyCreator) addUserVote(theMsgReceived.userName, theMsgReceived.userMessage);
            }
        }

        /// <summary>
        /// Permet d'analyser une commande de l'admin et de lancer cette derniere si celle-ci existe
        /// </summary>
        /// <param name="theMsgReceived"></param>
        public void analyzeCommandCreator(string theCommand)
        {
            if (theCommand.Contains("!melenchbot")) melenchBot.writeMessage("Ils peuvent toujours nommer Donald à l'Europe; c’est quand même Picsou qui commande.");
                if (theCommand.Contains("!unban ")) unBanUser(theCommand.Substring(theCommand.IndexOf(" ") + 1));
                if (theCommand.Contains("!ban ")) banUser(theCommand.Substring(theCommand.IndexOf(" ") + 1));
            if (theCommand.Contains("!reini ")) reiniWarningUser(theCommand.Substring(theCommand.IndexOf(" ") + 1));
            if (theCommand.Contains("!vote")) melenchBot.writeMessage(launchNewVote(theCommand));

        }

        /// <summary>
        /// Permet de debannir un utilisateur.
        /// </summary>
        /// <param name="theUserName"></param>
        private void unBanUser(string theUserName)
        {
            melenchBot.mlbDBInterract.unbanUser(theUserName);
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
            melenchBot.mlbDBInterract.banUser(theUserName);
            //User user = melenchBot.lsUsers.getUser(theUserName);
            //user.reinitWarnings();
            //user.banUser();
        }

        private void reiniWarningUser(string theUserName)
        {
            melenchBot.writeMessage(theUserName + " ==> Tes péchés sont pardonnés.");
            melenchBot.mlbDBInterract.reiniWarningUser(melenchBot.mlbDBInterract.lsUsers.getUser(theUserName));
        }

        /// <summary>
        /// Permet d'obtenir la liste des utilisateurs bannis.
        /// </summary>
        private void lsUsersBans()
        {
            string msg = "Liste des utilisateurs bannis :";
            Status ban = melenchBot.mlbDBInterract.lsStatus.getStatus(melenchBot.mlbDBInterract.lsStatus.ban);
            foreach (KeyValuePair<string, User> u in melenchBot.mlbDBInterract.lsUsers.list)
            {
                if(u.Value.user_status == ban) msg += " - " + u.Value.user_name;
            }
            melenchBot.writeMessage(msg);
            //Console.WriteLine(melenchBot.lsUsers.getAllBanUsers().Count);
            //foreach(UserOld u in melenchBot.lsUsers.getAllBanUsers())
            //{
            //    msg += " - " + u.userName;
            //}
            //melenchBot.writeMessage(msg);
        }

        /// <summary>
        /// Permet de lancer un nouveau vote.
        /// Le format autorisé doit etre : "!vote $minutes$,$option1$,$option2$, ...
        /// TODO : Voir comment améliorer ce code.
        /// </summary>
        /// <param name="theCommande"></param>
        /// <returns></returns>
        private string launchNewVote(string theCommande)
        {
            int beforeMinute = theCommande.IndexOf(" ");
            if(beforeMinute == -1) return ErrorManager.voteError[0];

            int positionFirstComma = theCommande.IndexOf(",");
            if (positionFirstComma == -1) return ErrorManager.voteError[0];

            int minutes = 0;
            //Permet de vérifier s'il s'agit bien de temps. Evite le FormatException.
            try
            {
                minutes = int.Parse(theCommande.Substring(beforeMinute + 1, positionFirstComma - beforeMinute - 1));
            } catch(FormatException e)
            {
                Console.WriteLine("Erreur de type : " + e.Message);
                return ErrorManager.voteError[2];
            }
            List<string> lsOptions = new List<string>();

            int positionSecondComma = theCommande.IndexOf(",", positionFirstComma + 1);
            bool remainingComma = (positionSecondComma != -1);

            if (!remainingComma) return ErrorManager.voteError[1];
            lsOptions.Add(theCommande.Substring(positionFirstComma + 1, positionSecondComma - positionFirstComma - 1));

            while (remainingComma)
            {
                positionFirstComma = positionSecondComma;
                positionSecondComma = theCommande.IndexOf(",", positionFirstComma + 1);
                if (positionSecondComma == -1)
                {
                    remainingComma = false;
                    lsOptions.Add(theCommande.Substring(positionFirstComma + 1));
                }
                else lsOptions.Add(theCommande.Substring(positionFirstComma + 1, positionSecondComma - positionFirstComma - 1));
            }

            vote = new Vote(minutes, lsOptions);

            string messageLaunchVote = "Référendum, temps : " + minutes + " minute(s), votez avec !vote *numero*";
            int index = 1;
            foreach (string option in lsOptions)
            {
                messageLaunchVote += ", " + index + " pour " + option + " ";
                index++;
            }
            Task.Run(() => this.threadVote());
            return messageLaunchVote;
        }

        /// <summary>
        /// Permet d'ajouter un vote si un référendum est en cours.
        /// </summary>
        /// <param name="theUserName"></param>
        /// <param name="theOption"></param>
        private void addUserVote(string theUserName, string theOption)
        {
            if (vote != null && !vote.timeOver) melenchBot.writeMessage(theUserName + " => " + vote.addVote(theUserName, theOption));
            else melenchBot.writeMessage("Aucun référendum en cours.");
        }

        /// <summary>
        /// Thread permettant d'annoncer lorsqu'un référendum est terminé, ainsi que de données les résultats.
        /// </summary>
        private void threadVote()
        {
            while (!vote.timeOver)
            {
                Thread.Sleep(1000);
            }
            melenchBot.writeMessage("Référendum terminé !!! Les résultats arrivent ...");
            string messageResult = "Les résultats sont là";
            foreach(KeyValuePair<string, int> entry in vote.getResult())
            {
                messageResult += ", " + entry.Key + " : " + entry.Value;
            }
            melenchBot.writeMessage(messageResult);
        }

        private void morsayCitation()
        {
            if (!citationsMorsay.timeOver)
            {
                melenchBot.writeMessage("On se calme, c'est une citation toutes les 5 minutes max.");
            }
            else
            {
                Random rand = new Random();
                int morsayCitation = rand.Next(0, CitationsMorsay.citationsMorsay.Length);
                citationsMorsay.launchTimer();
                melenchBot.writeMessage("Comme le disait le grand philosophe Morsay : " + CitationsMorsay.citationsMorsay[morsayCitation]);
            }
        }
    }
}
