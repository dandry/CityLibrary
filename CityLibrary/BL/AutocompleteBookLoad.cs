using CityLibrary.DAL;
using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.BL
{
    public class AutocompleteBookLoad
    {
        string[] _bookDetailsArray;
        string _title;
        string _author;

        private void SplitTerm(string term)
        {
            _bookDetailsArray = term.Split(new string[] { " | " }, StringSplitOptions.None);
            _title = _bookDetailsArray[0];
            _author = _bookDetailsArray[1];
        }

        public IEnumerable<Book> FilterBooksToList(LibraryContext db, string bookDetails, bool autocompleteSource = false)
        {
            IEnumerable<Book> books;

            if (autocompleteSource)
            {
                // autocomplete submits the form
                SplitTerm(bookDetails);

                books = db.LibraryBooks
                    .Where(b => (b.Title.Contains(_title) && b.Author.Contains(_author)))
                    .GroupBy(b => b.Title)
                    .Select(b => b.FirstOrDefault())
                    .ToList();
            }
            else
            {
                // user submits the form, custom search input
                books = db.LibraryBooks
                    .Where(b => (b.Title.Contains(bookDetails) || b.Author.Contains(bookDetails)))
                    .GroupBy(b => b.Title)
                    .Select(b => b.FirstOrDefault())
                    .ToList();
            }

            return books;
        }

        public ILookup<string, Book> FilterBooksToLookup(LibraryContext db, string bookDetails, bool autocompleteSource = false)
        {
            ILookup<string, Book> books;

            if (autocompleteSource)
            {
                // autocomplete submits the form
                SplitTerm(bookDetails);

                books = db.LibraryBooks
                    .Where(b => (b.Title.Contains(_title) && b.Author.Contains(_author)))
                    .OrderBy(b => b.Title)
                    .Take(25)
                    .ToLookup(b => b.Title);
            }
            else
            {
                // user submits the form, custom search input
                books = db.LibraryBooks
                    .Where(b => (b.Title.Contains(bookDetails) || b.Author.Contains(bookDetails)))
                    .Take(25)
                    .ToLookup(b => b.Title);
            }

            return books;
        }
    }
}