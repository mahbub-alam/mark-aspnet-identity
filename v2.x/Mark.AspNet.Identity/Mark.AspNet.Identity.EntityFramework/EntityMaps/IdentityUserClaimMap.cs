// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents mapping configuration for 'UserClaim' table.
    /// </summary>
    /// <typeparam name="TUserClaim">User claim type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserClaimMap<TUserClaim, TKey> : EntityMap<TUserClaim>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        public IdentityUserClaimMap(string tableName) : base(tableName)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => p.Id);
            Property(p => p.Id)
                .HasColumnName("Id");
        }

        /// <summary>
        /// Map all non-key fields.
        /// </summary>
        protected override void MapFields()
        {
            Property(p => p.UserId)
                .HasColumnName("UserId");

            Property(p => p.ClaimType)
                .HasColumnName("ClaimType");

            Property(p => p.ClaimValue)
                .HasColumnName("ClaimValue");
        }
    }
}
