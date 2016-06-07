using CityLibrary.Controllers;
using CityLibrary.DAL.Models;
using CityLibrary.Tests.FakeComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CityLibrary.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        FakeUnitOfWork fUow = new FakeUnitOfWork();

        [TestMethod]
        public void Users_Index()
        {
            // arrange
            var controller = new UsersController(fUow);
            controller.ControllerContext = FakeUserAuthContext.UserAuthenticated(true, controller);
            //FakeDbInit.InitDummyDb();

            // act
            ViewResult viewResult = (ViewResult)controller.Index(null, false);

            // assert
            var model = viewResult.Model as IEnumerable<LibraryUser>;
            Assert.AreEqual(9, model.Count());
        }
    }
}
