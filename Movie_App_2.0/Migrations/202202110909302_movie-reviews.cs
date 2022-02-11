namespace Movie_App_2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moviereviews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "ReviewID", c => c.Int(nullable: false));
            CreateIndex("dbo.Movies", "ReviewID");
            AddForeignKey("dbo.Movies", "ReviewID", "dbo.Reviews", "ReviewID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "ReviewID", "dbo.Reviews");
            DropIndex("dbo.Movies", new[] { "ReviewID" });
            DropColumn("dbo.Movies", "ReviewID");
        }
    }
}
