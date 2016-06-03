using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private LibraryContext context = new LibraryContext();
        private GenericRepository<BookCollection> collectionRepository;
        private GenericRepository<LibraryUser> userRepository;
        private GenericRepository<Book> bookRepository;

        public GenericRepository<BookCollection> BookCollectionRepository
        {
            get
            {

                if (this.collectionRepository == null)
                {
                    this.collectionRepository = new GenericRepository<BookCollection>(context);
                }
                return collectionRepository;
            }
        }

        public GenericRepository<LibraryUser> LibraryUserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<LibraryUser>(context);
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
                    this.bookRepository = new GenericRepository<Book>(context);
                }
                return bookRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}