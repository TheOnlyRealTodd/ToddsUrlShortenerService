namespace URLShortener.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOurId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Urls", "OurId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Urls", "OurId");
        }
    }
}
