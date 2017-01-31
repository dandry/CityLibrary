using CityLibrary.DAL;
using CityLibrary.DAL.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLibrary.Tests.FakeComponents
{
    class FakeUnitOfWork : IUnitOfWork
    {
        LibraryContext dummyContext = new LibraryContext("DummyDb");

        private GenericRepository<BookCollection> collectionRepository;
        private IdentityRepository<ApplicationUser> userRepository;
        private GenericRepository<Book> bookRepository;

        public GenericRepository<BookCollection> BookCollectionRepository
        {
            get
            {

                if (this.collectionRepository == null)
                {
                    this.collectionRepository = new GenericRepository<BookCollection>(dummyContext);
                }
                return collectionRepository;
            }
        }

        public IdentityRepository<ApplicationUser> UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new IdentityRepository<ApplicationUser>(new IdentityContext());
                }
                return userRepository;
            }
        }

        public GenericRepository<Book> BookRepository
        {
            get
            {

                if (this.bookRepository == null)
                {
                    this.bookRepository = new GenericRepository<Book>(dummyContext);
                }
                return bookRepository;
            }
        }

        public void Save()
        {
            dummyContext.SaveChanges();
        }
    }
}
