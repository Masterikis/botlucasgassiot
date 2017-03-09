using MelenchBot.DataBase;
using MelenchBot_DataBase.DBClasses;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot_DataBase.DataBase
{
    public class DBUserCommunication : DBCommunication
    {
        public DBUserCommunication() : base()
        {}

        public Users getUsers(Statuss lsStatus)
        {
            Users lsUsers = new Users();
            string query = "SELECT user_id, user_name, user_status FROM melenchbot_user";

            var reader = this.executeQuery(query);
            while (reader != null && reader.Read())
            {
                lsUsers.addUser(
                    new User(
                        reader.GetString(1), 
                        reader.GetInt32(0), 
                        lsStatus.getStatus(reader.GetInt32(2))
                        )
                );
            }
            if (reader != null) reader.Close();
            return lsUsers;
        }

        public void setUserWarnings(User theUser)
        {
            string query = "SELECT warning_id, user_warning_number FROM melenchbot_user_warning WHERE user_id =" + theUser.user_id;

            var reader = this.executeQuery(query);
            while(reader != null && reader.Read())
            {
                theUser.user_warnings.setWarning(reader.GetInt32(0), reader.GetInt32(1));
            }
            if (reader != null) reader.Close();
        }

        public void insertNewUser(string theUserName)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = dbConnection.Connection;
            cmd.CommandText = "INSERT INTO melenchbot_user(user_name, user_status) VALUES (?,1)";
            cmd.Parameters.Add("user_name", MySqlDbType.VarChar).Value = theUserName;
            cmd.ExecuteNonQuery();
        }

        public User getByUserName(string theUserName, Statuss lsStatus)
        {
            User u = null;
            string query = "SELECT user_id, user_name, user_status FROM melenchbot_user WHERE user_name = '" + theUserName + "'";

            var reader = this.executeQuery(query);
            while (reader != null && reader.Read())
            {
                u = new User(reader.GetString(1), reader.GetInt32(0), lsStatus.getStatus(reader.GetInt32(2)));
            }
            if (reader != null) reader.Close();
            return u;
        }

        public void insertWarning(int? theWarningId, int theNumber, User theUser)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = dbConnection.Connection;
            cmd.CommandText = "INSERT INTO melenchbot_user_warning(user_id, warning_id, user_warning_number) VALUES (?,?,?)";
            cmd.Parameters.Add("user_id", MySqlDbType.Int32).Value = theUser.user_id;
            cmd.Parameters.Add("warning_id", MySqlDbType.Int32).Value = theWarningId;
            cmd.Parameters.Add("user_warning_number", MySqlDbType.Int32).Value = theNumber;
            cmd.ExecuteNonQuery();
        }

        public void updateWarning(int? theWarningId, int theNumber, User theUser)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = dbConnection.Connection;
            cmd.CommandText = "UPDATE melenchbot_user_warning SET user_warning_number = @theNumber WHERE user_id = @theUserId AND warning_id = @theWarningId";
            cmd.Parameters.AddWithValue("@theUserId", theUser.user_id);
            cmd.Parameters.AddWithValue("@theWarningId", theWarningId);
            cmd.Parameters.AddWithValue("@theNumber", theNumber);
            cmd.ExecuteNonQuery();
        }

        public void banUser(string theUserName)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = dbConnection.Connection;
            cmd.CommandText = "UPDATE melenchbot_user SET user_status = @theStatus WHERE user_name = @theUserName";
            cmd.Parameters.Add("@theStatus", MySqlDbType.Int32).Value = int.Parse(ConfigurationManager.AppSettings["statusBan"]);
            cmd.Parameters.AddWithValue("@theUserName", theUserName);
            cmd.ExecuteNonQuery();
        }

        public void unbanUser(string theUserName)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = dbConnection.Connection;
            cmd.CommandText = "UPDATE melenchbot_user SET user_status = @theStatus WHERE user_name = @theUserName";
            cmd.Parameters.Add("@theStatus", MySqlDbType.Int32).Value = int.Parse(ConfigurationManager.AppSettings["statusNormal"]);
            cmd.Parameters.AddWithValue("@theUserName", theUserName);
            cmd.ExecuteNonQuery();
        }

        public void reiniWarningUser(User theUser)
        {
            var cmd = new MySqlCommand();
            cmd.Connection = dbConnection.Connection;
            cmd.CommandText = "UPDATE melenchbot_user_warning SET user_warning_number = @theNumber WHERE user_id = @theUserId";
            cmd.Parameters.AddWithValue("@theUserId", theUser.user_id);
            cmd.Parameters.AddWithValue("@theNumber", 0);
            cmd.ExecuteNonQuery();
        }
    }
}
