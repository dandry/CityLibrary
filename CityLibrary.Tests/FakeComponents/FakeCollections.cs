using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLibrary.Tests.FakeComponents
{
    public static class FakeCollections
    {
        public static IEnumerable<BookCollection> GetCollections()
        {
            var collections = new List<BookCollection>();

            for (int i = 1; i < 10; i++)
            {
                var collection = new BookCollection()
                {
                    Name = "Collection" + i
                };

                collections.Add(collection);
            }

            return collections;
        }
    }
}
