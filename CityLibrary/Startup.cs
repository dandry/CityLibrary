using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CityLibrary.Startup))]
namespace CityLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
