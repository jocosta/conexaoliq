using CTX.Bot.ConexaoLiq.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Infra.Configurations
{
    public class PesquisaConfig : EntityTypeConfiguration<Pesquisa>
    {
        public PesquisaConfig()
        {
            ToTable("TB_PESQUISA");

            HasKey(c => c.Id);
            
        }
    }
}