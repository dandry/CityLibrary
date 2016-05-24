using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.ViewModels
{
    public class BookCreateViewModel
    {
        public Book Book { get; set; }

        public int SelectedCollectionId { get; set; }

        public List<BookCollection> Collection { get; set; }

        public IEnumerable<SelectListItem> CollectionList
        {
            get
            {
                return new SelectList(CollectionList, "CollectionId", "Name");
            }
        }

        public BookCreateViewModel()
        {
        }
    }
}