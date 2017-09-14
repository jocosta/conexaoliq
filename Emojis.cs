using CTX.Bot.ConexaoLiq.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LuisBot
{
    public class Emojis
    {

        private static Dictionary<string, string> emojis = new Dictionary<string, string>
        {
            {"@triste","\U0001F625" },
            {"@confuso","\U0001F615" },
            {"@sorriso","\U0001F601" },
            {"@piscada","\U0001F609" },
            {"@pensativo","\U0001F914" },
            {"@piscadelacomlingua","\U0001F61C" },
            {"@dinheiro","\U0001F632" },
            {"@boanoite", "\U0001F303" },
            {"@duvida", "\U0001F914" },
            {"@risada", "\U0001F605" },
            {"@upsidedown", "\U0001F643" },
            {"@sunglasses", "\U0001F60E" },
            {"@sorridente", "\U0001F60F" }
            
        };


        public static string SubstituirTags(string texto)
        {

            foreach (var emoji in emojis)
                texto = Regex.Replace(texto, emoji.Key, emoji.Value);

            return texto;
        }
    }
}