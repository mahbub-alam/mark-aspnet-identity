// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Data;

namespace Mark.AspNet.Identity.SqlServer.Tests
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
        public ApplicationUserStore(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
