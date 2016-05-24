using CityLibrary.ViewModels;
using CityLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityLibrary.Models.Library;

namespace CityLibrary.Controllers
{
    public class CollectionsController : Controller
    {
        LibraryContext db = new LibraryContext();

        // GET: Collection
        public ActionResult Index()
        {
            var collections = db.BookCollections
                .OrderBy(bc => bc.Name)
                .ToList();

            var collectionsViewModel = new List<BookCollectionViewModel>();

            foreach (var collection in collections)
            {
                var bookCollectionViewModel = new BookCollectionViewModel(collection);
                collectionsViewModel.Add(bookCollectionViewModel);
            }
            
            return View(collectionsViewModel);
        }

        public ActionResult AvailableBooks(int? id)
        {
            List<Book> availableBooks;

            if (id == null)
            {
                availableBooks = db.BookCollections
                    .SelectMany(cb => cb.CollectionBooks)
                    .OrderBy(b => b.Title)
                    .ToList();

            }
            else
            {
                var collection = db.BookCollections.Find(id);

                availableBooks = collection.CollectionBooks
                    .Where(b => b.UserId == null)
                    .ToList();

                ViewBag.CollectionName = collection.Name;
            }

            return View(availableBooks);
        }

        // GET: Collection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Collection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Collection/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Collection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Collection/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Collection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Collection/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
