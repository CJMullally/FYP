namespace FYPInitial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.AspNetUsers", "Address", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.AspNetUsers", "PhoneNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PhoneNo");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
    }
}
