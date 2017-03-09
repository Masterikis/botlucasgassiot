using MelenchBot.DataBase;
using MelenchBot_DataBase.DataBase;
using MelenchBot_DataBase.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot
{
    public class MelenchBotDBInterractions
    {

        private MelenchBot melenchBot;
        public Warnings lsWarnings { get; set; }
        public Statuss lsStatus { get; set; }
        public Users lsUsers { get; set; }
        private DBWarningCommunication dbWarningCommunication;
        private DBStatusCommunication dbStatusCommunication;
        private DBUserCommunication dbUserCommunication;

        public MelenchBotDBInterractions(MelenchBot theMelenchBot)
        {
            melenchBot = theMelenchBot;

            dbWarningCommunication = new DBWarningCommunication();
            lsWarnings = dbWarningCommunication.getWarnings();
            dbStatusCommunication = new DBStatusCommunication();
            lsStatus = dbStatusCommunication.getStatuss();
            dbUserCommunication = new DBUserCommunication();
            lsUsers = dbUserCommunication.getUsers(lsStatus);

            foreach (KeyValuePair<string, User> u in lsUsers.list)
            {
                dbUserCommunication.setUserWarnings(u.Value);
            }
        }

        public int getUserWarning(string theUserName, int? theWarningId)
        {
            User u = lsUsers.getUser(theUserName);
            if (u == null)
            {
                u = addNewUser(theUserName);
            }
            UserWarning uw = u.user_warnings.getUserWarning(theWarningId);
            if (uw == null)
            {
                u.user_warnings.addWarning(theWarningId, dbUserCommunication, u);
                return 0;
            }
            return uw.number;
        }

        public void addWarning(User theUser, int theWarningCount, int? theWarningId)
        {
            theUser.user_warnings.addWarning(theWarningId, dbUserCommunication, theUser, theWarningCount);
        }

        public User addNewUser(string theUserName)
        {
            dbUserCommunication.insertNewUser(theUserName);
            lsUsers.addUser(dbUserCommunication.getByUserName(theUserName, lsStatus));
            return lsUsers.getUser(theUserName);
        }

        public void banUser(string theUserName)
        {
            dbUserCommunication.banUser(theUserName);
            lsUsers.getUser(theUserName).user_status = lsStatus.getStatus(lsStatus.ban);
        }

        public void unbanUser(string theUserName)
        {
            dbUserCommunication.unbanUser(theUserName);
            lsUsers.getUser(theUserName).user_status = lsStatus.getStatus(lsStatus.normal);
        }

        public void reiniWarningUser(User theUser)
        {
            dbUserCommunication.reiniWarningUser(theUser);
            theUser.user_warnings.reiniAll();
        }
    }
}
