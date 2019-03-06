namespace Phonix.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Phonepropertydeleted : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Phones", "CoverImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Phones", "CoverImagePath", c => c.String());
        }
    }
}
