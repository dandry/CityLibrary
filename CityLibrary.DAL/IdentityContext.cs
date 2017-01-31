using CityLibrary.DAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CityLibrary.DAL
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext()
            : base("LibraryContext", throwIfV1Schema: false)
        {
        }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }
}
