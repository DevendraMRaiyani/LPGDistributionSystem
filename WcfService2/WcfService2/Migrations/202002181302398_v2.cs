namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Cylinders");
            AddColumn("dbo.Cylinders", "CylenderType", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Cylinders", "CylenderType");
            DropColumn("dbo.Cylinders", "type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cylinders", "type", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Cylinders");
            DropColumn("dbo.Cylinders", "CylenderType");
            AddPrimaryKey("dbo.Cylinders", "type");
        }
    }
}
