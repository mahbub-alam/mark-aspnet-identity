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
    /// Represents entity mapping user login entity.
    /// </summary>
    /// <typeparam name="TUserLogin">User login type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserLoginMap<TUserLogin, TKey> : EntityMap<TUserLogin> 
        where TUserLogin : IdentityUserLogin<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserLoginMap(EntityConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => new
            {
               p.LoginProvider,
               p.ProviderKey,
               p.UserId
            });

            Property(p => p.LoginProvider)
                .HasMaxLength(128)
                .HasColumnName(Configuration[UserLoginFields.LoginProvider]);

            Property(p => p.ProviderKey)
                .HasMaxLength(128)
                .HasColumnName(Configuration[UserLoginFields.ProviderKey]);

            Property(p => p.UserId)
                .HasColumnName(Configuration[UserLoginFields.UserId]);
        }
    }
}
