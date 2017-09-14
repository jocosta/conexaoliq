using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.LUIS;
using Microsoft.Bot.Builder.Location.Bing;
using System.Collections.Generic;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class LocationRepository
    {
        public Dictionary<string, Location> Listar()
        {
            return JsonResources.Get<Dictionary<string, Location>>("locations");

        }
    }
}