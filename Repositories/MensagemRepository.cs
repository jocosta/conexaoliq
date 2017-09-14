using LuisBot.Ctx.Baas.Core.Helpers;
using CTX.Bot.ConexaoLiq.Helpers;
using CTX.Bot.ConexaoLiq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Repositories
{
    public class MensagemRepository
    {



        public string Pesquisar(TipoMensagem tipoMensagem, string parametro)
        {
            var mensagem = Pesquisar(tipoMensagem);

            if (mensagem.Contains("{0}"))
                return string.Format(mensagem, parametro);

            return mensagem;
        }

        public string Pesquisar(TipoMensagem tipoMensagem)
        {
            var mensagens = JsonResources.Get<ICollection<Mensagem>>("mensagens")
                .Where(c => c.TipoMensagem == tipoMensagem);



            Random random = new Random();
            var mensagem = mensagens.ElementAt(random.Next(mensagens.Count()));


            return mensagem.Texto.IncluirEmojis();

        }

    }
}