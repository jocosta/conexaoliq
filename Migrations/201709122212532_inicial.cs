namespace CTX.Bot.ConexaoLiq.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_ACTIVITY",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Activity = c.String(storeType: "ntext"),
                        Data = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TB_ACTIVITY");
        }
    }
}
