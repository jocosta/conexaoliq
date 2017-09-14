using CTX.Bot.ConexaoLiq.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Infra.Configurations
{
    public class UserActivityConfig :  EntityTypeConfiguration<UserActivity>
    {
        public UserActivityConfig()
        {
            ToTable("TB_ACTIVITY");

            HasKey(c => c.Id);

            Property(c => c.Activity)
                .HasColumnType("ntext");
        }
    }
}