using CityLibrary.ViewModels;
using CityLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityLibrary.Models.Library;
using System.Data.Entity;

namespace CityLibrary.Controllers
{
    public class CollectionsController : Controller
    {
        LibraryContext db = new LibraryContext();

        public ActionResult Index()
        {
            var collections = db.BookCollections
                .OrderBy(bc => bc.Name)
                .ToList();

            if (collections == null)
            {
                return RedirectToAction("Index");
            }

            var collectionsViewModel = new List<BookCollectionViewModel>();

            foreach (var collection in collections)
            {
                var bookCollectionViewModel = new BookCollectionViewModel(collection);
                collectionsViewModel.Add(bookCollectionViewModel);
            }

            return View(collectionsViewModel);
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

        public JsonResult Autocomplete(string term)
        {

            var result = db.LibraryBooks
                .Where(b => b.Title.Contains(term))
                .GroupBy(b => b.Title)
                .Select(b => b.FirstOrDefault())
                .Take(10)
                .Select(b => new
                {
                    value = b.Title + " | " + b.Author
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadBook(int id, string bookDetails)
        {

            // 'bookDetails' parameter format received by jQuery autocomplete: "BookTitle | BookAuthor" 

            var bookDetailsArray = bookDetails.Split(new string[] { " | " }, StringSplitOptions.None);
            // if a normal string (w/o autocomplete) has been passed, the array contains only one element (no split occured)

            Book book;

            if (bookDetailsArray.Count() == 1)
            {
                // custom search string passed
                var books = db.LibraryBooks
                    .Where(b => (b.Title.Contains(bookDetails)) || b.Author.Contains(bookDetails))
                    .GroupBy(b => b.Title)
                    .Select(b => b.FirstOrDefault())
                    .ToList();

                // there are more than one entry to display, so search string is not specific enough
                if (books.Count > 1)
                {
                    return new EmptyResult();
                }

                book = books.FirstOrDefault();
            }
            else
            {
                // jQuery autocomplete format passed
                var title = bookDetailsArray[0];
                var author = bookDetailsArray[1];

                book = db.LibraryBooks
                    .Where(b => (b.Title.Contains(title)) && b.Author.Contains(author))
                    .FirstOrDefault();
            }

            if (book == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CollectionId = id;

            return PartialView("_LoadBook", book);
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

                    return RedirectToAction("Books", "Collections", new { id = collection.CollectionId, type = BookType.All });
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
