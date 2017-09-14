
using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;

namespace CTX.Bot.ConexaoLiq.Repositories
{

    public class InformacaoBotRepository
    {

        public InformacaoBot Obter(string texto)
        {

            texto = texto + string.Empty;

            var bot = JsonResources.Get<Models.Bot>("bot");

            var informacao = bot.Textos
                .FirstOrDefault(c => c.Tags.Any(x => x.ToSearch().Like(texto?.ToSearch())));

            if (informacao == null)
                return null;

            informacao.Texto = informacao.Texto.IncluirEmojis();

            if (informacao.ExibirCriadores)
            {
                informacao.Criadores = bot.Criadores;
            }

            return informacao;

        }

    }
}