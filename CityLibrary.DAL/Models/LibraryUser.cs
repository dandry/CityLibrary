using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.DAL.Models
{
    [Table("LibraryUsers")]
    public class LibraryUser
    {
        [Key]
        public int UserId { get; set; }

        [Required, Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required, Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "Number PESEL musi składać się z 11 cyfr.")]
        [Remote("IsPeselAvailable", "Users", AdditionalFields = "UserId", ErrorMessage = "Użytkownik z podanym numerem PESEL znajduje się już w bazie.")]
        public long PESEL { get; set; }

        [Required, Display(Name = "Data urodzin")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required, Display(Name = "Data rejestracji")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime RegistrationDate { get; set; }

        [EmailAddress(ErrorMessage = "Adres e-mail musi być w formacie nazwa@domena.")]
        public string Email { get; set; }

        [Required, Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Nr lokalu")]
        public int? ApartmentNumber { get; set; }

        [Required, Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Kod pocztowy musi być w formacie 00-000")]
        public string PostalCode { get; set; }

        [Required, Display(Name = "Miasto")]
        public string City { get; set; }

        public virtual ICollection<Book> BorrowedBooks { get; set; }

        // Not mapped properties
        [NotMapped]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
    }
}