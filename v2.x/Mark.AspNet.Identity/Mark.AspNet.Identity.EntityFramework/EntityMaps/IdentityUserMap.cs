// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents entity mapping for user entity.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    /// <typeparam name="TUserClaim">User claim type.</typeparam>
    public class IdentityUserMap<TUser, TKey, TUserLogin, TUserRole, TUserClaim>
        : EntityMap<TUser>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserMap(EntityConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => p.Id);
            Property(p => p.Id)
                .HasColumnName(Configuration[UserFields.Id]);
        }

        /// <summary>
        /// Map all non-key fields.
        /// </summary>
        protected override void MapFields()
        {
            Property(p => p.UserName)
                .HasColumnName(Configuration[UserFields.UserName])
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UK_User_UserName") { IsUnique = true }));

            Property(p => p.SecurityStamp)
                .HasColumnName(Configuration[UserFields.SecurityStamp]);

            Property(p => p.PasswordHash)
                .HasColumnName(Configuration[UserFields.PasswordHash]);

            Property(p => p.Email)
                .HasColumnName(Configuration[UserFields.Email])
                .HasMaxLength(64);

            Property(p => p.EmailConfirmed)
                .HasColumnName(Configuration[UserFields.EmailConfirmed]);

            Property(p => p.PhoneNumber)
                .HasColumnName(Configuration[UserFields.PhoneNumber])
                .HasMaxLength(15);

            Property(p => p.PhoneNumberConfirmed)
                .HasColumnName(Configuration[UserFields.PhoneNumberConfirmed]);

            Property(p => p.TwoFactorEnabled)
                .HasColumnName(Configuration[UserFields.TwoFactorEnabled]);

            Property(p => p.LockoutEnabled)
                .HasColumnName(Configuration[UserFields.LockoutEnabled]);

            Property(p => p.LockoutEndDateUtc)
                .HasColumnName(Configuration[UserFields.LockoutEndDateUtc]);

            Property(p => p.AccessFailedCount)
                .HasColumnName(Configuration[UserFields.AccessFailedCount]);
        }

        /// <summary>
        /// Map relationship among entities.
        /// </summary>
        protected override void MapRelationships()
        {
            HasMany(p => p.Roles)
                .WithRequired()
                .HasForeignKey(p => p.UserId);

            HasMany(p => p.Claims)
                .WithRequired()
                .HasForeignKey(p => p.UserId);

            HasMany(p => p.Logins)
                .WithRequired()
                .HasForeignKey(p => p.UserId);
        }
    }

    /// <summary>
    /// Represents entity mapping for user entity.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserMap<TUser, TKey>
        : IdentityUserMap<TUser, TKey, 
            IdentityUserLogin<TKey>, 
            IdentityUserRole<TKey>, 
            IdentityUserClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserMap(EntityConfiguration configuration) : base(configuration)
        {
        }
    }
}
