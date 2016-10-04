namespace URLShortener.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOurUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Urls", "OurUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Urls", "OurUrl");
        }
    }
}
