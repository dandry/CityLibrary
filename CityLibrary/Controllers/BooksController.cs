using CityLibrary.DAL;
using CityLibrary.Models.Library;
using CityLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.Controllers
{
    public class BooksController : Controller
    {
        LibraryContext db = new LibraryContext();

        // Default, complete list of books
        public ActionResult Index(string searchString)
        {
            if (searchString == null)
            {
                var queriedBooks = db.LibraryBooks
                .OrderBy(b => b.Title)
                .ThenBy(b => b.ReturnDate)
                .ToLookup(b => b.Title);

                return View(queriedBooks);
            }
            else
            {
                var queriedBooks = db.LibraryBooks
                .Where(b => b.Title.Contains(searchString))
                .OrderBy(b => b.Title)
                .ThenBy(b => b.ReturnDate)
                .ToLookup(b => b.Title);

                return View(queriedBooks);
            } 
        }

        public ActionResult AddCopy()
        {
            return View();
        }

        public PartialViewResult AddCopy_LoadAuthors(string searchString)
        {
            var bookList = db.LibraryBooks
                .Where(b => b.Author.Contains(searchString))
                .GroupBy(b => b.Author)
                .Select(b => b.FirstOrDefault())
                .Select(b => new
                {
                    Id = b.BookId,
                    Name = b.Author
                })
                .ToList();

            ViewBag.AuthorsDropDown = new SelectList(bookList, "Name", "Name");

            return PartialView("_AddCopy_LoadAuthors");
        }

        public PartialViewResult AddCopy_LoadAuthorBooks(string authorName)
        {
            var bookList = db.LibraryBooks
                .Where(b => b.Author == authorName)
                .GroupBy(b => b.Title)
                .Select(b => b.FirstOrDefault());

            return PartialView("_AddCopy_LoadAuthorBooks", bookList);
        }

        public PartialViewResult AddCopy_RenderDetails(int id)
        {
            var book = db.LibraryBooks.Find(id);

            var newCopy = new Book()
            {
                Author = book.Author,
                Title = book.Title,
                Publisher = book.Publisher,
                CollectionId = book.CollectionId,
                Collection = book.Collection
            };

            return PartialView("_AddCopy_Details", newCopy);
        }

        [HttpPost]
        public ActionResult AddCopy(Book book)
        {
            if (ModelState.IsValid)
            {
                db.LibraryBooks.Add(book);
                db.SaveChanges();

                return Json(new { url = Url.Action("Details", new { id = book.BookId }) });
            }

            return PartialView("_AddCopy_Details", book);
        }

        public ActionResult Author(string name)
        {
            if (name == null)
            {
                return RedirectToAction("Index");
            }

            var queriedBooks = db.LibraryBooks
                .Where(b => b.Author.Contains(name))
                .ToLookup(b => b.Title);

            ViewBag.AuthorName = name;

            return View(queriedBooks);
        }

        public ActionResult Details(int id)
        {
            var book = db.LibraryBooks.Find(id);

            //Enables and disabled buttons to return/prolong/borrow a book 
            //based on existance of UserId
            if (book.UserId != null)
            {
                //Prolong and Return buttons enabled as a book is borrowed
                ViewBag.ProlongReturnButtonsDisabledAttribute = null;
                //Borrow button is disabled as a book is already borrowed
                ViewBag.BorrowButtonDisabledAttribute = "disabled";
            }
            else
            {
                ViewBag.ProlongReturnButtonsDisabledAttribute = "disabled";
                ViewBag.BorrowButtonDisabledAttribute = null;
            }

            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            //DropDownList initialization for Book Collections in the View
            ViewBag.CollectionId = new SelectList(db.BookCollections, "CollectionId", "Name");

            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.LibraryBooks.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = book.BookId });
                }
                throw new Exception("Saving to database failed.");
            }
            catch
            {
                //DropDownList initialization for Book Collections in the View
                ViewBag.CollectionId = new SelectList(db.BookCollections, "CollectionId", "Name");
                return View(book);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ViewBag.Collections = db.BookCollections;
            var book = db.LibraryBooks.Find(id);

            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = book.BookId });
                }
                ViewBag.Collections = db.BookCollections;
                return View(book);
            }
            catch
            {
                return View(book);
            }    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var book = db.LibraryBooks.Find(id);
            db.LibraryBooks.Remove(book);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Borrow(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Books");
            }

            var book = db.LibraryBooks.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index");
            }

            if (book.UserId != null)
            {
                return new HttpStatusCodeResult(500, "Bad request");
            }

            var userList = db.LibraryUsers.ToList();

            var bookBorrowViewModel = new BookBorrowViewModel()
            {
                Book = book,
                Users = userList
            };

            return View(bookBorrowViewModel);
        }

        [HttpPost]
        public ActionResult Borrow(BookBorrowViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var book = db.LibraryBooks.Find(viewModel.Book.BookId);

                    book.UserId = viewModel.SelectedListUserId;
                    book.BorrowDate = viewModel.CurrentDate;
                    book.ReturnDate = viewModel.ReturnDate;

                    db.LibraryBooks.AddOrUpdate(book);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = book.BookId });
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Return(int id)
        {
            try
            {
                var book = db.LibraryBooks.Find(id);

                book.UserId = null;
                book.BorrowDate = null;
                book.ReturnDate = null;

                db.LibraryBooks.AddOrUpdate(book);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = id });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // This remote validation check whether there is an entity with given ISBN already in the database. To prevent this method 
        // from firing in Edit action (obviously the ISBN would then exist), BookId is passed along. Create and AddCopy actions 
        //have it them set to 0, but an Edit action has actually the Id.
        public JsonResult IsISBNAvailable(string ISBN, int? bookId)
        {
            var result = false;

            // passed from Edit action, so allow the ISBN
            if (bookId != 0)
            {
                result = true;
            }
            // passed from Create/AddCopy actions, check if ISBN already exists
            else
            {
                result = !db.LibraryBooks.Any(b => b.ISBN == ISBN);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
