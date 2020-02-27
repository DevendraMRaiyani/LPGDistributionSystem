namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v7 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TxStoves", newName: "TxStoveRegulators");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TxStoveRegulators", newName: "TxStoves");
        }
    }
}
