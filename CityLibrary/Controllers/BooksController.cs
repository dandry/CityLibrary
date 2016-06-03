using CityLibrary.BL;
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
    [Authorize]
    public class BooksController : Controller
    {
        IUnitOfWork uow;
        BookFilter bf;

        public BooksController() : this(new UnitOfWork())
        {
        }

        public BooksController(IUnitOfWork uow)
        {
            this.uow = uow;
            bf = new BookFilter(uow);
        }



        public ActionResult Index(string bookDetails, bool autocompleteSource = false)
        {
            if (Request.IsAjaxRequest())
            {
                var books = bf.FilterByAutocomplete(bookDetails, autocompleteSource).ToLookup(b => b.Title);

                return PartialView("_BookList", books);
            }
            else
            {
                var books = uow.BookRepository.Get(orderBy:
                    q => q.OrderBy(b => b.Title)
                    .ThenBy(b => b.ReturnDate))
                    .Take(25)
                    .ToLookup(b => b.Title);

                return View(books);
            }
        }

        public JsonResult Autocomplete(string term)
        {
            var result = bf.Autocomplete(term);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCopy()
        {
            return View();
        }

        public PartialViewResult AddCopy_LoadAuthors(string searchString)
        {
            var authorList = uow.BookRepository.Get(filter:
                b => b.Author.Contains(searchString))
                .GroupBy(b => b.Author)
                .Select(b => b.FirstOrDefault())
                .Select(b => new
                {
                    Id = b.BookId,
                    Name = b.Author
                });

            ViewBag.AuthorsDropDown = authorList;

            return PartialView("_AddCopy_LoadAuthors");
        }

        public PartialViewResult AddCopy_LoadAuthorBooks(string authorName)
        {
            var bookList = uow.BookRepository.Get(filter:
                b => b.Author.Contains(authorName))
                .GroupBy(b => b.Author)
                .Select(b => b.FirstOrDefault());

            return PartialView("_AddCopy_LoadAuthorBooks", bookList);
        }

        public PartialViewResult AddCopy_RenderDetails(int id)
        {
            var book = uow.BookRepository.GetById(id);

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
        [ValidateAntiForgeryToken]
        public ActionResult AddCopy([Bind(Exclude = "Collection")]Book book)
        {
            if (ModelState.IsValid)
            {
                uow.BookRepository.Insert(book);
                uow.Save();

                return Json(new { url = Url.Action("Details", new { id = book.BookId }) });
            }

            return PartialView("_AddCopy_Details", book);
        }

        public ActionResult Authors(string name)
        {
            IEnumerable<Book> books;

            if (name == null)
            {
                books = uow.BookRepository.Get(orderBy:
                    q => q.OrderBy(b => b.Author));

            }
            else
            {
                books = uow.BookRepository.Get(filter:
                    b => b.Author.Contains(name), orderBy:
                    q => q.OrderBy(b => b.Author));

                ViewBag.AuthorName = name;
            }

            return View(books.ToLookup(b => b.Title));
        }

        public ActionResult Details(int id)
        {
            var book = uow.BookRepository.GetById(id);

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
            ViewBag.Collections = uow.BookCollectionRepository.Get(orderBy:
                q => q.OrderBy(c => c.Name));

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
                    uow.BookRepository.Insert(book);
                    uow.Save();

                    return RedirectToAction("Details", new { id = book.BookId });
                }
                throw new Exception("Saving to database failed.");
            }
            catch
            {
                //DropDownList initialization for Book Collections in the View
                ViewBag.Collections = uow.BookCollectionRepository.Get(orderBy:
                q => q.OrderBy(c => c.Name));

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

            ViewBag.Collections = uow.BookCollectionRepository.Get(orderBy:
                q => q.OrderBy(c => c.Name));

            var book = uow.BookRepository.GetById(id);

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    uow.BookRepository.Update(book);
                    uow.Save();

                    return RedirectToAction("Details", new { id = book.BookId });
                }

                ViewBag.Collections = uow.BookCollectionRepository.Get(orderBy:
                q => q.OrderBy(c => c.Name));

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
            uow.BookRepository.Delete(id);
            uow.Save();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Borrow(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Books");
            }

            var book = uow.BookRepository.GetById(id);

            if (book == null)
            {
                return RedirectToAction("Index");
            }

            if (book.UserId != null)
            {
                return new HttpStatusCodeResult(500, "Bad request");
            }

            var userList = uow.LibraryUserRepository.Get();

            var bookBorrowVM = new BookBorrowViewModel()
            {
                Book = book,
                Users = userList.ToList()
            };

            return View(bookBorrowVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Borrow(BookBorrowViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var book = uow.BookRepository.GetById(viewModel.Book.BookId);

                    book.UserId = viewModel.SelectedListUserId;
                    book.BorrowDate = viewModel.CurrentDate;
                    book.ReturnDate = viewModel.ReturnDate;

                    uow.BookRepository.Update(book);
                    uow.Save();

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
        [ValidateAntiForgeryToken]
        public ActionResult Return(int id)
        {
            try
            {
                var book = uow.BookRepository.GetById(id);

                book.UserId = null;
                book.BorrowDate = null;
                book.ReturnDate = null;

                uow.BookRepository.Update(book);
                uow.Save();

                return RedirectToAction("Details", new { id = id });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Prolong(int id)
        {
            var book = uow.BookRepository.GetById(id);
            book.ReturnDate = DateTime.Now.AddMonths(1).AddDays(-1);

            uow.BookRepository.Update(book);
            uow.Save();

            return RedirectToAction("Details", new { id = book.BookId });
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
                result = uow.BookRepository.Get(filter:
                    b => b.ISBN.Equals(ISBN)).Count() == 0 ? true : false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
