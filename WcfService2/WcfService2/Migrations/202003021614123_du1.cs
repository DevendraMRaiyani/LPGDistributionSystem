namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class du1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistributorUsers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistributorUsers", "Email");
        }
    }
}
