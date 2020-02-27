namespace WcfService2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TxCylinders", "CashMemoNo", c => c.Int(nullable: false));
            AddColumn("dbo.TxRegulators", "CashMemoNo", c => c.Int(nullable: false));
            AddColumn("dbo.TxStoves", "CashMemoNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TxStoves", "CashMemoNo");
            DropColumn("dbo.TxRegulators", "CashMemoNo");
            DropColumn("dbo.TxCylinders", "CashMemoNo");
        }
    }
}
