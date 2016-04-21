// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Mark.AspNet.Identity.EntityFramework.Tests
{
    public class ApplicationUserStore 
        : UserStore<
            ApplicationUser, 
            ApplicationRole, 
            int, 
            ApplicationUserLogin, 
            ApplicationUserRole, 
            ApplicationUserClaim>
    {
        public ApplicationUserStore(DbContext context) : base(context)
        {
        }
    }
}
