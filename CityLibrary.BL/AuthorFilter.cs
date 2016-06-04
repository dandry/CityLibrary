using CityLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.BL
{
    public class AuthorFilter
    {
        IUnitOfWork uow;

        public AuthorFilter(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public object Autocomplete(string term)
        {
            var result = uow.BookRepository.Get(filter:
                b => b.Author.Contains(term))
                .GroupBy(b => b.Author)
                .Select(b => b.FirstOrDefault())
                .Take(10)
                .Select(b => new
                {
                    value = b.Author
                });

            return result;
        }
    }
}