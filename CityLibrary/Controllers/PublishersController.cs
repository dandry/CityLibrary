using CityLibrary.BL;
using CityLibrary.DAL;
using CityLibrary.Models.Library;
using CityLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.Controllers
{
    [Authorize]
    public class PublishersController : Controller
    {
        IUnitOfWork uow;
        PublisherFilter pf;

        public PublishersController() : this (new UnitOfWork())
        {
        }

        public PublishersController(IUnitOfWork uow)
        {
            this.uow = uow;
            pf = new PublisherFilter(uow);
        }


        public ActionResult Index(string publisherName = "", bool autocompleteSource = false)
        {
            var authorList = uow.BookRepository.Get(filter:
                    b => b.Publisher.Contains(publisherName))
                    .GroupBy(b => b.Publisher)
                    .ToDictionary(g => g.Key, g => g.ToList());

            List<AuthorPublisherViewModel> authorVMList = new List<AuthorPublisherViewModel>();

            foreach (var keyValuePair in authorList)
            {
                authorVMList.Add(new AuthorPublisherViewModel()
                {

                    Name = keyValuePair.Key,
                    CopyCount = keyValuePair.Value.Count,
                    BookCount = keyValuePair.Value
                        .GroupBy(v => v.Title)
                        .Select(v => v.First())
                        .Count()
                });
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AuthorPublisherList", authorVMList);
            }

            return View(authorVMList);
        }

        public JsonResult Autocomplete(string term)
        {
            var result = pf.Autocomplete(term);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string name)
        {
            var publisherBooks = uow.BookRepository.Get(filter:
                    b => b.Publisher.Contains(name), orderBy:
                    q => q.OrderBy(b => b.Author).ThenBy(b => b.Title))
                    .ToLookup(b => b.Title);

            ViewBag.Publisher = name;
            return View(publisherBooks);
        }
    }
}