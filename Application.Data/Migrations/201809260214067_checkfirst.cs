namespace Application.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkfirst : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryRequestLines", "BrandId", c => c.Long());
            AddColumn("dbo.DeliveryRequestLines", "ItemId", c => c.Long());
            CreateIndex("dbo.DeliveryRequestLines", "BrandId");
            CreateIndex("dbo.DeliveryRequestLines", "ItemId");
            AddForeignKey("dbo.DeliveryRequestLines", "BrandId", "dbo.Brands", "Id");
            AddForeignKey("dbo.DeliveryRequestLines", "ItemId", "dbo.Items", "Id");
            DropColumn("dbo.DeliveryRequestLines", "Brand");
            DropColumn("dbo.ExpectedReceiptLines", "BrandCode");
            DropColumn("dbo.ExpectedReceiptLines", "BrandName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExpectedReceiptLines", "BrandName", c => c.String());
            AddColumn("dbo.ExpectedReceiptLines", "BrandCode", c => c.String());
            AddColumn("dbo.DeliveryRequestLines", "Brand", c => c.String(maxLength: 100, unicode: false));
            DropForeignKey("dbo.DeliveryRequestLines", "ItemId", "dbo.Items");
            DropForeignKey("dbo.DeliveryRequestLines", "BrandId", "dbo.Brands");
            DropIndex("dbo.DeliveryRequestLines", new[] { "ItemId" });
            DropIndex("dbo.DeliveryRequestLines", new[] { "BrandId" });
            DropColumn("dbo.DeliveryRequestLines", "ItemId");
            DropColumn("dbo.DeliveryRequestLines", "BrandId");
        }
    }
}
