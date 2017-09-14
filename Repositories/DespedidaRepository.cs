
using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CTX.Bot.ConexaoLiq.Repositories
{

    public class DespedidaRepository
    {

        public Informacao Obter()
        {
            var informacoes = JsonResources.Get<ICollection<Informacao>>("despedidas").ToList();

            if (informacoes == null || !informacoes.Any())
                return null;

            Random random = new Random();
            var informacao = informacoes[random.Next(informacoes.Count())];

            informacao.Texto = informacao.Texto.IncluirEmojis();

            return informacao;


        }

    }
}