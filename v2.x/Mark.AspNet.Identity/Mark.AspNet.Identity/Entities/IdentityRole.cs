// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Entity type that represents role for the user.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class IdentityRole<TKey, TUserRole> : IRole<TKey>, IEntity 
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public IdentityRole()
        {
            Users = new List<TUserRole>();
        }

        /// <summary>
        /// Get or set primary key.
        /// </summary>
        public virtual TKey Id
        {
            get; set;
        }

        /// <summary>
        /// Get or set role name.
        /// </summary>
        public virtual string Name
        {
            get; set;
        }

        /// <summary>
        /// Navigation property for users in the role.
        /// </summary>
        public virtual ICollection<TUserRole> Users
        {
            get; set;
        }
    }

    /// <summary>
    /// Entity type that represents role for the user.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityRole<TKey> : IdentityRole<TKey, IdentityUserRole<TKey>>
         where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public IdentityRole()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given role name.
        /// </summary>
        public IdentityRole(string roleName) : this()
        {
            this.Name = roleName;
        }
    }
}
