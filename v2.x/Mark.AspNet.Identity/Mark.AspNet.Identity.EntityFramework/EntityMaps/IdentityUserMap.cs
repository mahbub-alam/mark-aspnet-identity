// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents mapping configuration for 'User' table.
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
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        public IdentityUserMap(string tableName) : base(tableName)
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
            Property(p => p.UserName)
                .HasColumnName("UserName")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UK_User_UserName") { IsUnique = true }));

            Property(p => p.SecurityStamp)
                .HasColumnName("SecurityStamp");

            Property(p => p.PasswordHash)
                .HasColumnName("PasswordHash");

            Property(p => p.Email)
                .HasColumnName("Email")
                .HasMaxLength(64);

            Property(p => p.EmailConfirmed)
                .HasColumnName("EmailConfirmed");

            Property(p => p.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(15);

            Property(p => p.PhoneNumberConfirmed)
                .HasColumnName("PhoneNumberConfirmed");

            Property(p => p.TwoFactorEnabled)
                .HasColumnName("TwoFactorEnabled");

            Property(p => p.LockoutEnabled)
                .HasColumnName("LockoutEnabled");

            Property(p => p.LockoutEndDateUtc)
                .HasColumnName("LockoutEndDateUtc");

            Property(p => p.AccessFailedCount)
                .HasColumnName("AccessFailedCount");
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
    /// Represents mapping configuration for 'User' table.
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
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        public IdentityUserMap(string tableName) : base(tableName)
        {
        }
    }
}
