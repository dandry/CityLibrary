using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CityLibrary.Models.Library
{
    [Table("LibraryUsers")]
    public class LibraryUser
    {
        [Key]
        public int UserId { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }


        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Book> BorrowedBooks { get; set; }

    }
}