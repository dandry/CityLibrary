using CityLibrary.ViewModels;
using CityLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityLibrary.DAL.Models;
using System.Data.Entity;
using CityLibrary.BL;
using PagedList;

namespace CityLibrary.Controllers
{
    [Authorize]
    public class CollectionsController : Controller
    {
        IUnitOfWork uow;
        BookFilter bf;

        public CollectionsController() : this (new UnitOfWork())
        {
        }

        public CollectionsController(IUnitOfWork uow)
        {
            this.uow = uow;
            bf = new BookFilter(uow);
        }

        public ActionResult Index(int page = 1)
        {
            var collections = uow.BookCollectionRepository.Get(
                filter: c => c.Name.Contains(""), orderBy: q => q.OrderBy(c => c.Name))
                .ToList();
                
            if (collections == null)
            {
                return RedirectToAction("Index");
            }

            var collectionViewModelList = new List<BookCollectionViewModel>();

            foreach (var collection in collections)
            {
                var bookCollectionViewModel = new BookCollectionViewModel(collection);
                collectionViewModelList.Add(bookCollectionViewModel);
            }

            var collectionViewModelPagedList = collectionViewModelList.ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CollectionList", collectionViewModelPagedList);
            }

            return View(collectionViewModelPagedList);
        }

        public ActionResult Books(int id, BookType type, string search = "")
        {
            var collection = uow.BookCollectionRepository.GetById(id);

            if (collection == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CollectionName = collection.Name;
            ViewBag.CollectionId = collection.CollectionId;
            ViewBag.BookType = type;

            var books = bf.FilterByType(id, type, search).ToLookup(b => b.Title);

            return View("CollectionBooks", books);
        }

        public ActionResult AddBook(int id)
        {
            var collection = uow.BookCollectionRepository.GetById(id);

            if (collection == null)
            {
                return RedirectToAction("Index");
            }

            return View(collection);
        }

        [HttpPost]
        public ActionResult LoadBook(int id, string bookDetails, bool autoCompleteSource)
        {
            var books = bf.FilterByAutocomplete(bookDetails, autoCompleteSource);

            if (books == null)
            {
                return new EmptyResult();
            }

            ViewBag.CollectionId = id;
            return PartialView("_LoadBook", books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCopy(int id, string title, string author)
        {
            var books = uow.BookRepository.Get(filter:
                b => b.Title.Contains(title)
                && b.Author.Contains(author));

            var collection = uow.BookCollectionRepository.GetById(id);

            if (books == null || collection == null)
            {
                return RedirectToAction("Index");
            }

            foreach (Book b in books)
            {
                collection.CollectionBooks.Add(b);
            }

            uow.BookCollectionRepository.Update(collection);
            uow.Save();

            return RedirectToAction("Books", new { id = id, type = BookType.All });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    uow.BookCollectionRepository.Insert(collection);
                    uow.Save();

                    return RedirectToAction("Index");
                }

                return View(collection);
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Nie udało się utworzyć kolekcji. Spróbuj ponownie lub skontaktuj się z administratorem.");
                return View(collection);
            }
        }

        public ActionResult Edit(int id)
        {
            var collection = uow.BookCollectionRepository.GetById(id);

            if (collection == null)
            {
                return RedirectToAction("Index");
            }

            return View(collection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    uow.BookCollectionRepository.Update(collection);
                    uow.Save();

                    return RedirectToAction("Books", "Collections", new { id = collection.CollectionId, type = BookType.All });
                }

                ModelState.AddModelError(String.Empty, "Nie udało się zaktualizować kolekcji. Spróbuj ponownie lub skontaktuj się z administratorem.");
                return View(collection);
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Wystąpił błąd podczas aktualizacji pozycji w bazie danych. Skontaktuj się z administratorem.");
                return View(collection);
            }
        }

        public ActionResult Delete(int id)
        {
            var collection = uow.BookCollectionRepository.GetById(id);

            if (collection == null)
            {
                return HttpNotFound();
            }

            return View(collection);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var collection = uow.BookCollectionRepository.GetById(id);

            if (collection.CollectionBooks.Count > 0)
            {
                ModelState.AddModelError(String.Empty, "Nie można usunąć wybranej kolekcji, ponieważ znajdują się w niej książki.");

                return View(collection);
            }

            uow.BookCollectionRepository.Delete(collection);
            uow.Save();

            return RedirectToAction("Index");
        }

        public JsonResult IsNameAvailable(string name, int? collectionId)
        {
            var result = false;

            if (collectionId != null)
            {
                result = true;
            }
            else
            {
                result = uow.BookCollectionRepository.Get(filter:
                    c => c.Name.Equals(name)).Count() == 0 ? true : false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
