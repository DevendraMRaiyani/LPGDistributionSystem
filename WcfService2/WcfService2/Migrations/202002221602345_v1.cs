namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        DistributorCode = c.Int(nullable: false),
                        CustomerName = c.String(),
                        Gender = c.String(),
                        CustomerType = c.String(),
                        AadharNo = c.String(),
                        RashanCardNo = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        PinNo = c.Int(nullable: false),
                        Taluka = c.String(),
                        District = c.String(),
                        State = c.String(),
                        ContactNo = c.String(),
                        Email = c.String(),
                        BankIFSC = c.String(),
                        BankAccountNo = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Cylinders",
                c => new
                    {
                        type = c.String(nullable: false, maxLength: 128),
                        Quentity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.type);
            
            CreateTable(
                "dbo.DistributorUsers",
                c => new
                    {
                        DisId = c.Int(nullable: false, identity: true),
                        DistributorCode = c.Int(nullable: false),
                        AgencyName = c.String(),
                        DistributorName = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        PANnumber = c.String(),
                        GSTIN = c.String(),
                        LicenceIssueDate = c.DateTime(nullable: false),
                        LicenceExpiryDate = c.DateTime(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.DisId);
            
            CreateTable(
                "dbo.Stoves",
                c => new
                    {
                        type = c.String(nullable: false, maxLength: 128),
                        Quentity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.type);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stoves");
            DropTable("dbo.DistributorUsers");
            DropTable("dbo.Cylinders");
            DropTable("dbo.Customers");
        }
    }
}
