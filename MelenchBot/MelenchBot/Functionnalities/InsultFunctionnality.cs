using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot.Functionnalities
{
    class InsultFunctionnality : Functionality
    {
        private string[] insultsList = { "putain", "pt1", "ptin", "encul", "sale chienne", "merde", "salaud", "salope", "baise ", "pd", "pédé", "va chier", "pute", "nique", "tg", "ta gueule", "salaud", "bitch", "biatch" };

        public InsultFunctionnality(MelenchBot theMelenchBot, bool theExcludeAdmin) : base(theMelenchBot, theExcludeAdmin)
        {
        }

        protected override bool isMatchWith(MessageReceived theMessage)
        {
            if(theMessage.itsUserMessage && insultsList.Any(s => theMessage.userMessage.IndexOf(s,StringComparison.InvariantCultureIgnoreCase) != 1))
            {
                return true;
            }
            return false;
        }

        public override void ProceedWith(MessageReceived theMessage)
        {
            melenchBot.containtsSomeInsults(theMessage.userName);
        }
    }
}
