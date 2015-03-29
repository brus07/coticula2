namespace Coticula2Face.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSubmit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Submits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        VerdictID = c.Int(nullable: false),
                        ProblemID = c.Int(nullable: false),
                        LanguageID = c.Int(nullable: false),
                        Solution = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageID, cascadeDelete: true)
                .ForeignKey("dbo.Problems", t => t.ProblemID, cascadeDelete: true)
                .ForeignKey("dbo.Verdicts", t => t.VerdictID, cascadeDelete: true)
                .Index(t => t.VerdictID)
                .Index(t => t.ProblemID)
                .Index(t => t.LanguageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Submits", "VerdictID", "dbo.Verdicts");
            DropForeignKey("dbo.Submits", "ProblemID", "dbo.Problems");
            DropForeignKey("dbo.Submits", "LanguageID", "dbo.Languages");
            DropIndex("dbo.Submits", new[] { "LanguageID" });
            DropIndex("dbo.Submits", new[] { "ProblemID" });
            DropIndex("dbo.Submits", new[] { "VerdictID" });
            DropTable("dbo.Submits");
        }
    }
}
