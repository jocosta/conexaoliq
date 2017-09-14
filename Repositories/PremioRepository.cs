using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class PremioRepository
    {
        BrowserSession _browserSession = new BrowserSession();

        public ICollection<Premio> Listar()
        {

            return JsonResources.Get<ICollection<Premio>>("premios");

        }
    }
}