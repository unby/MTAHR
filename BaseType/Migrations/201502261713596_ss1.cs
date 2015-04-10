namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppJurnals",
                c => new
                    {
                        IdEntry = c.Guid(nullable: false),
                        IdTask = c.Guid(nullable: false),
                        DateEntry = c.DateTime(nullable: false),
                        Message = c.String(nullable: false),
                        MessageType = c.Int(nullable: false),
                        MessageCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdEntry);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        IdProject = c.Guid(nullable: false),
                        Name = c.String(),
                        Comment = c.String(),
                        Purpose = c.String(),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        TypeProject = c.Int(nullable: false),
                        Author_IdUser = c.Guid(),
                    })
                .PrimaryKey(t => t.IdProject)
                .ForeignKey("dbo.Users", t => t.Author_IdUser)
                .Index(t => t.Author_IdUser);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Guid(nullable: false),
                        LoginName = c.String(maxLength: 50),
                        SID = c.Binary(maxLength: 600),
                        Authorization = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 150),
                        Surname = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        BirthDate = c.DateTime(),
                        PhoneNumber = c.Int(nullable: false),
                        IsWork = c.Boolean(nullable: false),
                        Comment = c.String(maxLength: 250),
                        SystemRole = c.Int(nullable: false),
                        Post = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.IdUser);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        IdProject = c.Guid(nullable: false),
                        IdUser = c.Guid(nullable: false),
                        Role = c.Int(nullable: false),
                        Comment = c.String(maxLength: 120),
                    })
                .PrimaryKey(t => new { t.IdProject, t.IdUser })
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdProject)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        IdTask = c.Guid(nullable: false),
                        Author = c.Guid(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateFinish = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateFisrtNotification = c.DateTime(nullable: false),
                        DateLastNotification = c.DateTime(nullable: false),
                        DateEndNotofication = c.DateTime(nullable: false),
                        DateClose = c.DateTime(nullable: false),
                        NameTask = c.String(nullable: false),
                        Result = c.String(),
                        Description = c.String(),
                        Comment = c.String(),
                        ParentTask = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        Performer_IdUser = c.Guid(nullable: false),
                        Project_IdProject = c.Guid(),
                    })
                .PrimaryKey(t => t.IdTask)
                .ForeignKey("dbo.Users", t => t.Performer_IdUser, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_IdProject)
                .Index(t => t.Performer_IdUser)
                .Index(t => t.Project_IdProject);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        IdProperty = c.Guid(nullable: false),
                        Value = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        TypeValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProperty);
            
            CreateTable(
                "dbo.TaskComments",
                c => new
                    {
                        IdComment = c.Guid(nullable: false),
                        IdTask = c.Guid(nullable: false),
                        Message = c.String(),
                        DateMessage = c.DateTime(nullable: false),
                        AuthorUser_IdUser = c.Guid(),
                    })
                .PrimaryKey(t => new { t.IdComment, t.IdTask })
                .ForeignKey("dbo.Users", t => t.AuthorUser_IdUser)
                .Index(t => t.AuthorUser_IdUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskComments", "AuthorUser_IdUser", "dbo.Users");
            DropForeignKey("dbo.Tasks", "Project_IdProject", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "Performer_IdUser", "dbo.Users");
            DropForeignKey("dbo.Projects", "Author_IdUser", "dbo.Users");
            DropForeignKey("dbo.Members", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Members", "IdProject", "dbo.Projects");
            DropIndex("dbo.TaskComments", new[] { "AuthorUser_IdUser" });
            DropIndex("dbo.Tasks", new[] { "Project_IdProject" });
            DropIndex("dbo.Tasks", new[] { "Performer_IdUser" });
            DropIndex("dbo.Members", new[] { "IdUser" });
            DropIndex("dbo.Members", new[] { "IdProject" });
            DropIndex("dbo.Projects", new[] { "Author_IdUser" });
            DropTable("dbo.TaskComments");
            DropTable("dbo.Properties");
            DropTable("dbo.Tasks");
            DropTable("dbo.Members");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.AppJurnals");
        }
    }
}
