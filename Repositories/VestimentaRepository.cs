using CTX.Bot.ConexaoLiq.Models;
using LuisBot.Ctx.Baas.Core.Helpers;
using System.Collections.Generic;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class VestimentaRepository
    {
        BrowserSession _browserSession = new BrowserSession();

        public ICollection<Vestimenta> Listar()
        {
            return JsonResources.Get<ICollection<Vestimenta>>("vestimenta");
        }
    }
}