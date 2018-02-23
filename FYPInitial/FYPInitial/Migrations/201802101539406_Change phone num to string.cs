namespace FYPInitial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changephonenumtostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNo", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNo", c => c.Int(nullable: false));
        }
    }
}
