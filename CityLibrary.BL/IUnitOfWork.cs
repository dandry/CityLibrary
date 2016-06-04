using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.DAL
{
    public interface IUnitOfWork
    {
        GenericRepository<BookCollection> BookCollectionRepository { get; }
        GenericRepository<LibraryUser> LibraryUserRepository { get; }
        GenericRepository<Book> BookRepository { get; }

        void Save();        
    }
}