using CityLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.ViewModels
{
    public class BookBorrowViewModel
    {

        public Book Book { get; set; }

        [Display(Name = "Użytkownik")]
        public int SelectedListUserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CurrentDate => DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ReturnDate => CurrentDate.AddDays(30);

        public IEnumerable<LibraryUser> Users { get; set; }

        public IEnumerable<SelectListItem> UserList => new SelectList(Users, "UserId", "FullName");
    }
}