namespace FYPInitial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilePaths : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilePaths",
                c => new
                    {
                        FilePathId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255, storeType: "nvarchar"),
                        FileType = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.FilePathId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilePaths", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.FilePaths", new[] { "UserID" });
            DropTable("dbo.FilePaths");
        }
    }
}
