namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TaskComments");
            CreateTable(
                "dbo.Notivications",
                c => new
                    {
                        NotivicationId = c.Guid(nullable: false),
                        Description = c.String(nullable: false, maxLength: 350),
                        DateCreate = c.DateTime(nullable: false),
                        TimeSend = c.DateTime(nullable: false),
                        Author_IdUser = c.Guid(),
                        Task_IdTask = c.Guid(),
                    })
                .PrimaryKey(t => t.NotivicationId)
                .ForeignKey("dbo.Users", t => t.Author_IdUser)
                .ForeignKey("dbo.Tasks", t => t.Task_IdTask)
                .Index(t => t.Author_IdUser)
                .Index(t => t.Task_IdTask);
            
            CreateTable(
                "dbo.WorkGroups",
                c => new
                    {
                        WorkGroupId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.WorkGroupId)
                .ForeignKey("dbo.Tasks", t => t.WorkGroupId)
                .Index(t => t.WorkGroupId);
            
            AddColumn("dbo.Users", "WorkGroup_WorkGroupId", c => c.Guid());
            AddColumn("dbo.TaskComments", "TaskCommentId", c => c.Guid(nullable: false));
            AddColumn("dbo.TaskComments", "Task_IdTask", c => c.Guid());
            AddPrimaryKey("dbo.TaskComments", "TaskCommentId");
            CreateIndex("dbo.Users", "WorkGroup_WorkGroupId");
            CreateIndex("dbo.TaskComments", "Task_IdTask");
            AddForeignKey("dbo.TaskComments", "Task_IdTask", "dbo.Tasks", "IdTask");
            AddForeignKey("dbo.Users", "WorkGroup_WorkGroupId", "dbo.WorkGroups", "WorkGroupId");
            DropColumn("dbo.Tasks", "DateFisrtNotification");
            DropColumn("dbo.Tasks", "DateLastNotification");
            DropColumn("dbo.Tasks", "DateEndNotofication");
            DropColumn("dbo.TaskComments", "IdComment");
            DropColumn("dbo.TaskComments", "IdTask");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskComments", "IdTask", c => c.Guid(nullable: false));
            AddColumn("dbo.TaskComments", "IdComment", c => c.Guid(nullable: false));
            AddColumn("dbo.Tasks", "DateEndNotofication", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tasks", "DateLastNotification", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tasks", "DateFisrtNotification", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.WorkGroups", "WorkGroupId", "dbo.Tasks");
            DropForeignKey("dbo.Users", "WorkGroup_WorkGroupId", "dbo.WorkGroups");
            DropForeignKey("dbo.TaskComments", "Task_IdTask", "dbo.Tasks");
            DropForeignKey("dbo.Notivications", "Task_IdTask", "dbo.Tasks");
            DropForeignKey("dbo.Notivications", "Author_IdUser", "dbo.Users");
            DropIndex("dbo.WorkGroups", new[] { "WorkGroupId" });
            DropIndex("dbo.TaskComments", new[] { "Task_IdTask" });
            DropIndex("dbo.Notivications", new[] { "Task_IdTask" });
            DropIndex("dbo.Notivications", new[] { "Author_IdUser" });
            DropIndex("dbo.Users", new[] { "WorkGroup_WorkGroupId" });
            DropPrimaryKey("dbo.TaskComments");
            DropColumn("dbo.TaskComments", "Task_IdTask");
            DropColumn("dbo.TaskComments", "TaskCommentId");
            DropColumn("dbo.Users", "WorkGroup_WorkGroupId");
            DropTable("dbo.WorkGroups");
            DropTable("dbo.Notivications");
            AddPrimaryKey("dbo.TaskComments", new[] { "IdComment", "IdTask" });
        }
    }
}
