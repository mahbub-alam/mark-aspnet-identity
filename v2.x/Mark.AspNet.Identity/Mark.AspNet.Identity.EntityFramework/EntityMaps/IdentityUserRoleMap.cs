// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents mapping configuration for 'UserRole' table.
    /// </summary>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserRoleMap<TUserRole, TKey> : EntityMap<TUserRole> 
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        public IdentityUserRoleMap(string tableName) : base(tableName)
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
                .HasColumnName("UserId")
                .IsRequired();

            Property(p => p.RoleId)
                .HasColumnName("RoleId")
                .IsRequired();
        }
    }
}
