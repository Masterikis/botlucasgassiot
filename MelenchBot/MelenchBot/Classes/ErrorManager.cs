      using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelenchBot.Classes
{
    /// <summary>
    /// Permet de retourner des erreurs qui seront envoyés par MelenchBot dans le tchat.
    /// N'est pour l'instant qu'utiliser dans le tchat.
    /// </summary>
    public static class ErrorManager
    {
        public static Dictionary<int, string> voteError = new Dictionary<int, string> {
            { 0, "Format attendu : !vote minute, option1, option2, etc..." },
            { 1, "Impossible de faire un vote avec une seule option ..."},
            { 2, "La première valeur ça doit être du temps mon gars !"}
        };
    }

}
