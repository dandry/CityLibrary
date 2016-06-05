namespace CityLibrary.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebDeploy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookCollections",
                c => new
                    {
                        CollectionId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CollectionId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        ISBN = c.String(nullable: false),
                        Publisher = c.String(nullable: false),
                        YearPrinted = c.Int(nullable: false),
                        UserId = c.Int(),
                        CollectionId = c.Int(nullable: false),
                        BorrowDate = c.DateTime(),
                        ReturnDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.BookCollections", t => t.CollectionId, cascadeDelete: true)
                .ForeignKey("dbo.LibraryUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CollectionId);
            
            CreateTable(
                "dbo.LibraryUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        PESEL = c.Long(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Email = c.String(),
                        Street = c.String(nullable: false),
                        ApartmentNumber = c.Int(),
                        PostalCode = c.String(nullable: false),
                        City = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "UserId", "dbo.LibraryUsers");
            DropForeignKey("dbo.Books", "CollectionId", "dbo.BookCollections");
            DropIndex("dbo.Books", new[] { "CollectionId" });
            DropIndex("dbo.Books", new[] { "UserId" });
            DropTable("dbo.LibraryUsers");
            DropTable("dbo.Books");
            DropTable("dbo.BookCollections");
        }
    }
}
