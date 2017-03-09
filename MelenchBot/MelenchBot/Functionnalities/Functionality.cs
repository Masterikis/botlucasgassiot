using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// IMPORTANT : Package pas encore fonctionelle, ne pas utiliser pour l'instant.
/// </summary>
namespace MelenchBot.Functionnalities
{
    /// <summary>
    /// Classe abstraite servant de bases aux différentes classes contenant des fonctionnalités.
    /// Basé sur le code de Anthony LARRE.
    /// </summary>
    public abstract class Functionality
    {

        public MelenchBot melenchBot { get; private set; }
        public bool excludeAdmin { get; private set; }
        public bool isActive { get; set; }
        protected abstract bool isMatchWith(MessageReceived message);

        public Functionality(MelenchBot theMelenchBot, bool theExcludeAdmin)
        {
            melenchBot = theMelenchBot;
            excludeAdmin = theExcludeAdmin;
        }

        public bool IsMatchWith(MessageReceived theMessage)
        {
            if(!isActive || excludeAdmin)
            {
                return false;
            }
            return isMatchWith(theMessage);
        }

        public abstract void ProceedWith(MessageReceived theMessage);

    }
}
