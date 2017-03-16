using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MelenchBot.Messages
{
    /// <summary>
    /// Classe permettant la gestion d'envois de messages.
    /// La connexion à l'IRC Twitch et au salon se faisant avec des messages écrit, cette classe s'occupe également de ces fonctions
    /// Une queue et un Thread permet d'éviter de perdre les messages.
    /// </summary>
    public class MessageSender
    {

        private ConcurrentQueue<String> _queue = new ConcurrentQueue<string>();
        private static StreamWriter writer;
        private CancellationToken token;

        public MessageSender(StreamWriter theWriter, CancellationToken theToken)
        {
            writer = theWriter;
            token = theToken;

            Thread t = new Thread(this.threadSender);
            t.Start();
            
        }

        /// <summary>
        /// Permet la connexion au serveur IRC de twitch via login/mdp du bot
        /// Le mdp est généré via oAuth (en partenariat avec twitch)
        /// </summary>
        /// <param name="thePassword"></param>
        /// <param name="theUserName"></param>
        public void connection(string thePassword, string theUserName)
        {
            try
            {
                writer.Write("PASS " + thePassword + "\n");
                writer.Flush();
                writer.Write("NICK " + theUserName + "\n");
                writer.Flush();
                Console.WriteLine("Connexion effectué.");
            } catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la connexion : " + e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
            
        }

        /// <summary>
        /// Permet de rejoindre un salon.
        /// </summary>
        /// <param name="theChannel">Le nom du salon (qui doit être précédé d'un #)</param>
        /// <param name="theNickName">Le nom d'utilisateur (ici MelenchBot)</param>
        public void joinChannel(string theChannel, string theNickName)
        {
            try
            {
                writer.WriteLine("JOIN " + theChannel);
                writer.Flush();
                Console.WriteLine("Salon rejoint avec succès.");
                writeMessage("Repectez les règles bande de capitalistes !", theNickName, theChannel);
            }
            catch (Exception e)
            {
                Console.WriteLine("Impossible de rejoindre le salon : " + e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Permet d'écrire un message dans le tchat.
        /// Passe par le Thread pour ne pas perdre de messages.
        /// </summary>
        /// <param name="theMessage"></param>
        /// <param name="theNickName"></param>
        /// <param name="theChannel"></param>
        public void writeMessage(string theMessage, string theNickName, string theChannel)
        {
            theMessage = "[BOT] " + theMessage;

            theMessage = ":" + theNickName + "!" + theNickName + "@" + theNickName +
                                     "tmi.twitch.tv PRIVMSG " + theChannel + " : " + theMessage;

            _queue.Enqueue(theMessage);
        }

        /// <summary>
        /// Permet de bannir un utilisateur.
        /// </summary>
        /// <param name="theUserName"></param>
        /// <param name="theNickName"></param>
        /// <param name="theChannel"></param>
        public void banUser(string theUserName, string theNickName, string theChannel)
        {
            string theMessage = ":" + theNickName + "!" + theNickName + "@" + theNickName +
                                     "tmi.twitch.tv PRIVMSG " + theChannel + " : " + "/ban " + theUserName;
            Console.WriteLine(theMessage);
            _queue.Enqueue(theMessage);
        }

        /// <summary>
        /// Permet de débannir un utilisateur.
        /// </summary>
        /// <param name="theUserName"></param>
        /// <param name="theNickName"></param>
        /// <param name="theChannel"></param>
        public void unbanUser(string theUserName, string theNickName, string theChannel)
        {
            string theMessage = ":" + theNickName + "!" + theNickName + "@" + theNickName +
                                     "tmi.twitch.tv PRIVMSG " + theChannel + " : " + "/unban " + theUserName;

            _queue.Enqueue(theMessage);
        }

        /// <summary>
        /// Permet de gérer l'envoie de message.
        /// Si ce Thread n'est pas utilisé, certains messages peuvent être perdus.
        /// Le sleep est d'1.5 secondes (ce qui permet de ne jamais perdre de messages, après tests)
        /// </summary>
        private void threadSender()
        {
            while (!this.token.IsCancellationRequested)
            {
                string message = null;

                if(this._queue.TryDequeue(out message))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }

                Thread.Sleep(1500);
            }
            Console.WriteLine("Fin du Thread d'envoie");
        }


        private void threadSenderV2()
        {
            while (!this.token.IsCancellationRequested)
            {
                string message = null;

                if (this._queue.TryDequeue(out message))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }

                Thread.Sleep(1500);
            }
            Console.WriteLine("Fin du Thread d'envoie");
        }
    }
}
