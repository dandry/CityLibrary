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
    public class UsersController : Controller
    {
        LibraryContext db = new LibraryContext();

        //private ILibraryUserRepository userRepository;

        //public UsersController()
        //{
        //    this.userRepository = new LibraryUserRepository(new LibraryContext());
        //}

        public ActionResult Index()
        {
            var userList = db.LibraryUsers
                .OrderBy(u => u.LastName).ToList();

            return View(userList);
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

        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,LastName,FirstName,DateOfBirth,RegistrationDate")] LibraryUser libraryUser)
        {
            if (ModelState.IsValid)
            {
                db.LibraryUsers.Add(libraryUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(libraryUser);
        }

        public ActionResult Edit(int? id)
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            LibraryUser libraryUser = db.LibraryUsers.Find(id);
            db.LibraryUsers.Remove(libraryUser);
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
