using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.ViewModels
{
    public class BookListViewModel
    {
        public Dictionary<string, List<BookViewModel>> Books { get; private set; }
        private List<BookViewModel> queriedBooksViewModel;

        public BookListViewModel(IEnumerable<Book> queriedBooks)
        {
            queriedBooksViewModel = new List<BookViewModel>();

            foreach (var book in queriedBooks)
            {
                var bookViewModel = new BookViewModel(book);
                queriedBooksViewModel.Add(bookViewModel);
            }

            Books = queriedBooksViewModel
                .GroupBy(b => b.Name)
                .Where(g => g.Count() > 1)
                .ToDictionary(b => b.Key, b => b.ToList());
        }

    }
}