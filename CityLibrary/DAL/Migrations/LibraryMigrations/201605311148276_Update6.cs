namespace CityLibrary.DAL.Migrations.LibraryMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LibraryUsers", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.LibraryUsers", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.LibraryUsers", "PESEL", c => c.Long(nullable: false));
            AlterColumn("dbo.LibraryUsers", "ApartmentNumber", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LibraryUsers", "ApartmentNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.LibraryUsers", "PESEL", c => c.Int(nullable: false));
            AlterColumn("dbo.LibraryUsers", "FirstName", c => c.String());
            AlterColumn("dbo.LibraryUsers", "LastName", c => c.String());
        }
    }
}
