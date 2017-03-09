using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot_DataBase.DBClasses
{
    public class Status
    {
        public int? status_id { get; set; }
        public string status_label { get; set; }

        public Status(string theStatus_label, int? theStatus_id)
        {
            status_id = theStatus_id;
            status_label = theStatus_label;
        }
    }

    public class Statuss
    {
        public int normal { get; private set; } 
        public int ban { get; private set; }

        public Dictionary<int?, Status> list { get; private set; }

        public Statuss()
        {
            ban = int.Parse(ConfigurationManager.AppSettings["statusBan"]);
            normal = int.Parse(ConfigurationManager.AppSettings["statusNormal"]);
            
            list = new Dictionary<int?, Status>();
        }

        public Status getStatus(int? theStatusId)
        {
            if (!list.ContainsKey(theStatusId)) return null;
            return list[theStatusId];
        }

        public void addStatus(Status theStatus)
        {
            list.Add(theStatus.status_id, theStatus);
        }
    }
}
