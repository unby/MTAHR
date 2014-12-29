namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRoles", "IdGroup", "dbo.WorkGroups");
            DropIndex("dbo.UserRoles", new[] { "IdGroup" });
            RenameColumn(table: "dbo.UserRoles", name: "IdGroup", newName: "Group_IdGroup");
            DropPrimaryKey("dbo.UserRoles");
            AddColumn("dbo.UserRoles", "IdUserRole", c => c.Guid(nullable: false));
            AddColumn("dbo.UserRoles", "User_IdUser", c => c.Guid());
            AlterColumn("dbo.UserRoles", "Group_IdGroup", c => c.Guid());
            AddPrimaryKey("dbo.UserRoles", "IdUserRole");
            CreateIndex("dbo.UserRoles", "Group_IdGroup");
            CreateIndex("dbo.UserRoles", "User_IdUser");
            AddForeignKey("dbo.UserRoles", "User_IdUser", "dbo.Users", "IdUser");
            AddForeignKey("dbo.UserRoles", "Group_IdGroup", "dbo.WorkGroups", "IdGroup");
            DropColumn("dbo.UserRoles", "IdUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRoles", "IdUser", c => c.Guid(nullable: false));
            DropForeignKey("dbo.UserRoles", "Group_IdGroup", "dbo.WorkGroups");
            DropForeignKey("dbo.UserRoles", "User_IdUser", "dbo.Users");
            DropIndex("dbo.UserRoles", new[] { "User_IdUser" });
            DropIndex("dbo.UserRoles", new[] { "Group_IdGroup" });
            DropPrimaryKey("dbo.UserRoles");
            AlterColumn("dbo.UserRoles", "Group_IdGroup", c => c.Guid(nullable: false));
            DropColumn("dbo.UserRoles", "User_IdUser");
            DropColumn("dbo.UserRoles", "IdUserRole");
            AddPrimaryKey("dbo.UserRoles", new[] { "IdUser", "IdGroup" });
            RenameColumn(table: "dbo.UserRoles", name: "Group_IdGroup", newName: "IdGroup");
            CreateIndex("dbo.UserRoles", "IdGroup");
            AddForeignKey("dbo.UserRoles", "IdGroup", "dbo.WorkGroups", "IdGroup", cascadeDelete: true);
        }
    }
}
