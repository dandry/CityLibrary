using CityLibrary.DAL;
using CityLibrary.DAL.Models;
using CityLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.Controllers
{
    public class HomeController : Controller
    {
        IUnitOfWork uow;

        public HomeController() : this (new UnitOfWork())
        {
        }

        public HomeController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var books = uow.BookRepository.Get(filter:
                    b => b.UserId.HasValue, orderBy:
                    q => q.OrderBy(b => b.ReturnDate))
                    .Take(50);

                var bookListingVM = new BookListingViewModel();

                foreach (Book b in books)
                {
                    if (b.DaysLeft > 0)
                        bookListingVM.ValidBorrowing.Add(b);
                    else
                        bookListingVM.PendingBorrowing.Add(b);
                }

                return View("IndexAuthenticated", bookListingVM);
            }
            return View();
        }
    }
}