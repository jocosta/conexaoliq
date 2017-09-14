using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class Mensagem
    {
        public TipoMensagem TipoMensagem { get; set; }

        public string Texto { get; set; }
    }
}