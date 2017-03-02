using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot.Classes
{
    public class User
    {
        public string userName { get; }
        public int linkWarning { get; set; }
        public int insultWarning { get; set; }
        public bool isBan { get; set; }

        public User(string theUserName)
        {
            userName = theUserName;
            linkWarning = 0;
            insultWarning = 0;
            isBan = false;
        }

        public int addLinkWarning()
        {
            return ++linkWarning;
        }

        public int addInsultWarning()
        {
            return ++insultWarning;
        }

        public void banUser()
        {
            isBan = true;
        }

        public void unbanUser()
        {
            isBan = false;
        }

        public void reinitWarnings()
        {
            linkWarning = 0;
            insultWarning = 0;
        }
    }

    public class Users
    {
        public Dictionary<string, User> lsUser { get; set; }

        public Users()
        {
            lsUser = new Dictionary<string, User>();
        }

        public User getUser(string theUserName)
        {
            if (!lsUser.ContainsKey(theUserName))
            {
                lsUser.Add(theUserName, new User(theUserName));
            }
            return lsUser[theUserName];
        }

        /// <summary>
        /// Permet d'obtenir une liste de tous les utilisateurs qui sont bannis.
        /// </summary>
        /// <returns></returns>
        public List<User> getAllBanUsers()
        {
            List<User> lsBanUser = new List<User>();

            foreach(KeyValuePair<string, User> u in lsUser)
            {
                if (u.Value.isBan)
                {
                    lsBanUser.Add(u.Value);
                }
            }

            return lsBanUser;
        }
    }
}
