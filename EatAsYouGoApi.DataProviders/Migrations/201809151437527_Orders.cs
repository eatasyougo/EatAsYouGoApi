namespace EatAsYouGoApi.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailsId = c.Long(nullable: false, identity: true),
                        OrderId = c.Long(nullable: false),
                        DealId = c.Long(nullable: false),
                        VoucherCode = c.String(),
                        VoucherRedeemed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailsId)
                .ForeignKey("dbo.Deals", t => t.DealId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.DealId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        RestaurantId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        OrderStatus = c.String(),
                        PaymentStatus = c.String(),
                        PaymentToken = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RestaurantId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrderDetailsMenuItems",
                c => new
                    {
                        OrderDetails_OrderDetailsId = c.Long(nullable: false),
                        MenuItem_MenuItemId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderDetails_OrderDetailsId, t.MenuItem_MenuItemId })
                .ForeignKey("dbo.OrderDetails", t => t.OrderDetails_OrderDetailsId, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuItem_MenuItemId, cascadeDelete: true)
                .Index(t => t.OrderDetails_OrderDetailsId)
                .Index(t => t.MenuItem_MenuItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.OrderDetailsMenuItems", "MenuItem_MenuItemId", "dbo.MenuItems");
            DropForeignKey("dbo.OrderDetailsMenuItems", "OrderDetails_OrderDetailsId", "dbo.OrderDetails");
            DropForeignKey("dbo.OrderDetails", "DealId", "dbo.Deals");
            DropIndex("dbo.OrderDetailsMenuItems", new[] { "MenuItem_MenuItemId" });
            DropIndex("dbo.OrderDetailsMenuItems", new[] { "OrderDetails_OrderDetailsId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "RestaurantId" });
            DropIndex("dbo.OrderDetails", new[] { "DealId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropTable("dbo.OrderDetailsMenuItems");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
        }
    }
}
