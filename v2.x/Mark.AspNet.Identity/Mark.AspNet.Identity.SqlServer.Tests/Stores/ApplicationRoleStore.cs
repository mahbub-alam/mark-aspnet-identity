// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Data;

namespace Mark.AspNet.Identity.SqlServer.Tests
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>
    {
        public ApplicationRoleStore(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
