using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class Cliente
    {
        public Cliente(GrupoCliente grupoCliente, string nome, string url)
        {
            GrupoCliente = grupoCliente;
            Nome = nome;
            Imagem = new Imagem { Caminho = url };
        }

        public GrupoCliente GrupoCliente { get; set; }
        public string Nome { get; set; }

        public Imagem Imagem { get; set; }
    }
}