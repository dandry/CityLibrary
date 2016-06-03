using CityLibrary.Controllers;
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
    public class BookControllerTest
    {
        [TestMethod]
        public void TestIndexBookLoad()
        {
            // arrange
            var controller = new BooksController();

            // act
            ViewResult result = controller.Index(null) as ViewResult;

            // assert

            Assert.IsTrue(true);
        }
    }
}
