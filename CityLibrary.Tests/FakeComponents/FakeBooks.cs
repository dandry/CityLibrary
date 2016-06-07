using CityLibrary.DAL;
using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLibrary.Tests.FakeComponents
{
    public static class FakeBooks
    {
        public static IEnumerable<Book> GetBooks()
        {
            var books = new List<Book>();

            for (int i = 1; i<10; i++)
            {
                var book = new Book()
                {
                    Author = "Author" + i,
                    Title = "Title" + i,
                    CollectionId = i,
                    ISBN = "000-00-000-0000-" + i,
                    Publisher = "Publisher" + i,
                    YearPrinted = 2016,
                };

                if (i == 6)
                {
                    book.BorrowDate = DateTime.Now.AddMonths(-1).AddDays(-5);
                    book.ReturnDate = DateTime.Now.AddMonths(-1);
                }
                if (i > 6)
                {
                    book.BorrowDate = DateTime.Now;
                    book.ReturnDate = DateTime.Now.AddMonths(1).AddDays(-1);
                }

                books.Add(book);
            }

            return books;
        }
    }
}
