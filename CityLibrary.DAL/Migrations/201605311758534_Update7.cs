namespace CityLibrary.DAL.Migrations.LibraryMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LibraryUsers", "PostalCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LibraryUsers", "PostalCode");
        }
    }
}
