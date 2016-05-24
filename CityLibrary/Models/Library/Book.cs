using CityLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.Models.Library
{
    [Table("Books")]
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{2}-[0-9]{4}-[0-9]{3}-[0-9]{1}$", ErrorMessage = "Number ISBN musi być podany w formacie 000-00-0000-000-0")]
        [Remote("IsISBNAvailable", "Books", AdditionalFields = "BookId", ErrorMessage = "Number ISBN znajduje się już w bazie")]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "Wydawnictwo")]
        public string Publisher { get; set; }

        [Required]
        [BookPrintDateRange]
        [Display(Name = "Rok wydania")]
        public int YearPrinted { get; set; }

        public int? UserId { get; set; }

        [Required]
        public int CollectionId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data wypożyczenia")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? BorrowDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data zwrotu")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ReturnDate { get; set; }

        public virtual BookCollection Collection { get; set; }
        public virtual LibraryUser LibraryUser { get; set; }


        /// <summary>
        /// Not mapped properties
        /// </summary>

        [NotMapped]
        [Display(Name = "Pozostało dni")]
        public int? DaysLeft
        {
            get
            {
                if (BorrowDate != null || ReturnDate != null)
                    return (ReturnDate.Value - DateTime.Now).Days;
                else
                    return null;
            }
        }

        [NotMapped]
        [Display(Name = "Wypożyczył")]
        public string UserFullName
        {
            get
            {
                if (LibraryUser == null)
                    return null;

                return LibraryUser.FullName;
            }
        }

        
    }
}