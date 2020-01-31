namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LPGDBv1 : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Regulators",
                c => new
                    {
                        Quentity = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Quentity);
            
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
            DropTable("dbo.Regulators");
            DropTable("dbo.Cylinders");
        }
    }
}
