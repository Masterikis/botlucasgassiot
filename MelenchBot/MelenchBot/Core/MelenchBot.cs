using MelenchBot.Classes;
using MelenchBot.Messages;
using MelenchBot_DataBase.DBClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MelenchBot
{
    public class MelenchBot
    {

        private ConnectionDatas connectionDatas;
        public MelenchBotDBInterractions mlbDBInterract { get; private set; }

        private TcpClient client;
        private static StreamReader reader;
        private MessageSender messageSender;
        private CommandsMelenchBot cmdsMelenchBot;
        public event EventHandler<MessageReceived> onMessageReceveid;

        private CancellationTokenSource source = new CancellationTokenSource();

        public MelenchBot(int theMaxRetries = 3)
        {
            connectionDatas = new ConnectionDatas();

            client = new TcpClient(connectionDatas.serverAddress, connectionDatas.port);
            reader = new StreamReader(client.GetStream());


            messageSender = new MessageSender(new StreamWriter(client.GetStream()), source.Token);
            cmdsMelenchBot = new CommandsMelenchBot(this);
            mlbDBInterract = new MelenchBotDBInterractions(this);
        }

        /// <summary>
        /// Permet de lancer la connexion du bot au tchat twitch.
        /// </summary>
        public void connect()
        {
            connection();
            joinChannel();

            Task.Run(() => this.listen());
        }

        public void stop()
        {
            source.Cancel();
        }

        /// <summary>
        /// Thread permettant d'écouter les messages.
        /// </summary>
        private void listen()
        {
            while (true)
            {
                inspectTheChat();
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Permet la connection à Twitch.
        /// </summary>
        public void connection()
        {
            messageSender.connection(connectionDatas.password, connectionDatas.userName);
        }

        /// <summary>
        /// Permet de rejoindre un salon
        /// </summary>
        public void joinChannel()
        {
            messageSender.joinChannel(connectionDatas.channel, connectionDatas.nickName);
        }

        /// <summary>
        /// Permet d'envoyer un message sur le tchat de Twitch
        /// </summary>
        /// <param name="theMessage"></param>
        public void writeMessage(string theMessage)
        {
            messageSender.writeMessage(theMessage, connectionDatas.nickName, connectionDatas.channel);
        }

        /// <summary>
        /// Permet de ban un utilisateur
        /// </summary>
        /// <param name="theUserName"></param>
        public void banUser(string theUserName)
        {
            messageSender.banUser(theUserName, connectionDatas.nickName, connectionDatas.channel);
        }

        /// <summary>
        /// Permet de unban un utilisateur
        /// </summary>
        /// <param name="theUserName"></param>
        public void unbanUser(string theUserName)
        {
            messageSender.unbanUser(theUserName, connectionDatas.nickName, connectionDatas.channel);
        }

        /// <summary>
        /// Permet de gérer les différents messages pouvant apparaitre dans le tchat, devant attiré l'attention du bot.
        /// </summary>
        public void inspectTheChat()
        {
            string message;
            MessageReceived msgReceived;
            while((message = reader.ReadLine()) != null)
            {
                msgReceived = MessageReceived.parseMessage(message); //Développeur .net (base)
                onMessageReceveid?.Invoke(this, msgReceived); // if (onMessageReceveid != null) onMessageReceveid(this, msgReceived);
                if (msgReceived.itsUserMessage && !msgReceived.itsMelenchBot && !msgReceived.itsMyCreator)
                {
                    if (msgReceived.itsALink) userSendLink(msgReceived.userName);
                    if (msgReceived.containtsInsult) containtsSomeInsults(msgReceived.userName);
                    if (msgReceived.itsACommand) cmdsMelenchBot.analyzeCommand(msgReceived);
                }
                else if (msgReceived.itsACommand && msgReceived.itsMyCreator) {
                    if (msgReceived.itsACommand) cmdsMelenchBot.analyzeCommand(msgReceived);
                    //cmdsMelenchBot.analyzeCommandCreator(msgReceived.userMessage);
                }
            }
        }

        ///// <summary>
        ///// Permet de gérer lorsqu'un utilisateur envoie un lien sur le tchat.
        ///// Envoie jusqu'à 3 avertissements, au quatrième lien, la personne est banni.
        ///// </summary>
        ///// <param name="theUserName"></param>
        //public void userSendLink(string theUserName)
        //{
        //    User user = lsUsers.getUser(theUserName);
        //    int warningCount = user.addLinkWarning();
        //    Console.WriteLine(warningCount);
        //    switch (warningCount)
        //    {
        //        case 1 :
        //            writeMessage(theUserName + " : Pas de lien, premier avertissement !");
        //            break;
        //        case 2:
        //            writeMessage(theUserName + " : Pas de lien, deuxième avertissement !");
        //            break;
        //        case 3:
        //            writeMessage(theUserName + " : Pas de lien, troisième et dernier avertissement !");
        //            break;
        //        default:
        //            writeMessage(theUserName + " ==> Au goulag le capitaliste !");
        //            banUser(theUserName);
        //            user.reinitWarnings();
        //            user.banUser();
        //            break;

        //    }
        //}


        /// <summary>
        /// Permet de gérer lorsqu'un utilisateur envoie un lien sur le tchat.
        /// Envoie jusqu'à 3 avertissements, au quatrième lien, la personne est banni.
        /// </summary>
        /// <param name="theUserName"></param>
        public void userSendLink(string theUserName)
        {
            User user = mlbDBInterract.lsUsers.getUser(theUserName);
            if(user == null)
            {
                user = mlbDBInterract.addNewUser(theUserName);
            }
            int warningCount = mlbDBInterract.getUserWarning(theUserName, mlbDBInterract.lsWarnings.getWarning("Link").warning_id);
            warningCount++;
            mlbDBInterract.addWarning(user, warningCount, mlbDBInterract.lsWarnings.getWarning("Link").warning_id);
            
            switch (warningCount)
            {
                case 1:
                    writeMessage(theUserName + " : Pas de lien, premier avertissement !");
                    break;
                case 2:
                    writeMessage(theUserName + " : Pas de lien, deuxième avertissement !");
                    break;
                case 3:
                    writeMessage(theUserName + " : Pas de lien, troisième et dernier avertissement !");
                    break;
                default:
                    writeMessage(theUserName + " ==> Au goulag le capitaliste !");
                    banUser(theUserName);
                    mlbDBInterract.banUser(theUserName);
                    break;

            }
        }

        /// <summary>
        /// Permet de gérer lorsqu'un utilisateur envoie des insultes (ou grossierté).
        /// Envoie jusqu'à 3 avertissements, au quatrième lien, la personne est banni.
        /// </summary>
        /// <param name="theUserName"></param>
        public void containtsSomeInsults(string theUserName)
        {
            User user = mlbDBInterract.lsUsers.getUser(theUserName);
            if (user == null)
            {
                user = mlbDBInterract.addNewUser(theUserName);
            }
            int warningCount = mlbDBInterract.getUserWarning(theUserName, mlbDBInterract.lsWarnings.getWarning("Insult").warning_id);
            warningCount++;
            mlbDBInterract.addWarning(user, warningCount, mlbDBInterract.lsWarnings.getWarning("Insult").warning_id);
            switch (warningCount)
            {
                case 1:
                    writeMessage(theUserName + " : Pas de grossierté, premier avertissement !");
                    break;
                case 2:
                    writeMessage(theUserName + " : Pas de grossierté, deuxième avertissement !");
                    break;
                case 3:
                    writeMessage(theUserName + " : Pas de grossierté, troisième et dernier avertissement !");
                    break;
                default:
                    writeMessage(theUserName + " ==> Au goulag le capitaliste !");
                    banUser(theUserName);
                    mlbDBInterract.banUser(theUserName);
                    break;
            }
        }

        ///// <summary>
        ///// Permet de gérer lorsqu'un utilisateur envoie des insultes (ou grossierté).
        ///// Envoie jusqu'à 3 avertissements, au quatrième lien, la personne est banni.
        ///// </summary>
        ///// <param name="theUserName"></param>
        //public void containtsSomeInsults(string theUserName)
        //{
        //    Classes.User user = lsUsers.getUser(theUserName);
        //    int warningCount = user.addInsultWarning();
        //    switch (warningCount)
        //    {
        //        case 1:
        //            writeMessage(theUserName + " : Pas de grossierté, premier avertissement !");
        //            break;
        //        case 2:
        //            writeMessage(theUserName + " : Pas de grossierté, deuxième avertissement !");
        //            break;
        //        case 3:
        //            writeMessage(theUserName + " : Pas de grossierté, troisième et dernier avertissement !");
        //            break;
        //        default:
        //            writeMessage(theUserName + " ==> Au goulag le capitaliste !");
        //            banUser(theUserName);
        //            user.reinitWarnings();
        //            user.banUser();
        //            break;
        //    }
        //}
    }
}
