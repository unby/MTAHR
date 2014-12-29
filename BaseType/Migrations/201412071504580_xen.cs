namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Post", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "TabelNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "TabelNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Users", "Post");
        }
    }
}
