using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class Faturamento
    {
        public Faturamento(string trimestre, string urlPdf)
        {
            Trimestre = trimestre;
            UrlPdf = urlPdf;
        }

        public string Trimestre { get; set; }

        public string UrlPdf { get; set; }

    }
}