namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Properties", "Project_IdProject", "dbo.Projects");
            DropIndex("dbo.Properties", new[] { "Project_IdProject" });
            DropColumn("dbo.Properties", "Project_IdProject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Properties", "Project_IdProject", c => c.Guid());
            CreateIndex("dbo.Properties", "Project_IdProject");
            AddForeignKey("dbo.Properties", "Project_IdProject", "dbo.Projects", "IdProject");
        }
    }
}
