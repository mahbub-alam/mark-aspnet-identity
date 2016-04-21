// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.EntityFramework.Tests
{
    public class ApplicationDbContext 
        : IdentityDbContext<
            ApplicationUser, 
            ApplicationRole, 
            int, 
            ApplicationUserLogin, 
            ApplicationUserRole, 
            ApplicationUserClaim>
    {

        public ApplicationDbContext() : base("DbConnectionString")
        {
        }
    }
}
