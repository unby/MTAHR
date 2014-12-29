namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appjournaledit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppJurnals", "MessageType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppJurnals", "MessageType");
        }
    }
}
