using MelenchBot_DataBase.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot_DataBase.DBClasses
{
    public class UserWarning
    {
        public int? warningId { get; set; }
        public int number { get; set; }

        public UserWarning(int? theWarningId, int theNumber)
        {
            warningId = theWarningId;
            number = theNumber;
        }
    }

    public class UserWarnings
    {
        public Dictionary<int?, UserWarning> list { get; private set; }

        public UserWarnings()
        {
            list = new Dictionary<int?, UserWarning>();
        }

        public UserWarning getUserWarning(int? theWarningId)
        {
            if (!list.ContainsKey(theWarningId)) return null;
            return list[theWarningId];
        }


        public void setWarning(int? theWarningId, int theNumber = 0)
        {
            if (list.ContainsKey(theWarningId)) list[theWarningId] = new UserWarning(theWarningId, theNumber);
            else list.Add(theWarningId, new UserWarning(theWarningId, theNumber));
        }

        public void addWarning(int? theWarningId, DBUserCommunication theDbUserCom, User theUser, int theNumber = 0)
        {
            if (!list.ContainsKey(theWarningId)) theDbUserCom.insertWarning(theWarningId, theNumber, theUser);
            else theDbUserCom.updateWarning(theWarningId, theNumber, theUser);
            setWarning(theWarningId, theNumber);

        }

        public void reiniAll()
        {
            foreach (KeyValuePair<int?, UserWarning> w in list)
            {
                w.Value.number = 0;
            }
        }
    }
}
