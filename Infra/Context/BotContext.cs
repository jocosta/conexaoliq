using CTX.Bot.ConexaoLiq.Infra.Configurations;
using CTX.Bot.ConexaoLiq.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CTX.Bot.ConexaoLiq.Infra.Context
{
    public class BotContext : DbContext
    {
        public BotContext()
            :base(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

        }

        public DbSet<UserActivity> Activities { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties()
                .Where(p => p.Name == "Id")
                .Configure(p => p.IsKey());

            //modelBuilder.Properties<string>()
            //   .Configure(p => p.HasMaxLength(300));

            modelBuilder.Properties<DateTime>()
              .Configure(p => p.HasColumnType("datetime2"));

            modelBuilder.Configurations.Add(new UserActivityConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}