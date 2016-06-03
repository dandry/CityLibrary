using CityLibrary.DAL;
using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.BL
{
    public class BookFilter
    {
        IUnitOfWork uow;

        public BookFilter(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IEnumerable<Book> FilterByType(int collectionId, BookType type, string search)
        {
            IEnumerable<Book> result;

            if (type == BookType.Available)
            {
                result = uow.BookRepository.Get(filter:
                        b => b.CollectionId.Equals(collectionId) 
                        && b.UserId == null
                        && b.Title.Contains(search));
            }
            else if (type == BookType.Borrowed)
            {
                result = uow.BookRepository.Get(filter:
                        b => b.CollectionId.Equals(collectionId)
                        && b.UserId != null
                        && b.Title.Contains(search));
            }
            else //all books
            {
                result = uow.BookRepository.Get(filter:
                        b => b.CollectionId.Equals(collectionId)
                        && b.Title.Contains(search));
            }

            return result;
        }

        public IEnumerable<Book> FilterByType(BookType type, string search)
        {
            IEnumerable<Book> result;

            if (type == BookType.Available)
            {
                result = uow.BookRepository.Get(filter:
                        b => b.UserId == null
                        && b.Title.Contains(search));
            }
            else if (type == BookType.Borrowed)
            {
                result = uow.BookRepository.Get(filter:
                        b => b.UserId != null
                        && b.Title.Contains(search));
            }
            else //all books
            {
                result = uow.BookRepository.Get(filter:
                        b => b.Title.Contains(search));
            }

            return result;
        }

        public IEnumerable<Book> FilterByAutocomplete(string bookDetails, bool autocompleteSource = false)
        {
            IEnumerable<Book> books;

            if (autocompleteSource)
            {
                // autocomplete submits the form
                var bookDetailsArray = bookDetails.Split(new string[] { " | " }, StringSplitOptions.None);
                var title = bookDetailsArray[0];
                var author = bookDetailsArray[1];

                books = uow.BookRepository.Get(filter:
                    b => (b.Title.Contains(title)
                    && b.Author.Contains(author)))
                    .GroupBy(b => b.Title)
                    .Select(b => b.FirstOrDefault());
            }
            else
            {
                // user submits the form, custom search input
                books = uow.BookRepository.Get(filter:
                    b => (b.Title.Contains(bookDetails)
                    || b.Author.Contains(bookDetails)))
                    .GroupBy(b => b.Title)
                    .Select(b => b.FirstOrDefault());
            }

            return books;
        }

        public object Autocomplete(string searchTerm)
        {
            object result = uow.BookRepository.Get(filter:
                b => b.Title.Contains(searchTerm)
                || b.Author.Contains(searchTerm))
                .GroupBy(b => b.Title)
                .Select(b => b.FirstOrDefault())
                .Take(10)
                .Select(b => new
                {
                    value = b.Title + " | " + b.Author
                });

            return result;
        }
    }
}