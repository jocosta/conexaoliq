
using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CTX.Bot.ConexaoLiq.Repositories
{

    public class SaudacaoRepository
    {

        public Informacao Obter(string texto)
        {
            var informacoes = JsonResources.Get<ICollection<Informacao>>("saudacoes")
              .Where(c => c.Tags.Any(x => x.ToSearch().Like(texto?.ToSearch())))
              ?.ToList();

            if (informacoes == null || !informacoes.Any())
                return null;

            Random random = new Random();            
            var informacao = informacoes[random.Next(informacoes.Count())];

            informacao.Texto = informacao.Texto.IncluirEmojis();

            return informacao;


            //var informacao = JsonResources.Get<ICollection<Informacao>>("saudacoes")
            //  .FirstOrDefault(c => c.Tags.Any(x => x.ToSearch().Like(texto?.ToSearch())));

            //if (informacao == null)
            //    return null;

            //informacao.Texto = informacao.Texto.IncluirEmojis();

            //return informacao;


        }

    }
}