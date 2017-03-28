using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Mark.AspNet.Identity;
using Mark.DotNet.Data.Common;

namespace MvcAspNetIdentitySqlServerSample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbStorageContext<ApplicationUser, int>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }

    public class UnitOfWorkFactory
    {
        public static UnitOfWork Create()
        {
            return new UnitOfWork(new ApplicationDbContext());
        }
    }
}