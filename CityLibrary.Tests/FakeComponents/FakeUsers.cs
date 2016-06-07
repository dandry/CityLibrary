using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLibrary.Tests.FakeComponents
{
    public static class FakeUsers
    {
        public static IEnumerable<LibraryUser> GetUsers()
        {
            var users = new List<LibraryUser>();

            for (int i=1; i<10; i++)
            {
                var user = new LibraryUser()
                {
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    PESEL = Convert.ToInt64("1000000000" + i),
                    Email = "user" + i + "@testmail.pl",
                    Street = "Teststreet" + i,
                    ApartmentNumber = i,
                    PostalCode = "00-00" + i,
                    City = "TestCity",
                    DateOfBirth = DateTime.Parse("2005/05/05"),
                    RegistrationDate = DateTime.Now,
                };

                users.Add(user);
            }

            return users;
        }
    }
}
