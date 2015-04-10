namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskComments", "Task_IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskComments", "AuthorUser_IdUser", "dbo.Users");
            DropIndex("dbo.TaskComments", new[] { "AuthorUser_IdUser" });
            DropIndex("dbo.TaskComments", new[] { "Task_IdTask" });
            AlterColumn("dbo.Tasks", "TaskRating", c => c.Int(nullable: false));
            AlterColumn("dbo.TaskComments", "Message", c => c.String(nullable: false));
            AlterColumn("dbo.TaskComments", "AuthorUser_IdUser", c => c.Guid(nullable: false));
            AlterColumn("dbo.TaskComments", "Task_IdTask", c => c.Guid(nullable: false));
            CreateIndex("dbo.TaskComments", "AuthorUser_IdUser");
            CreateIndex("dbo.TaskComments", "Task_IdTask");
            AddForeignKey("dbo.TaskComments", "Task_IdTask", "dbo.Tasks", "IdTask", cascadeDelete: true);
            AddForeignKey("dbo.TaskComments", "AuthorUser_IdUser", "dbo.Users", "IdUser", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskComments", "AuthorUser_IdUser", "dbo.Users");
            DropForeignKey("dbo.TaskComments", "Task_IdTask", "dbo.Tasks");
            DropIndex("dbo.TaskComments", new[] { "Task_IdTask" });
            DropIndex("dbo.TaskComments", new[] { "AuthorUser_IdUser" });
            AlterColumn("dbo.TaskComments", "Task_IdTask", c => c.Guid());
            AlterColumn("dbo.TaskComments", "AuthorUser_IdUser", c => c.Guid());
            AlterColumn("dbo.TaskComments", "Message", c => c.String());
            AlterColumn("dbo.Tasks", "TaskRating", c => c.Byte(nullable: false));
            CreateIndex("dbo.TaskComments", "Task_IdTask");
            CreateIndex("dbo.TaskComments", "AuthorUser_IdUser");
            AddForeignKey("dbo.TaskComments", "AuthorUser_IdUser", "dbo.Users", "IdUser");
            AddForeignKey("dbo.TaskComments", "Task_IdTask", "dbo.Tasks", "IdTask");
        }
    }
}
