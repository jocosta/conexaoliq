using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class Agenda
    {
        public DateTime Data { get; set; }

        public ICollection<Atividade> Atividades { get; set; }
    }
}