namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class syn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Synchroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableName = c.String(),
                        Operation = c.String(),
                        RecordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Synchroes");
        }
    }
}
