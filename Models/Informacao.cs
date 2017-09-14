using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class Informacao
    {
        public Informacao(string texto, string[] tags, string imagem)
        {
            Texto = texto;
            Tags = tags;
            Imagem = new Imagem { Caminho = imagem };
        }

        public string Texto { get; set; }
        public string[] Tags { get; set; }
        public Imagem Imagem { get; set; }
    }
}