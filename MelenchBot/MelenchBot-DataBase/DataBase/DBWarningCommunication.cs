using MelenchBot.DataBase;
using MelenchBot_DataBase.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot_DataBase.DataBase
{
    public class DBWarningCommunication : DBCommunication
    {
        public DBWarningCommunication() : base() { }

        public Warnings getWarnings()
        {
            Warnings lsWarnings = new Warnings();
            string query = "SELECT warning_id, warning_label FROM melenchbot_warning";

            var reader = this.executeQuery(query);
            while(reader != null && reader.Read())
            {
                lsWarnings.addWarning(new Warning(
                    reader.GetString(1),
                    reader.GetInt32(0)
                    ));
            }
            if (reader != null) reader.Close();
            return lsWarnings;
        }
    }
}
