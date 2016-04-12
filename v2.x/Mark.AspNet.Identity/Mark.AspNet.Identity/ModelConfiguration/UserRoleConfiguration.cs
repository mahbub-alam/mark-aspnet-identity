// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user role entity configuration.
    /// </summary>
    public class UserRoleConfiguration : EntityConfiguration
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            TableName = Entities.UserRole;
            this[UserRoleFields.RoleId] = UserRoleFields.RoleId;
            this[UserRoleFields.UserId] = UserRoleFields.UserId;
        }
    }
}
