using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{

    public class Empresa
    {
        public string Nome { get; set; }
        public ICollection<Informacao> Institucional { get; set; }
    }
}