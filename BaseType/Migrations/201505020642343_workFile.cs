namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workFile : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TaskComments", name: "AuthorApplicationUser_Id", newName: "Author_Id");
            RenameIndex(table: "dbo.TaskComments", name: "IX_AuthorApplicationUser_Id", newName: "IX_Author_Id");
            CreateTable(
                "dbo.WorkFiles",
                c => new
                    {
                        FileId = c.Guid(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 254),
                        Comment = c.String(maxLength: 60),
                        DateCreate = c.DateTime(nullable: false),
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
            AddColumn("dbo.TaskMembers", "Participation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkFiles", "Catalog_IdTask", "dbo.Tasks");
            DropForeignKey("dbo.WorkFiles", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.WorkFiles", new[] { "Catalog_IdTask" });
            DropIndex("dbo.WorkFiles", new[] { "Author_Id" });
            DropColumn("dbo.TaskMembers", "Participation");
            DropTable("dbo.WorkFiles");
            RenameIndex(table: "dbo.TaskComments", name: "IX_Author_Id", newName: "IX_AuthorApplicationUser_Id");
            RenameColumn(table: "dbo.TaskComments", name: "Author_Id", newName: "AuthorApplicationUser_Id");
        }
    }
}
