// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents mapping configuration for 'UserRole' table.
    /// </summary>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserRoleMap<TUserRole, TKey> : EntityMap<TUserRole> 
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserRoleMap(EntityConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => new
            {
               p.UserId,
               p.RoleId
            });

            Property(p => p.UserId)
                .HasColumnName(Configuration[UserRoleFields.UserId])
                .IsRequired();

            Property(p => p.RoleId)
                .HasColumnName(Configuration[UserRoleFields.RoleId])
                .IsRequired();
        }
    }
}
