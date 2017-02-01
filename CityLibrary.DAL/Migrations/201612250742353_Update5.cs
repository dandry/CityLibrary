namespace CityLibrary.DAL.Migrations.LibraryMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LibraryUsers", "PESEL", c => c.Int(nullable: false));
            AddColumn("dbo.LibraryUsers", "Email", c => c.String());
            AddColumn("dbo.LibraryUsers", "Street", c => c.String());
            AddColumn("dbo.LibraryUsers", "ApartmentNumber", c => c.Int(nullable: false));
            AddColumn("dbo.LibraryUsers", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LibraryUsers", "City");
            DropColumn("dbo.LibraryUsers", "ApartmentNumber");
            DropColumn("dbo.LibraryUsers", "Street");
            DropColumn("dbo.LibraryUsers", "Email");
            DropColumn("dbo.LibraryUsers", "PESEL");
        }
    }
}
