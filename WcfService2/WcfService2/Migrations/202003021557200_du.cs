namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class du : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistributorUsers", "City", c => c.String());
            AddColumn("dbo.DistributorUsers", "Taluka", c => c.String());
            AddColumn("dbo.DistributorUsers", "District", c => c.String());
            AddColumn("dbo.DistributorUsers", "State", c => c.String());
            AddColumn("dbo.DistributorUsers", "ContectNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistributorUsers", "ContectNo");
            DropColumn("dbo.DistributorUsers", "State");
            DropColumn("dbo.DistributorUsers", "District");
            DropColumn("dbo.DistributorUsers", "Taluka");
            DropColumn("dbo.DistributorUsers", "City");
        }
    }
}
