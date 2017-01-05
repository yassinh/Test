namespace DevTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DevTests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CampaignName = c.String(),
                        Date = c.DateTime(nullable: false),
                        Clicks = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        AffiliateName = c.String(),
                        Conversions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DevTests");
        }
    }
}
