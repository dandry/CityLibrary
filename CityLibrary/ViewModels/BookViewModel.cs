using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityLibrary.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Autor")]
        public string Author { get; set; }
        
        public int CollectionId { get; set; }
        [Display(Name = "Nazwa kolekcji")]
        public string CollectionName { get; set; }

        public int? UserId { get; set; }
        public string UserName { get; set; }

        public BookViewModel(Book book)
        {
            this.BookId = book.BookId;
            this.Name = book.Title;
            this.Author = book.Author;
            this.CollectionId = book.CollectionId;
            this.CollectionName = book.Collection.Name;
            this.UserId = book.UserId;

            if (book.UserId == null)
                this.UserName = "–";
            else
                this.UserName = book.LibraryUser.FirstName + " " + book.LibraryUser.LastName;

        }
    }
}