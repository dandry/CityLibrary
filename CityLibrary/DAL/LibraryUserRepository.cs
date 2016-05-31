using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CityLibrary.Models.Library;
using System.Data.Entity;

namespace CityLibrary.DAL
{
    public class LibraryUserRepository : ILibraryUserRepository, IDisposable
    {
        private LibraryContext _context;

        public LibraryUserRepository(LibraryContext context)
        {
            this._context = context;
        }

        public IEnumerable<LibraryUser> GetUsers()
        {
            return _context.LibraryUsers.ToList();
        }

        public LibraryUser GetUserById(int userId)
        {
            return _context.LibraryUsers.Find(userId);
        }

        public void InsertUser(LibraryUser user)
        {
            _context.LibraryUsers.Add(user);
        }

        public void UpdateUser(LibraryUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public void DeleteUser(int userId)
        {
            LibraryUser user = _context.LibraryUsers.Find(userId);

            _context.LibraryUsers.Remove(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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