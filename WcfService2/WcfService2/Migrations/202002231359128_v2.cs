namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GSTRates",
                c => new
                    {
                        Comodity = c.String(nullable: false, maxLength: 128),
                        CGST = c.Double(nullable: false),
                        SGST = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Comodity);
            
            CreateTable(
                "dbo.TxCylinders",
                c => new
                    {
                        TxId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CustomerName = c.String(),
                        TxDate = c.DateTime(nullable: false),
                        CylinderDetails = c.String(),
                        Quentity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        CGST = c.Double(nullable: false),
                        SGST = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TxId);
            
            CreateTable(
                "dbo.TxRegulators",
                c => new
                    {
                        TxId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CustomerName = c.String(),
                        TxDate = c.DateTime(nullable: false),
                        Details = c.String(),
                        Quentity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        CGST = c.Double(nullable: false),
                        SGST = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TxId);
            
            CreateTable(
                "dbo.TxStoves",
                c => new
                    {
                        TxId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CustomerName = c.String(),
                        TxDate = c.DateTime(nullable: false),
                        Details = c.String(),
                        Quentity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        CGST = c.Double(nullable: false),
                        SGST = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TxId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TxStoves");
            DropTable("dbo.TxRegulators");
            DropTable("dbo.TxCylinders");
            DropTable("dbo.GSTRates");
        }
    }
}
