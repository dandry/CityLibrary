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
    public class AuthorsController : Controller
    {
        LibraryContext db = new LibraryContext();

        public ActionResult Index(string authorName, bool autocompleteSource = false)
        {
            IDictionary<string, List<Book>> authorList;

            if (Request.IsAjaxRequest())
            {
                authorList = db.LibraryBooks
                .Where(b => b.Author.Contains(authorName))
                .GroupBy(b => b.Author)
                .ToDictionary(g => g.Key, g => g.ToList());   
            }
            else
            {
                authorList = db.LibraryBooks
                   .GroupBy(b => b.Author)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }

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

            var result = db.LibraryBooks
                .Where(b => b.Author.Contains(term))
                .GroupBy(b => b.Author)
                .Select(b => b.FirstOrDefault())
                .Take(10)
                .Select(b => new
                {
                    value = b.Author
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string name)
        {
            var authorBooks = db.LibraryBooks
                    .Where(b => b.Author.Contains(name))
                    .OrderBy(b => b.Author)
                    .ThenBy(b => b.Title)
                    .ToLookup(b => b.Title);

            ViewBag.AuthorName = name;

            return View(authorBooks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}