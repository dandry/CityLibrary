using CityLibrary.ViewModels;
using CityLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityLibrary.Models.Library;
using System.Data.Entity;
using CityLibrary.BL;
using PagedList;

namespace CityLibrary.Controllers
{
    [Authorize]
    public class CollectionsController : Controller
    {
        LibraryContext db = new LibraryContext();
        AutocompleteBookLoad acBookLoad = new AutocompleteBookLoad();

        public ActionResult Index(int page = 1)
        {
            var collections = db.BookCollections
                .OrderBy(bc => bc.Name)
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

        public ActionResult Books(int id, BookType type, string search)
        {
            var collection = db.BookCollections.Find(id);

            if (collection == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CollectionName = collection.Name;
            ViewBag.CollectionId = collection.CollectionId;
            ViewBag.BookType = type;

            ILookup<string, Book> lookup;

            if (type == BookType.Available)
            {
                if (search != null)
                {
                    lookup = collection.CollectionBooks
                    .Where(b => b.UserId == null)
                    .Where(b => b.Title.ToLower()
                    .Contains(search.ToLower()))
                    .ToLookup(b => b.Title);
                }
                else
                {
                    lookup = collection.CollectionBooks
                    .Where(b => b.UserId == null)
                    .ToLookup(b => b.Title);
                }

            }
            else if (type == BookType.Borrowed)
            {
                if (search != null)
                {
                    lookup = collection.CollectionBooks
                    .Where(b => b.UserId != null)
                    .Where(b => b.Title.ToLower()
                    .Contains(search.ToLower()))
                    .ToLookup(b => b.Title);
                }
                else
                {
                    lookup = collection.CollectionBooks
                    .Where(b => b.UserId != null)
                    .ToLookup(b => b.Title);
                }
            }
            else //all books
            {
                if (search != null)
                {
                    lookup = collection.CollectionBooks
                    .Where(b => b.Title.ToLower()
                    .Contains(search.ToLower()))
                    .ToLookup(b => b.Title);
                }
                else
                {
                    lookup = collection.CollectionBooks
                    .ToLookup(b => b.Title);
                }
            }

            return View("CollectionBooks", lookup);
        }

        public ActionResult AddBook(int id)
        {
            var collection = db.BookCollections.Find(id);

            if (collection == null)
            {
                return RedirectToAction("Index");
            }

            return View(collection);
        }

        [HttpPost]
        public ActionResult LoadBook(int id, string bookDetails, bool autoCompleteSource)
        {
            IEnumerable<Book> books = acBookLoad.FilterBooksToList(db, bookDetails, autoCompleteSource);

            if (books == null)
            {
                return new EmptyResult();
            }

            ViewBag.CollectionId = id;
            return PartialView("_LoadBook", books);
        }



        [HttpPost]
        public ActionResult AddCopy(int id, string title, string author)
        {

            var books = db.LibraryBooks
                .Where(b => (b.Author.ToLower().Contains(author.ToLower()))
                && (b.Title.ToLower().Contains(title.ToLower())))
                .ToList();

            var collection = db.BookCollections.Find(id);

            if (books == null || collection == null)
            {
                return RedirectToAction("Index");
            }

            foreach (Book b in books)
            {
                collection.CollectionBooks.Add(b);
            }

            db.Entry(collection).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Books", new { id = id, type = BookType.All });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BookCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.BookCollections.Add(collection);
                    db.SaveChanges();

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
            var collection = db.BookCollections.Find(id);

            if (collection == null)
            {
                return RedirectToAction("Index");
            }

            return View(collection);
        }

        [HttpPost]
        public ActionResult Edit(BookCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(collection).State = EntityState.Modified;
                    db.SaveChanges();

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
            var collection = db.BookCollections.Find(id);

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
            var collection = db.BookCollections.Find(id);

            if (collection != null)
            {
                db.BookCollections.Remove(collection);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
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
