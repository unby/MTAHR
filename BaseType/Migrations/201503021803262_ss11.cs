namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notivications", "NotivicationStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Surname", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Users", "MiddleName", c => c.String(maxLength: 30));
            AlterColumn("dbo.Users", "PhoneNumber", c => c.String(maxLength: 20));
            AlterColumn("dbo.Projects", "Name", c => c.String(maxLength: 70));
            AlterColumn("dbo.Projects", "Comment", c => c.String(maxLength: 90));
            AlterColumn("dbo.Projects", "Purpose", c => c.String(maxLength: 600));
            AlterColumn("dbo.Tasks", "DateClose", c => c.DateTime());
            AlterColumn("dbo.Tasks", "NameTask", c => c.String(nullable: false, maxLength: 160));
            AlterColumn("dbo.Tasks", "Result", c => c.String(maxLength: 300));
            AlterColumn("dbo.Tasks", "Description", c => c.String(maxLength: 700));
            AlterColumn("dbo.Tasks", "Comment", c => c.String(maxLength: 600));
            CreateIndex("dbo.Users", "Email", unique: true, name: "UK_Email");
            CreateIndex("dbo.Projects", "Name", unique: true, name: "UK_ProjectName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projects", "UK_ProjectName");
            DropIndex("dbo.Users", "UK_Email");
            AlterColumn("dbo.Tasks", "Comment", c => c.String());
            AlterColumn("dbo.Tasks", "Description", c => c.String());
            AlterColumn("dbo.Tasks", "Result", c => c.String());
            AlterColumn("dbo.Tasks", "NameTask", c => c.String(nullable: false));
            AlterColumn("dbo.Tasks", "DateClose", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "Purpose", c => c.String());
            AlterColumn("dbo.Projects", "Comment", c => c.String());
            AlterColumn("dbo.Projects", "Name", c => c.String());
            AlterColumn("dbo.Users", "PhoneNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "MiddleName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "Surname", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Notivications", "NotivicationStatus");
        }
    }
}
