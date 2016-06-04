using CityLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.BL
{
    public class PublisherFilter
    {
        IUnitOfWork uow;

        public PublisherFilter(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public object Autocomplete(string term)
        {
            var result = uow.BookRepository.Get(filter:
                b => b.Publisher.Contains(term))
                .GroupBy(b => b.Publisher)
                .Select(b => b.FirstOrDefault())
                .Take(10)
                .Select(b => new
                {
                    value = b.Publisher
                });

            return result;
        }
    }
}