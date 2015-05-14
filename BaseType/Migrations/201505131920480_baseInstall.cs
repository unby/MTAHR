namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class baseInstall : DbMigration
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
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 200),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notivications",
                c => new
                    {
                        IdNotivication = c.Guid(nullable: false),
                        Description = c.String(nullable: false, maxLength: 350),
                        DateCreate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TimeSend = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IdTask = c.Guid(nullable: false),
                        IdUserFrom = c.Guid(),
                        IdUserTo = c.Guid(),
                        NotivicationStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdNotivication)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUserFrom)
                .ForeignKey("dbo.Tasks", t => t.IdTask, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUserTo)
                .Index(t => t.IdTask)
                .Index(t => t.IdUserFrom)
                .Index(t => t.IdUserTo);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 256),
                        Surname = c.String(nullable: false, maxLength: 30),
                        Name = c.String(nullable: false, maxLength: 25),
                        MiddleName = c.String(maxLength: 30),
                        BirthDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        PhoneNumber = c.String(maxLength: 20),
                        IsWork = c.Boolean(nullable: false),
                        Comment = c.String(maxLength: 250),
                        Post = c.String(maxLength: 60),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        PasswordHash = c.String(maxLength: 800),
                        SecurityStamp = c.String(maxLength: 200),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "UK_Email")
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdProject)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        IdProject = c.Guid(nullable: false),
                        Name = c.String(maxLength: 70),
                        Comment = c.String(maxLength: 90),
                        Purpose = c.String(maxLength: 600),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateUpdate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TypeProject = c.Int(nullable: false),
                        Author_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.IdProject)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Name, unique: true, name: "UK_ProjectName")
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        IdTask = c.Guid(nullable: false),
                        Author = c.Guid(nullable: false),
                        Project = c.Guid(nullable: false),
                        DateCreate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateFinish = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateUpdate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateClose = c.DateTime(precision: 7, storeType: "datetime2"),
                        TaskRating = c.Int(nullable: false),
                        NameTask = c.String(nullable: false, maxLength: 160),
                        Result = c.String(maxLength: 300),
                        Description = c.String(maxLength: 700),
                        Comment = c.String(maxLength: 600),
                        ParentTask = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        Project_IdProject = c.Guid(),
                    })
                .PrimaryKey(t => t.IdTask)
                .ForeignKey("dbo.Projects", t => t.Project_IdProject)
                .Index(t => t.Project_IdProject);
            
            CreateTable(
                "dbo.WorkFiles",
                c => new
                    {
                        FileId = c.Guid(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 254),
                        Comment = c.String(maxLength: 60),
                        DateCreate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Size = c.Int(nullable: false),
                        Author_Id = c.Guid(),
                        Catalog_IdTask = c.Guid(),

                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Tasks", t => t.Catalog_IdTask)
                .Index(t => t.Author_Id)
                .Index(t => t.Catalog_IdTask);
            Sql("alter table [dbo].[WorkFiles] ALTER COLUMN [FileId] add rowguidcol ");
            Sql("alter table [dbo].[WorkFiles] ADD [Data] varbinary(max) FILESTREAM NOT NULL");
            CreateTable(
                "dbo.TaskComments",
                c => new
                    {
                        TaskCommentId = c.Guid(nullable: false),
                        Message = c.String(nullable: false),
                        DateMessage = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Author_Id = c.Guid(nullable: false),
                        Task_IdTask = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TaskCommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.Task_IdTask, cascadeDelete: true)
                .Index(t => t.Author_Id)
                .Index(t => t.Task_IdTask);
            
            CreateTable(
                "dbo.TaskMembers",
                c => new
                    {
                        IdTask = c.Guid(nullable: false),
                        IdUser = c.Guid(nullable: false),
                        Participation = c.Int(nullable: false),
                        LevelNotivication = c.Int(nullable: false),
                        TaskRole = c.Int(nullable: false),
                        Comment = c.String(maxLength: 80),
                    })
                .PrimaryKey(t => new { t.IdTask, t.IdUser })
                .ForeignKey("dbo.Tasks", t => t.IdTask, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdTask)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Notivications", "IdUserTo", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notivications", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.Notivications", "IdUserFrom", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Members", "IdUser", "dbo.AspNetUsers");
            DropForeignKey("dbo.Members", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "Project_IdProject", "dbo.Projects");
            DropForeignKey("dbo.TaskMembers", "IdUser", "dbo.AspNetUsers");
            DropForeignKey("dbo.TaskMembers", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskComments", "Task_IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskComments", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.WorkFiles", "Catalog_IdTask", "dbo.Tasks");
            DropForeignKey("dbo.WorkFiles", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.TaskMembers", new[] { "IdUser" });
            DropIndex("dbo.TaskMembers", new[] { "IdTask" });
            DropIndex("dbo.TaskComments", new[] { "Task_IdTask" });
            DropIndex("dbo.TaskComments", new[] { "Author_Id" });
            DropIndex("dbo.WorkFiles", new[] { "Catalog_IdTask" });
            DropIndex("dbo.WorkFiles", new[] { "Author_Id" });
            DropIndex("dbo.Tasks", new[] { "Project_IdProject" });
            DropIndex("dbo.Projects", new[] { "Author_Id" });
            DropIndex("dbo.Projects", "UK_ProjectName");
            DropIndex("dbo.Members", new[] { "IdUser" });
            DropIndex("dbo.Members", new[] { "IdProject" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", "UK_Email");
            DropIndex("dbo.Notivications", new[] { "IdUserTo" });
            DropIndex("dbo.Notivications", new[] { "IdUserFrom" });
            DropIndex("dbo.Notivications", new[] { "IdTask" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.TaskMembers");
            DropTable("dbo.TaskComments");
            DropTable("dbo.WorkFiles");
            DropTable("dbo.Tasks");
            DropTable("dbo.Projects");
            DropTable("dbo.Members");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Notivications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AppJurnals");
        }
    }
}
