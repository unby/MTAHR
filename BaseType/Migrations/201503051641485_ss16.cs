namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Project", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Project");
        }
    }
}
