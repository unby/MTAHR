namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss4 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.WorkGroups", newName: "TaskMembers");
            DropForeignKey("dbo.Users", "WorkGroup_WorkGroupId", "dbo.WorkGroups");
            DropIndex("dbo.Users", new[] { "WorkGroup_WorkGroupId" });
            DropIndex("dbo.TaskMembers", new[] { "WorkGroupId" });
            RenameColumn(table: "dbo.TaskMembers", name: "WorkGroupId", newName: "Task_IdTask");
            DropPrimaryKey("dbo.TaskMembers");
            AddColumn("dbo.TaskMembers", "IdTask", c => c.Guid(nullable: false));
            AddColumn("dbo.TaskMembers", "UserId", c => c.Guid(nullable: false));
            AddColumn("dbo.TaskMembers", "LevelNotivication", c => c.Int(nullable: false));
            AddColumn("dbo.TaskMembers", "TaskRole", c => c.Int(nullable: false));
            AddColumn("dbo.TaskMembers", "Comment", c => c.String(maxLength: 80));
            AddColumn("dbo.TaskMembers", "User_IdUser", c => c.Guid());
            AlterColumn("dbo.TaskMembers", "Task_IdTask", c => c.Guid());
            AddPrimaryKey("dbo.TaskMembers", new[] { "IdTask", "UserId" });
            CreateIndex("dbo.TaskMembers", "Task_IdTask");
            CreateIndex("dbo.TaskMembers", "User_IdUser");
            AddForeignKey("dbo.TaskMembers", "User_IdUser", "dbo.Users", "IdUser");
            DropColumn("dbo.Users", "WorkGroup_WorkGroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "WorkGroup_WorkGroupId", c => c.Guid());
            DropForeignKey("dbo.TaskMembers", "User_IdUser", "dbo.Users");
            DropIndex("dbo.TaskMembers", new[] { "User_IdUser" });
            DropIndex("dbo.TaskMembers", new[] { "Task_IdTask" });
            DropPrimaryKey("dbo.TaskMembers");
            AlterColumn("dbo.TaskMembers", "Task_IdTask", c => c.Guid(nullable: false));
            DropColumn("dbo.TaskMembers", "User_IdUser");
            DropColumn("dbo.TaskMembers", "Comment");
            DropColumn("dbo.TaskMembers", "TaskRole");
            DropColumn("dbo.TaskMembers", "LevelNotivication");
            DropColumn("dbo.TaskMembers", "UserId");
            DropColumn("dbo.TaskMembers", "IdTask");
            AddPrimaryKey("dbo.TaskMembers", "WorkGroupId");
            RenameColumn(table: "dbo.TaskMembers", name: "Task_IdTask", newName: "WorkGroupId");
            CreateIndex("dbo.TaskMembers", "WorkGroupId");
            CreateIndex("dbo.Users", "WorkGroup_WorkGroupId");
            AddForeignKey("dbo.Users", "WorkGroup_WorkGroupId", "dbo.WorkGroups", "WorkGroupId");
            RenameTable(name: "dbo.TaskMembers", newName: "WorkGroups");
        }
    }
}
