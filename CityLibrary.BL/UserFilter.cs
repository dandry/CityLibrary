using CityLibrary.DAL;
using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.BL
{
    public class UserFilter
    {
        IUnitOfWork uow;

        public UserFilter(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IEnumerable<LibraryUser> FilterByName(string userName, bool autocompleteSource)
        {
            IEnumerable<LibraryUser> result;

            if (autocompleteSource)
            {
                var userNameArray = userName.Split(' ');

                var lastName = userNameArray[0];
                var firstName = userNameArray[1];

                result = uow.LibraryUserRepository.Get(filter:
                    u => u.FirstName.Contains(firstName)
                    || u.LastName.Contains(lastName), orderBy:
                    q => q.OrderBy(u => u.LastName));
            }
            else
            {
                result = uow.LibraryUserRepository.Get(filter:
                    u => u.FirstName.Contains(userName)
                    || u.LastName.Contains(userName), orderBy:
                    q => q.OrderBy(u => u.LastName));
            }

            return result;
        }

        public object Autocomplete(string name)
        {
            var result = uow.LibraryUserRepository.Get(filter:
                u => u.FirstName.Contains(name)
                || u.LastName.Contains(name), orderBy:
                q => q.OrderBy(u => u.LastName))
                .Take(10)
                .Select(b => new
                {
                    value = b.LastName + " " + b.FirstName + ", " + b.PESEL
                });

            return result;
        }
    }
}