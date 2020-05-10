namespace MhmdKoeik_HomeWork3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransactionSource", c => c.String(nullable: false));
            AddColumn("dbo.Transactions", "TransactionDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "TransactionDate");
            DropColumn("dbo.Transactions", "TransactionSource");
        }
    }
}
