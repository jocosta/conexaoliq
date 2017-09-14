using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class PresidenteRepository
    {


        public Presidente Pesquisar(string texto)
        {
            var presidentes = JsonResources.Get<ICollection<Presidente>>("presidentes");

            return presidentes.Where(c => texto.ToTagSearch().Any(x => c.Tags.ToSearch().Like(x.ToSearch()))).FirstOrDefault();
        }
    }
}