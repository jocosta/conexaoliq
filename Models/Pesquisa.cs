
using System;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class Pesquisa
    {
        public Pesquisa()
        {
            Id = Guid.NewGuid();
            LastInteraction = Created = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Channel { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastInteraction { get; set; }

        public int? DinamicaMestreCerimonia { get; set; }

        public int? TemaMarcaFutureBrand { get; set; }

        public int? PalestraHortencia { get; set; }
        public string QualConhecimento { get; set; }

        public int? DesafiosHojeNelson { get; set; }

        public int? PlanejamentoEstrategico { get; set; }

        public int? PlanoMarketionChianello { get; set; }
        public int? ComercialProdutosFatimaOliveira { get; set; }
        public int? ExecucaoOperacional { get; set; }
        public int? CapitalHumano { get; set; }
        public int? TecnologiaSmart { get; set; }

        public int? ResponsabilidadeInegociavel { get; set; }
        public int? OpniaoBot { get; set; }
        public string OpniaoMelhoriaEvento { get; set; }

        public int? InfraEvento{get;set;}

    }
}