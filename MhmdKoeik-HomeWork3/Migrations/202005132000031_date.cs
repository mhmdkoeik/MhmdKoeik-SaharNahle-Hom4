namespace MhmdKoeik_HomeWork3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransactionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "TransactionDate");
        }
    }
}
