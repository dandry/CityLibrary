namespace CityLibrary.DAL.Migrations.LibraryMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookCollections", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BookCollections", "Name", c => c.String());
        }
    }
}
