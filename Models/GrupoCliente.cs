using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class GrupoCliente : Enumeration<GrupoCliente>
    {
        public static GrupoCliente Bpo = new GrupoCliente(0, "BPO I CRM");
        public static GrupoCliente Marketing = new GrupoCliente(1, "MARKETING SOLUTIONS");
        public static GrupoCliente It = new GrupoCliente(2, "IT");

        private GrupoCliente(int chave, string valor) : base(chave, valor)
        {

        }

        public GrupoCliente(int chave) : base(chave)
        { }


        public GrupoCliente()
        {

        }
    }
}