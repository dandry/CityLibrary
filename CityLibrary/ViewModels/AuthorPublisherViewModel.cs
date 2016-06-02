using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityLibrary.ViewModels
{
    public class AuthorPublisherViewModel
    {
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Liczba pozycji")]
        public int BookCount { get; set; }

        [Display(Name = "Liczba egzemplarzy")]
        public int CopyCount { get; set; }
    }
}