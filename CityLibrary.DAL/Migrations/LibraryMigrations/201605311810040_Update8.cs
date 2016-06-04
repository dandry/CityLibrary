namespace CityLibrary.DAL.Migrations.LibraryMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LibraryUsers", "Street", c => c.String(nullable: false));
            AlterColumn("dbo.LibraryUsers", "PostalCode", c => c.String(nullable: false));
            AlterColumn("dbo.LibraryUsers", "City", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LibraryUsers", "City", c => c.String());
            AlterColumn("dbo.LibraryUsers", "PostalCode", c => c.String());
            AlterColumn("dbo.LibraryUsers", "Street", c => c.String());
        }
    }
}
