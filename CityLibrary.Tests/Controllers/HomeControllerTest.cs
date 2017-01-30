using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CityLibrary;
using CityLibrary.Controllers;
using Moq;
using System.Web;
using System.Web.Routing;
using CityLibrary.Tests.FakeComponents;
using CityLibrary.ViewModels;

namespace CityLibrary.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        FakeUnitOfWork fUow = new FakeUnitOfWork();

        [TestMethod]
        public void Index_IsAuthenticated()
        {
            // user authenticated

            // arrange
            HomeController controller = new HomeController(fUow);

                // fake authentication = true
                controller.ControllerContext = FakeUserAuthContext.UserAuthenticated(true, controller);

            // act
            ViewResult viewResult = (ViewResult)controller.Index();

            // assert
            var model = viewResult.Model as BookListingViewModel;

            Assert.AreEqual(1, (model.ExpiredBorrowing.Count));
            Assert.AreEqual(3, (model.ValidBorrowing.Count));

        }

        [TestMethod]
        public void Index_IsNotAuthenticated()
        {
            // user not authenticated

            // arrange
            HomeController controller = new HomeController();

                // fake authentication = false
                controller.ControllerContext = FakeUserAuthContext.UserAuthenticated(false, controller);

            // act
            ViewResult viewResult = (ViewResult)controller.Index();

            // assert
            Assert.AreEqual(viewResult.ViewName, "");
        }

    }
}
