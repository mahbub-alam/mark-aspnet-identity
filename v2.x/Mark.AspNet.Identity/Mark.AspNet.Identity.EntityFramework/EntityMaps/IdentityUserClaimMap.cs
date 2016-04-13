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
    /// Represents entity mapping user claim entity.
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
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserClaimMap(EntityConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => p.Id);
            Property(p => p.Id)
                .HasColumnName(Configuration[UserClaimFields.Id]);
        }

        /// <summary>
        /// Map all non-key fields.
        /// </summary>
        protected override void MapFields()
        {
            Property(p => p.UserId)
                .HasColumnName(Configuration[UserClaimFields.UserId]);

            Property(p => p.ClaimType)
                .HasMaxLength(255)
                .HasColumnName(Configuration[UserClaimFields.ClaimType]);

            Property(p => p.ClaimValue)
                .HasMaxLength(255)
                .HasColumnName(Configuration[UserClaimFields.ClaimValue]);
        }
    }
}
