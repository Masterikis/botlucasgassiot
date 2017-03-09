using MelenchBot.DataBase;
using MelenchBot_DataBase.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot_DataBase.DataBase
{
    public class DBStatusCommunication : DBCommunication
    {
        public DBStatusCommunication() : base()
        {}

        public Statuss getStatuss()
        {
            Statuss lsStatus = new Statuss();

            string query = "SELECT status_id, status_label FROM melenchbot_status";

            var reader = this.executeQuery(query);

            while (reader != null && reader.Read())
            {
                lsStatus.addStatus(new Status(reader.GetString(1), reader.GetInt32(0)));
            }
            if (reader != null) reader.Close();
            return lsStatus;
        }
    }
}
