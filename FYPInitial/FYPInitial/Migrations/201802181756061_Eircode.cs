namespace FYPInitial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eircode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Eircode", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Eircode");
        }
    }
}
