namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Access_tokens", name: "User_Id", newName: "Social_network_Id");
            RenameIndex(table: "dbo.Access_tokens", name: "IX_User_Id", newName: "IX_Social_network_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Access_tokens", name: "IX_Social_network_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Access_tokens", name: "Social_network_Id", newName: "User_Id");
        }
    }
}
