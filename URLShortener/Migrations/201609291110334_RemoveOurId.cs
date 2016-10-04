namespace URLShortener.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOurId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Urls", "OurId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Urls", "OurId", c => c.Int(nullable: false));
        }
    }
}
