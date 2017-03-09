using MelenchBot_DataBase.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot_DataBase.DBClasses
{
    public class User
    {

        public int? user_id { get; set; }
        public string user_name { get; set; }
        public Status user_status { get; set; }
        public UserWarnings user_warnings { get; set; }

        public User(string theUser_name, int? theUser_id = null, Status theUser_status = null, UserWarnings theUserWarnings = null)
        {
            user_id = theUser_id;
            user_name = theUser_name;
            user_status = theUser_status;
            user_warnings = (theUserWarnings != null) ? theUserWarnings : new UserWarnings();
        }

    }

    public class Users
    {
        public Dictionary<string, User> list { get; private set; }

        public Users()
        {
            list = new Dictionary<string, User>();
        }

        public User getUser(string theUserName)
        {
            if (!list.ContainsKey(theUserName)) return null;
            return list[theUserName];
        }

        public void addUser(User theUser)
        {
            list.Add(theUser.user_name, theUser);
        }
    }
}
