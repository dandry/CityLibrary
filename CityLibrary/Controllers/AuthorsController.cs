using CityLibrary.BL;
using CityLibrary.DAL;
using CityLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        IUnitOfWork uow;
        AuthorFilter af;

        public AuthorsController() : this (new UnitOfWork())
        {
        }

        public AuthorsController(IUnitOfWork uow)
        {
            this.uow = uow;
            af = new AuthorFilter(uow);
        }

        public ActionResult Index(string authorName = "", bool autocompleteSource = false)
        {
            var authorList = uow.BookRepository.Get(filter:
                    b => b.Author.Contains(authorName), orderBy:
                    q => q.OrderBy(b => b.Author))
                    .GroupBy(b => b.Author)
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

            var result = af.Autocomplete(term);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string name)
        {
            var authorBooks = uow.BookRepository.Get(filter:
                    b => b.Author.Contains(name), orderBy:
                    q => q.OrderBy(b => b.Author).ThenBy(b => b.Title))
                    .ToLookup(b => b.Title);

            ViewBag.AuthorName = name;

            return View(authorBooks);
        }
    }
}