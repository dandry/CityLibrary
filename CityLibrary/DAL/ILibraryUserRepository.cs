using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.DAL
{
    public interface ILibraryUserRepository : IDisposable
    {
        IEnumerable<LibraryUser> GetUsers();
        LibraryUser GetUserById(int userId);
        void InsertUser(LibraryUser user);
        void UpdateUser(LibraryUser user);
        void DeleteUser(int userId);
        void Save();
    }
}