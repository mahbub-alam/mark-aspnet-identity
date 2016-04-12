// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents role entity configuration.
    /// </summary>
    public class RoleConfiguration : EntityConfiguration
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            TableName = Entities.Role;
            this[RoleFields.Id] = RoleFields.Id;
            this[RoleFields.Name] = RoleFields.Name;
        }
    }
}
