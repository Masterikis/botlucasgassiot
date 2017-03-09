using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot
{
    /// <summary>
    /// Permet d'analyser les messages reçus
    /// Permet de voir si le messsage est bien celui d'un utilisateur, si celui-ci est un lien, etc...
    /// </summary>
    public class MessageReceived
    {
        public string originalMessage { get; private set; }
        public string userName { get; private set; }
        public string userMessage { get; private set; }
        public bool itsMelenchBot { get; private set; }
        public bool itsMyCreator { get; private set; }
        public bool itsUserMessage { get; private set; }
        public bool itsALink { get { return this.itsALinkFunc(); } }
        public bool itsACommand { get { return this.itsACommandFunc(); }  }
        public bool containtsInsult { get { return this.containtsInsultFunc(); } }
        private string[] linkList = { "http:", "https:", "www.", ".org", ".com", ".fr", ".net" };
        private string[] insultsList = { "putain", "pt1", "ptin", "encul", "sale chienne", "merde", "salaud", "salope", "baise ", "pd", "pédé", "va chier", "pute", "nique", "tg", "ta gueule", "salaud", "bitch", "biatch"};

        private MessageReceived()
        {
        }

        public static MessageReceived parseMessage(string theMessage)
        {
            MessageReceived item = new MessageReceived();
            item.originalMessage = theMessage;
            item.itsUserMessage = false;
            item.itsMelenchBot = false;
            item.itsMyCreator = false;
            item.parserMessage(theMessage);
            return item;
        }

        /// <summary>
        /// Permet de parser les messages reçus.
        /// isUserMessage passe à true si celui-ci est un message écrit par un utilsateur.
        /// </summary>
        /// <param name="theMessage"></param>
        private void parserMessage(string theMessage)
        {
            if (theMessage.IndexOf("PRIVMSG") == -1) itsUserMessage = false;
            else
            {
                var start = theMessage.IndexOf(":") + 1;
                var end = theMessage.IndexOf("!") - 1;
                userName = theMessage.Substring(start, end);
                userMessage = theMessage.Substring(theMessage.IndexOf(":", end) + 1);
                itsUserMessage = true;
                if (userName == "melanchbot") itsMelenchBot = true;
                if (userName == "masterikis") itsMyCreator = true;
            }
        }

        /// <summary>
        /// Permet de reconnaitre si un message est un lien.
        /// </summary>
        /// <returns></returns>
        private bool itsALinkFunc()
        {
            if(itsUserMessage && linkList.Any(s => userMessage.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) != -1))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de reconnaitre si un message contient des insultes.
        /// </summary>
        /// <returns></returns>
        private bool containtsInsultFunc()
        {
            if (itsUserMessage && insultsList.Any(s => userMessage.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) != -1))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de reconnaitre si un message est une commande.
        /// </summary>
        /// <returns></returns>
        public bool itsACommandFunc()
        {
            if(itsUserMessage && userMessage.IndexOf("!") == 0)
            { 
                return true;
            }
            return false;
        }
    }
}
