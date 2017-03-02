using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Timers;

namespace MelenchBot.Classes
{
    public class Vote
    {

        private Timer timer;
        private Dictionary<string, string> votersList;
        private List<string> options;
        public bool timeOver { get; set; }

        public Vote(int theMinutes, List<string> theOptions)
        {
            votersList = new Dictionary<string, string>();
            timeOver = false;
            options = theOptions;
            timer = new Timer(new TimerCallback(Callback), null, 60000 * theMinutes, 100);   
        }

        public string addVote(string theUserName, string theOption)
        {
            int option = -1;
            try
            {
                option = int.Parse(theOption.Substring(theOption.ToLower().IndexOf(" ") + 1));
            } catch(Exception e)
            {
                return "Vote non prit en compte";
            }

            if (options.Count < option || option == -1) return "Vote non prit en compte";
            votersList[theUserName] = options[option - 1];
            return "Vote prit en compte";
        }

        public Dictionary<string, int> getResult()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach(string option in options)
            {
                result.Add(option, 0);
            }
            foreach(KeyValuePair<string, string> entry in votersList)
            {
                result[entry.Value]++;
            }
            return result;
        }

        private void Callback(object o)
        {
            timeOver = true;
        }

    }
}
