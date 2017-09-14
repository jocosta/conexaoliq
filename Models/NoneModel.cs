using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class NoneModel
    {
        public NoneModel(string chave, string texto, string imagem)
        {
            Chave = chave;
            Texto = texto;
            Imagem = new Imagem { Caminho = imagem };
        }
        public string Chave { get; set; }

        public string Texto { get; set; }

        public Imagem Imagem { get; set; }
    }
}