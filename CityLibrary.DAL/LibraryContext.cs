using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CityLibrary.DAL
{
    public class LibraryContext : DbContext
    {
        public virtual DbSet<LibraryUser> LibraryUsers { get; set; }
        public DbSet<BookCollection> BookCollections { get; set; }
        public DbSet<Book> LibraryBooks { get; set; }

        public LibraryContext() : base("LibraryContext")
        {
        }

        public LibraryContext(string connString) : base(connString)
        {
        }
    }
}