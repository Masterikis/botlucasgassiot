using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot.Functionnalities
{
    public class LinkFunctionnality : Functionality
    {
        public LinkFunctionnality(MelenchBot theMelenchBot, bool theExcludeAdmin) : base(theMelenchBot, theExcludeAdmin)
        {
        }

        public override void ProceedWith(MessageReceived theMessage)
        {
            throw new NotImplementedException();
        }

        protected override bool isMatchWith(MessageReceived message)
        {
            throw new NotImplementedException();
        }
    }
}
