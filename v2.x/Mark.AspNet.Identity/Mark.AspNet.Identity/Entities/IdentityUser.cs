// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Mark.Data;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Entity type that represents user.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User logins type.</typeparam>
    /// <typeparam name="TUserRole">User roles type.</typeparam>
    /// <typeparam name="TUserClaim">User claims type.</typeparam>
    public class IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim> : IUser<TKey>, IEntity
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public IdentityUser()
        {
            Claims = new List<TUserClaim>();
            Roles = new List<TUserRole>();
            Logins = new List<TUserLogin>();

        }

        /// <summary>
        /// Get or set primary key.
        /// </summary>
        public virtual TKey Id
        {
            get; set;
        }

        /// <summary>
        /// Get or set user's username.
        /// </summary>
        public virtual string UserName
        {
            get; set;
        }

        /// <summary>
        /// Get or set the salted/hashed form of the user password.
        /// </summary>
        public virtual string PasswordHash
        {
            get; set;
        }

        /// <summary>
        /// Get or set a random value that should change whenever a 
        /// user's credentials have changed (password changed, login removed).
        /// </summary>
        public virtual string SecurityStamp
        {
            get; set;
        }

        /// <summary>
        /// Get or set user's email.
        /// </summary>
        public virtual string Email
        {
            get; set;
        }

        /// <summary>
        /// Get or set whether email is confirmed or not.
        /// </summary>
        public virtual bool EmailConfirmed
        {
            get; set;
        }

        /// <summary>
        /// Get or set user's phone number.
        /// </summary>
        public virtual string PhoneNumber
        {
            get; set;
        }

        /// <summary>
        /// Get or set whether phone number is confirmed or not.
        /// </summary>
        public virtual bool PhoneNumberConfirmed
        {
            get; set;
        }

        /// <summary>
        /// Get or set whether two factor is enabled or not.
        /// </summary>
        public virtual bool TwoFactorEnabled
        {
            get; set;
        }

        /// <summary>
        /// Get or set whether lockout is enabled or not.
        /// </summary>
        public virtual bool LockoutEnabled
        {
            get; set;
        }

        /// <summary>
        /// Get or set when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc
        {
            get; set;
        }

        /// <summary>
        /// Get or set value that is used to record failures for the purposes of lockout.
        /// </summary>
        public virtual int AccessFailedCount
        {
            get; set;
        }

        /// <summary>
        /// Navigation property for user's claims.
        /// </summary>
        public virtual ICollection<TUserClaim> Claims
        {
            get; private set;
        }

        /// <summary>
        /// Navigation property for user's logins.
        /// </summary>
        public virtual ICollection<TUserLogin> Logins
        {
            get; private set;
        }

        /// <summary>
        /// Navigation property for user's roles.
        /// </summary>
        public virtual ICollection<TUserRole> Roles
        {
            get; private set;
        }


    }

    /// <summary>
    /// Entity type that represents user.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUser<TKey> : 
        IdentityUser<TKey, IdentityUserLogin<TKey>, IdentityUserRole<TKey>, IdentityUserClaim<TKey>>
         where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public IdentityUser()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given username.
        /// </summary>
        /// <param name="userName">Username to assign.</param>
        public IdentityUser(string userName) : this()
        {
            this.UserName = userName;
        }
    }
}
