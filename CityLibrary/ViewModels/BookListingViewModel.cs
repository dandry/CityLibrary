using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.ViewModels
{
    public class BookListingViewModel
    {
        public BookListingViewModel()
        {
            ValidBorrowing = new List<Book>();
            ExpiredBorrowing = new List<Book>();
        }

        public List<Book> ValidBorrowing { get; set; }

        public List<Book> ExpiredBorrowing { get; set; }
    }
}