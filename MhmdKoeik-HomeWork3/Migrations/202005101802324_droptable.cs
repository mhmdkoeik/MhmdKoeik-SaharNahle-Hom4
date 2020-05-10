namespace MhmdKoeik_HomeWork3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class droptable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transactions", "TransactionSource");
            DropColumn("dbo.Transactions", "TransactionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "TransactionDate", c => c.String());
            AddColumn("dbo.Transactions", "TransactionSource", c => c.String(nullable: false));
        }
    }
}
