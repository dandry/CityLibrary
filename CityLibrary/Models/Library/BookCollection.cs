using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CityLibrary.Models.Library
{
    [Table("BookCollections")]
    public class BookCollection
    {
        [Key]
        public int CollectionId { get; set; }

        [Display(Name = "Kolekcja")]
        public string Name { get; set; }

        public virtual ICollection<Book> CollectionBooks { get; set; }
    }
}