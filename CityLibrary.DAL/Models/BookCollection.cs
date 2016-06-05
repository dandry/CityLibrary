using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.DAL.Models
{
    [Table("BookCollections")]
    public class BookCollection
    {
        [Key]
        public int CollectionId { get; set; }

        [Required]
        [Display(Name = "Kolekcja")]
        [Remote("IsNameAvailable", "Collections", AdditionalFields = "CollectionId", ErrorMessage = "Kolekcja o podanej nazwie znajduje się już w bazie.")]
        public string Name { get; set; }

        [Display(Name = "Książki")]
        public virtual ICollection<Book> CollectionBooks { get; set; }
    }
}