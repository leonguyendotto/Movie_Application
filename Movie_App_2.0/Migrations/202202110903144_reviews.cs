namespace Movie_App_2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        ReviewTitle = c.String(),
                        AlreadyPosted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reviews");
        }
    }
}
