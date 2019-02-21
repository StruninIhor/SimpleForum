namespace DataContract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01_newModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Text = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        Text = c.String(),
                        Order = c.Int(nullable: false),
                        ReplyToCommentId = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Comments", t => t.ReplyToCommentId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId)
                .Index(t => t.ReplyToCommentId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ForumId = c.Int(nullable: false),
                        Name = c.String(),
                        Text = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Forums", t => t.ForumId, cascadeDelete: true)
                .Index(t => t.ForumId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Forums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        UserName = c.String(),
                        Status = c.String(),
                        EmailNotificationsEnabled = c.Boolean(nullable: false),
                        ForumNotificationsEnabled = c.Boolean(nullable: false),
                        SubscriptionEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Topics", "ForumId", "dbo.Forums");
            DropForeignKey("dbo.Forums", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Topics", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "ReplyToCommentId", "dbo.Comments");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.UserProfiles", new[] { "Id" });
            DropIndex("dbo.Forums", new[] { "AuthorId" });
            DropIndex("dbo.Topics", new[] { "AuthorId" });
            DropIndex("dbo.Topics", new[] { "ForumId" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "ReplyToCommentId" });
            DropIndex("dbo.Comments", new[] { "TopicId" });
            DropIndex("dbo.Articles", new[] { "AuthorId" });
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Forums");
            DropTable("dbo.Topics");
            DropTable("dbo.Comments");
            DropTable("dbo.Articles");
        }
    }
}
