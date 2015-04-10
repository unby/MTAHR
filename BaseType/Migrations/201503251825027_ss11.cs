namespace BaseType.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss11 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Notivications", new[] { "From_Id" });
            DropIndex("dbo.Notivications", new[] { "To_Id" });
            DropColumn("dbo.Notivications", "IdUserFrom");
            DropColumn("dbo.Notivications", "IdUserTo");
            RenameColumn(table: "dbo.Notivications", name: "From_Id", newName: "IdUserFrom");
            RenameColumn(table: "dbo.Notivications", name: "To_Id", newName: "IdUserTo");
            AlterColumn("dbo.Notivications", "IdUserFrom", c => c.Guid());
            AlterColumn("dbo.Notivications", "IdUserTo", c => c.Guid());
            CreateIndex("dbo.Notivications", "IdUserFrom");
            CreateIndex("dbo.Notivications", "IdUserTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Notivications", new[] { "IdUserTo" });
            DropIndex("dbo.Notivications", new[] { "IdUserFrom" });
            AlterColumn("dbo.Notivications", "IdUserTo", c => c.Guid(nullable: false));
            AlterColumn("dbo.Notivications", "IdUserFrom", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Notivications", name: "IdUserTo", newName: "To_Id");
            RenameColumn(table: "dbo.Notivications", name: "IdUserFrom", newName: "From_Id");
            AddColumn("dbo.Notivications", "IdUserTo", c => c.Guid(nullable: false));
            AddColumn("dbo.Notivications", "IdUserFrom", c => c.Guid(nullable: false));
            CreateIndex("dbo.Notivications", "To_Id");
            CreateIndex("dbo.Notivications", "From_Id");
        }
    }
}
