namespace Movie_App_2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewersmovies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviewers",
                c => new
                    {
                        ReviewerID = c.Int(nullable: false, identity: true),
                        ReviewerFirstName = c.String(),
                        ReviewerLastName = c.String(),
                    })
                .PrimaryKey(t => t.ReviewerID);
            
            CreateTable(
                "dbo.ReviewerMovies",
                c => new
                    {
                        Reviewer_ReviewerID = c.Int(nullable: false),
                        Movie_MovieID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reviewer_ReviewerID, t.Movie_MovieID })
                .ForeignKey("dbo.Reviewers", t => t.Reviewer_ReviewerID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID, cascadeDelete: true)
                .Index(t => t.Reviewer_ReviewerID)
                .Index(t => t.Movie_MovieID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewerMovies", "Movie_MovieID", "dbo.Movies");
            DropForeignKey("dbo.ReviewerMovies", "Reviewer_ReviewerID", "dbo.Reviewers");
            DropIndex("dbo.ReviewerMovies", new[] { "Movie_MovieID" });
            DropIndex("dbo.ReviewerMovies", new[] { "Reviewer_ReviewerID" });
            DropTable("dbo.ReviewerMovies");
            DropTable("dbo.Reviewers");
        }
    }
}
