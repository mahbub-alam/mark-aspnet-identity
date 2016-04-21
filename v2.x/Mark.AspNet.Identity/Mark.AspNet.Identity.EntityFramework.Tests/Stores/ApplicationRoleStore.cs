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
    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>
    {
        public ApplicationRoleStore(DbContext context) : base(context)
        {
        }
    }
}
