namespace Phonix.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImagePathAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Phones", "CoverImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Phones", "CoverImagePath");
        }
    }
}
