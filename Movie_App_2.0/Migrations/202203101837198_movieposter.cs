namespace Movie_App_2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movieposter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "MovieHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movies", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "PicExtension");
            DropColumn("dbo.Movies", "MovieHasPic");
        }
    }
}
