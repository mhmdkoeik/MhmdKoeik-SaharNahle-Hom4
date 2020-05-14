namespace MhmdKoeik_HomeWork3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Source : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Source");
        }
    }
}
