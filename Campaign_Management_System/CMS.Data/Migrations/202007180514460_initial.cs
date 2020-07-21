namespace CMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuickCampaigns", "isDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuickCampaigns", "isDeleted", c => c.Boolean(nullable: false));
        }
    }
}
