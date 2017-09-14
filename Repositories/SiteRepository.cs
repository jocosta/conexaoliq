using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class SiteRepository
    {

        public ICollection<Site> Listar()
        {
            return JsonResources.Get<ICollection<Site>>("sites");
        }
    }
}