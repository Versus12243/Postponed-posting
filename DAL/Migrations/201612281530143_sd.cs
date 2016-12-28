namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Social_network",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Access_tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Social_network", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Social_network", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Access_tokens", "User_Id", "dbo.Social_network");
            DropIndex("dbo.Access_tokens", new[] { "User_Id" });
            DropIndex("dbo.Social_network", new[] { "User_Id" });
            DropTable("dbo.Access_tokens");
            DropTable("dbo.Social_network");
        }
    }
}
