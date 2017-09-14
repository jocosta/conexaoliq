using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class Premio
    {
        public Premio(string titulo, string texto, string imagem)
        {
            Titulo = titulo;
            Texto = texto;
            Imagem = new Imagem { Caminho = imagem };
        }

        public string Titulo { get; set; }

        public string Texto { get; set; }

        public Imagem Imagem { get; set; }

    }
}