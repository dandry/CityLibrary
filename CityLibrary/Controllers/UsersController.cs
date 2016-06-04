using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CityLibrary.DAL;
using CityLibrary.BL;
using CityLibrary.DAL.Models;

namespace CityLibrary.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        IUnitOfWork uow;
        BookFilter bf;
        UserFilter uf;

        public UsersController() : this(new UnitOfWork())
        {
        }

        public UsersController(IUnitOfWork uow)
        {
            this.uow = uow;
            bf = new BookFilter(uow);
            uf = new UserFilter(uow);
        }

        public ActionResult Index(string userName, bool autocompleteSource = false)
        {
            IEnumerable<LibraryUser> userList;

            if (Request.IsAjaxRequest())
            {
                userList = uf.FilterByName(userName, autocompleteSource).Take(50);
                
                return PartialView("_UserList", userList);
            }

            userList = uow.LibraryUserRepository.Get(orderBy:
                q => q.OrderBy(u => u.LastName))
                .Take(25);

            return View(userList);
        }

        public PartialViewResult LoadUserBooks(int id)
        {
            var user = uow.LibraryUserRepository.GetById(id);

            return PartialView("_UserBookList", user);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUser libraryUser = uow.LibraryUserRepository.GetById(id);
            if (libraryUser == null)
            {
                return HttpNotFound();
            }
            return View(libraryUser);
        }

        public PartialViewResult BorrowBook(int id)
        {
            var user = uow.LibraryUserRepository.GetById(id);

            return PartialView("_BorrowBook", user);
        }

        public PartialViewResult BorrowBook_LoadBooks(int id, string bookDetails, bool autocompleteSource = false)
        {
            var books = bf.FilterByType(BookType.Available, bookDetails, autocompleteSource);

            ViewBag.UserId = id;

            return PartialView("_BorrowBook_LoadBooks", books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BorrowBook(int id, int userId)
        {
            var book = uow.BookRepository.GetById(id);

            book.UserId = userId;
            book.BorrowDate = DateTime.Now;
            book.ReturnDate = book.BorrowDate.Value.AddMonths(1).AddDays(-1);

            uow.BookRepository.Update(book);
            uow.Save();

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

                uow.LibraryUserRepository.Insert(user);
                uow.Save();

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

            var user = uow.LibraryUserRepository.GetById(id);

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
                uow.LibraryUserRepository.Update(user);
                uow.Save();

                return RedirectToAction("Details", new { id = user.UserId });
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            uow.LibraryUserRepository.Delete(id);
            uow.Save();

            return RedirectToAction("Index");
        }

        public JsonResult Autocomplete(string type, string term)
        {
            object result;

            if (type == "user")
            {
                result = uf.Autocomplete(term);
            }
            else // book
            {
                result = bf.Autocomplete(term);
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsPeselAvailable(long pesel, int? userId)
        {
            var result = false;

            if (userId != null)
            {
                result = true;
            }
            else
            {
                result = uow.LibraryUserRepository.Get(filter:
                    u => u.PESEL.Equals(pesel)).Count() == 0 ? true : false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
