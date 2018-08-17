namespace EatAsYouGoApi.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Restaurants",
                c => new
                {
                    RestaurantId = c.Long(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    CuisineType = c.String(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.RestaurantId);

            CreateTable(
                "dbo.MenuItems",
                c => new
                {
                    MenuItemId = c.Long(nullable: false, identity: true),
                    RestaurantId = c.Long(nullable: false),
                    Name = c.String(nullable: false),
                    Description = c.String(),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Photo = c.String(),
                    IsFlatDiscountVoucher = c.Boolean(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.MenuItemId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId);

            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Long(nullable: false, identity: true),
                        RestaurantId = c.Long(nullable: false),
                        AddLine1 = c.String(nullable: false),
                        AddLine2 = c.String(),
                        AddLine3 = c.String(),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        PostCode = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        DealId = c.Long(nullable: false, identity: true),
                        RestaurantId = c.Long(nullable: false),
                        DealName = c.String(nullable: false),
                        DealDate = c.DateTime(nullable: false),
                        TimeFrom = c.Time(nullable: false, precision: 7),
                        TimeTo = c.Time(nullable: false, precision: 7),
                        OrderByTime = c.Time(precision: 7),
                        MaxOrder = c.Int(),
                    })
                .PrimaryKey(t => t.DealId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.DealMenuItems",
                c => new
                    {
                        DealId = c.Long(nullable: false),
                        MenuItemId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.DealId, t.MenuItemId })
                .ForeignKey("dbo.Deals", t => t.DealId, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuItemId, cascadeDelete: false)
                .Index(t => t.DealId)
                .Index(t => t.MenuItemId);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.Long(nullable: false, identity: true),
                    FirstName = c.String(),
                    LastName = c.String(),
                    Password = c.String(),
                    Email = c.String(nullable: false),
                    Mobile = c.String(),
                    Landline = c.String(),
                    LoginAttempts = c.Int(nullable: false),
                    AccountLocked = c.Boolean(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    RestaurantId = c.Long(),
                    PreferredLocation = c.String(),
                })
                .PrimaryKey(t => t.UserId);

            CreateTable(
                "dbo.Permissions",
                c => new
                {
                    PermissionId = c.Int(nullable: false, identity: true),
                    PermissionType = c.String(nullable: false),
                    PermissionDescription = c.String(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.PermissionId);

            CreateTable(
                "dbo.Groups",
                c => new
                {
                    GroupId = c.Int(nullable: false, identity: true),
                    GroupName = c.String(nullable: false),
                    GroupDescription = c.String(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.GroupId);

            CreateTable(
                "dbo.GroupPermissions",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        PermissionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.PermissionId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupPermissions", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.UserGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupPermissions", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Addresses", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Deals", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.MenuItems", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.DealMenuItems", "MenuItemId", "dbo.MenuItems");
            DropForeignKey("dbo.DealMenuItems", "DealId", "dbo.Deals");
            DropIndex("dbo.UserGroups", new[] { "GroupId" });
            DropIndex("dbo.UserGroups", new[] { "UserId" });
            DropIndex("dbo.GroupPermissions", new[] { "PermissionId" });
            DropIndex("dbo.GroupPermissions", new[] { "GroupId" });
            DropIndex("dbo.MenuItems", new[] { "RestaurantId" });
            DropIndex("dbo.DealMenuItems", new[] { "MenuItemId" });
            DropIndex("dbo.DealMenuItems", new[] { "DealId" });
            DropIndex("dbo.Deals", new[] { "RestaurantId" });
            DropIndex("dbo.Addresses", new[] { "RestaurantId" });
            DropTable("dbo.Permissions");
            DropTable("dbo.Users");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupPermissions");
            DropTable("dbo.MenuItems");
            DropTable("dbo.DealMenuItems");
            DropTable("dbo.Deals");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Addresses");
        }
    }
}
