using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CityLibrary.Tests.FakeComponents
{
    public static class FakeUserAuthContext
    {
        public static ControllerContext UserAuthenticated(bool isAuthenticated, Controller controller)
        {
            // user authenticated

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.IsAuthenticated).Returns(isAuthenticated); // or false
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            return new ControllerContext(context.Object, new RouteData(), controller);
        }
    }
}
