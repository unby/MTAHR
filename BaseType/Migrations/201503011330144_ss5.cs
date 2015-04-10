namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notivications", "Author_IdUser", "dbo.Users");
            DropForeignKey("dbo.Tasks", "Performer_IdUser", "dbo.Users");
            DropForeignKey("dbo.TaskMembers", "User_IdUser", "dbo.Users");
            DropForeignKey("dbo.TaskMembers", "Task_IdTask", "dbo.Tasks");
            DropForeignKey("dbo.Notivications", "Task_IdTask", "dbo.Tasks");
            DropIndex("dbo.TaskMembers", new[] { "Task_IdTask" });
            DropIndex("dbo.TaskMembers", new[] { "User_IdUser" });
            DropIndex("dbo.Tasks", new[] { "Performer_IdUser" });
            DropIndex("dbo.Notivications", new[] { "Author_IdUser" });
            DropIndex("dbo.Notivications", new[] { "Task_IdTask" });
            RenameColumn(table: "dbo.TaskMembers", name: "User_IdUser", newName: "IdUser");
            RenameColumn(table: "dbo.TaskMembers", name: "Task_IdTask", newName: "IdTask");
            RenameColumn(table: "dbo.Notivications", name: "Task_IdTask", newName: "IdTask");
            DropPrimaryKey("dbo.TaskMembers");
            DropPrimaryKey("dbo.Notivications");
            AddColumn("dbo.Tasks", "TaskRating", c => c.Byte(nullable: false));
            AddColumn("dbo.Notivications", "IdNotivication", c => c.Guid(nullable: false));
            AddColumn("dbo.Notivications", "IdUserFrom", c => c.Guid(nullable: false));
            AddColumn("dbo.Notivications", "IdUserTo", c => c.Guid(nullable: false));
            AlterColumn("dbo.TaskMembers", "IdTask", c => c.Guid(nullable: false));
            AlterColumn("dbo.TaskMembers", "IdUser", c => c.Guid(nullable: false));
            AlterColumn("dbo.Notivications", "IdTask", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.TaskMembers", new[] { "IdTask", "IdUser" });
            AddPrimaryKey("dbo.Notivications", "IdNotivication");
            CreateIndex("dbo.Notivications", "IdTask");
            CreateIndex("dbo.TaskMembers", "IdTask");
            CreateIndex("dbo.TaskMembers", "IdUser");
            AddForeignKey("dbo.TaskMembers", "IdUser", "dbo.Users", "IdUser", cascadeDelete: true);
            AddForeignKey("dbo.TaskMembers", "IdTask", "dbo.Tasks", "IdTask", cascadeDelete: true);
            AddForeignKey("dbo.Notivications", "IdTask", "dbo.Tasks", "IdTask", cascadeDelete: true);
            DropColumn("dbo.TaskMembers", "TaskId");
            DropColumn("dbo.TaskMembers", "UserId");
            DropColumn("dbo.Tasks", "Performer_IdUser");
            DropColumn("dbo.Notivications", "NotivicationId");
            DropColumn("dbo.Notivications", "Author_IdUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notivications", "Author_IdUser", c => c.Guid());
            AddColumn("dbo.Notivications", "NotivicationId", c => c.Guid(nullable: false));
            AddColumn("dbo.Tasks", "Performer_IdUser", c => c.Guid(nullable: false));
            AddColumn("dbo.TaskMembers", "UserId", c => c.Guid(nullable: false));
            AddColumn("dbo.TaskMembers", "TaskId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Notivications", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskMembers", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskMembers", "IdUser", "dbo.Users");
            DropIndex("dbo.TaskMembers", new[] { "IdUser" });
            DropIndex("dbo.TaskMembers", new[] { "IdTask" });
            DropIndex("dbo.Notivications", new[] { "IdTask" });
            DropPrimaryKey("dbo.Notivications");
            DropPrimaryKey("dbo.TaskMembers");
            AlterColumn("dbo.Notivications", "IdTask", c => c.Guid());
            AlterColumn("dbo.TaskMembers", "IdUser", c => c.Guid());
            AlterColumn("dbo.TaskMembers", "IdTask", c => c.Guid());
            DropColumn("dbo.Notivications", "IdUserTo");
            DropColumn("dbo.Notivications", "IdUserFrom");
            DropColumn("dbo.Notivications", "IdNotivication");
            DropColumn("dbo.Tasks", "TaskRating");
            AddPrimaryKey("dbo.Notivications", "NotivicationId");
            AddPrimaryKey("dbo.TaskMembers", new[] { "TaskId", "UserId" });
            RenameColumn(table: "dbo.Notivications", name: "IdTask", newName: "Task_IdTask");
            RenameColumn(table: "dbo.TaskMembers", name: "IdTask", newName: "Task_IdTask");
            RenameColumn(table: "dbo.TaskMembers", name: "IdUser", newName: "User_IdUser");
            CreateIndex("dbo.Notivications", "Task_IdTask");
            CreateIndex("dbo.Notivications", "Author_IdUser");
            CreateIndex("dbo.Tasks", "Performer_IdUser");
            CreateIndex("dbo.TaskMembers", "User_IdUser");
            CreateIndex("dbo.TaskMembers", "Task_IdTask");
            AddForeignKey("dbo.Notivications", "Task_IdTask", "dbo.Tasks", "IdTask");
            AddForeignKey("dbo.TaskMembers", "Task_IdTask", "dbo.Tasks", "IdTask");
            AddForeignKey("dbo.TaskMembers", "User_IdUser", "dbo.Users", "IdUser");
            AddForeignKey("dbo.Tasks", "Performer_IdUser", "dbo.Users", "IdUser", cascadeDelete: true);
            AddForeignKey("dbo.Notivications", "Author_IdUser", "dbo.Users", "IdUser");
        }
    }
}
