namespace DataContract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01identity_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsBlocked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Topics", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Forums", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Articles", "CreateDate");
            DropColumn("dbo.Comments", "CreateDate");
            DropColumn("dbo.Topics", "CreateDate");
            DropColumn("dbo.Forums", "CreateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Forums", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Topics", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Articles", "CreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Forums", "CreatedDate");
            DropColumn("dbo.Topics", "CreatedDate");
            DropColumn("dbo.Comments", "CreatedDate");
            DropColumn("dbo.AspNetUsers", "IsBlocked");
            DropColumn("dbo.Articles", "CreatedDate");
        }
    }
}
