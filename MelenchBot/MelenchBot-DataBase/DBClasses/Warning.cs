using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot_DataBase.DBClasses
{
    public class Warning
    {
        public int? warning_id { get; set; }
        public string warning_label { get; set; }

        public Warning(string theWarning_label, int? theWarning_id = null)
        {
            warning_label = theWarning_label;
            warning_id = theWarning_id;
        }
    }

    public class Warnings
    {
        public Dictionary<string, Warning> list { get; private set; }
        public List<Warning> test;

        public Warnings()
        {
            list = new Dictionary<string, Warning>();
        }

        public Warning getWarning(string theWarningLabel)
        {
            if (!list.ContainsKey(theWarningLabel)) return null;
            return list[theWarningLabel];
        }

        public void addWarning(Warning theWarning)
        {
            list.Add(theWarning.warning_label, theWarning);
        }
    }
}
