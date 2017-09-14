using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class InformacaoBot: Informacao
    {
        public InformacaoBot(string texto, string[] tags, string imagem) : base(texto, tags, imagem)
        {
        }

        public bool ExibirCriadores { get; set; }

        public bool ExibirImagemBot { get; set; }

        public ICollection<Cargo> Criadores { get; set; }

        public string ImagemBot { get; set; }
    }

    public class Bot
    {
        public ICollection<InformacaoBot> Textos { get; set; }

        public ICollection<Cargo> Criadores { get; set; }
    }
}