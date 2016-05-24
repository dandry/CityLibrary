using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityLibrary.ViewModels
{
    public class BookCollectionViewModel
    {
        [Display(Name = "ID")]
        public int CollectionId { get; set; }

        [Display(Name = "Nazwa kolekcji")]
        public string Name { get; set; }
        public ICollection<Book> CollectionBooks { get; set; }

        public ICollection<Book> BorrowedBooks
        {
            get
            {
                return CollectionBooks.Where(b => b.UserId != null).ToList();
            }
        }

        public ICollection<Book> AvailableBooks
        {
            get
            {
                return CollectionBooks.Where(b => b.UserId == null).ToList();
            }
        }

        public BookCollectionViewModel(BookCollection  bookCollection)
        {
            this.CollectionId = bookCollection.CollectionId;
            this.Name = bookCollection.Name;
            this.CollectionBooks = bookCollection.CollectionBooks;

        }


    }
}