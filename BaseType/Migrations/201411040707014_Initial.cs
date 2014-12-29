namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        Author_IdUser = c.Guid(),
                        ProjectGroup_IdGroup = c.Guid(),
                    })
                .PrimaryKey(t => t.IdProject)
                .ForeignKey("dbo.Users", t => t.Author_IdUser)
                .ForeignKey("dbo.WorkGroups", t => t.ProjectGroup_IdGroup)
                .Index(t => t.Author_IdUser)
                .Index(t => t.ProjectGroup_IdGroup);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 16),
                        Password = c.String(nullable: false, maxLength: 255),
                        Surname = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        MiddleName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        TabelNumber = c.Int(nullable: false),
                        IsWork = c.Boolean(nullable: false),
                        Comment = c.String(),
                        SystemRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdUser);
            
            CreateTable(
                "dbo.WorkGroups",
                c => new
                    {
                        IdGroup = c.Guid(nullable: false),
                        Comment = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.IdGroup);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        IdUser = c.Guid(nullable: false),
                        IdGroup = c.Guid(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdUser, t.IdGroup })
                .ForeignKey("dbo.WorkGroups", t => t.IdGroup, cascadeDelete: true)
                .Index(t => t.IdGroup);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        IdProperty = c.Guid(nullable: false),
                        Value = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        TypeValue = c.Int(nullable: false),
                        Project_IdProject = c.Guid(),
                    })
                .PrimaryKey(t => t.IdProperty)
                .ForeignKey("dbo.Projects", t => t.Project_IdProject)
                .Index(t => t.Project_IdProject);
            
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
            DropForeignKey("dbo.Properties", "Project_IdProject", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ProjectGroup_IdGroup", "dbo.WorkGroups");
            DropForeignKey("dbo.UserRoles", "IdGroup", "dbo.WorkGroups");
            DropForeignKey("dbo.Projects", "Author_IdUser", "dbo.Users");
            DropIndex("dbo.TaskComments", new[] { "AuthorUser_IdUser" });
            DropIndex("dbo.Tasks", new[] { "Project_IdProject" });
            DropIndex("dbo.Tasks", new[] { "Performer_IdUser" });
            DropIndex("dbo.Properties", new[] { "Project_IdProject" });
            DropIndex("dbo.UserRoles", new[] { "IdGroup" });
            DropIndex("dbo.Projects", new[] { "ProjectGroup_IdGroup" });
            DropIndex("dbo.Projects", new[] { "Author_IdUser" });
            DropTable("dbo.TaskComments");
            DropTable("dbo.Tasks");
            DropTable("dbo.Properties");
            DropTable("dbo.UserRoles");
            DropTable("dbo.WorkGroups");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.AppJurnals");
        }
    }
}
