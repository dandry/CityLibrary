using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CityLibrary.DAL;
using CityLibrary.Models.Library;

namespace CityLibrary.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        LibraryContext db = new LibraryContext();

        //private ILibraryUserRepository userRepository;

        //public UsersController()
        //{
        //    this.userRepository = new LibraryUserRepository(new LibraryContext());
        //}

        public ActionResult Index(string userName, bool autocompleteSource = false)
        {
            if (Request.IsAjaxRequest())
            {
                IList<LibraryUser> userListFiltered;

                if (autocompleteSource)
                {
                    var userNameArray = userName.Split(' ');

                    var lastName = userNameArray[0];
                    var firstName = userNameArray[1];

                    userListFiltered = db.LibraryUsers
                    .Where(u => (u.FirstName.Contains(firstName) || u.LastName.Contains(lastName)))
                    .ToList();
                }
                else
                {
                    userListFiltered = db.LibraryUsers
                    .Where(u => (u.FirstName.Contains(userName) || u.LastName.Contains(userName)))
                    .ToList();
                }
                
                return PartialView("_UserList", userListFiltered);
            }

            var userList = db.LibraryUsers
                .OrderBy(u => u.LastName).ToList();

            return View(userList);
        }

        public JsonResult Autocomplete(string term)
        {

            var result = db.LibraryUsers
                .Where(u => (u.FirstName.Contains(term) || u.LastName.Contains(term)))
                .OrderBy(u => u.LastName)
                .Take(10)
                .Select(b => new
                {
                    value = b.LastName + " " + b.FirstName
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult LoadUserBooks(int id)
        {
            var user = db.LibraryUsers.Find(id);

            return PartialView("_UserBookList", user);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUser libraryUser = db.LibraryUsers.Find(id);
            if (libraryUser == null)
            {
                return HttpNotFound();
            }
            return View(libraryUser);
        }

        public PartialViewResult BorrowBooks(int id)
        {
            var user = db.LibraryUsers.Find(id);

            return PartialView("_BorrowBook", user);
        }

        public PartialViewResult BorrowBook_LoadBooks(int id, string bookDetails)
        {
            // 'bookDetails' parameter format received by jQuery autocomplete: "BookTitle | BookAuthor" 

            var bookDetailsArray = bookDetails.Split(new string[] { " | " }, StringSplitOptions.None);
            // if a normal string (w/o autocomplete) has been passed, the array contains only one element (no split occured)
            // assumption: there are no titles and authors entities which contain | char

            List<Book> books;

            if (bookDetailsArray.Count() == 1)
            {
                // custom search string passed
                books = db.LibraryBooks
                    .Where(b => b.UserId == null && (b.Title.Contains(bookDetails) || b.Author.Contains(bookDetails)))
                    .OrderBy(b => b.Title)
                    .ToList();
            }
            else
            {
                // jQuery autocomplete format passed
                var title = bookDetailsArray[0];
                var author = bookDetailsArray[1];

                books = db.LibraryBooks
                    .Where(b => b.UserId == null && (b.Title.Contains(title) && b.Author.Contains(author)))
                    .OrderBy(b => b.Title)
                    .ToList();
            }

            ViewBag.UserId = id;

            return PartialView("_BorrowBook_LoadBooks", books);
        }

        [HttpPost]
        public ActionResult BorrowBook(int id, int userId)
        {
            var book = db.LibraryBooks.Find(id);

            book.UserId = userId;
            book.BorrowDate = DateTime.Now;
            book.ReturnDate = book.BorrowDate.Value.AddMonths(1).AddDays(-1);

            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "RegistrationDate")] LibraryUser user)
        {
            if (ModelState.IsValid)
            {
                user.RegistrationDate = DateTime.Now;

                db.LibraryUsers.Add(user);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = user.UserId });
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.LibraryUsers.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LibraryUser user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = user.UserId });
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = db.LibraryUsers.Find(id);
            db.LibraryUsers.Remove(user);
            db.SaveChanges();

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
