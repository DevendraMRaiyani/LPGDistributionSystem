namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CylCustMappings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CylenderType = c.String(),
                        CustomerType = c.String(),
                        NoCylender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CylCustMappings");
        }
    }
}
