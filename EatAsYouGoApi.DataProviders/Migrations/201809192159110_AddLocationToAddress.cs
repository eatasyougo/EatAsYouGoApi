namespace EatAsYouGoApi.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationToAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Location");
        }
    }
}
