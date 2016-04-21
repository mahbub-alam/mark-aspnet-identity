﻿// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Data;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// EntityType that represents a user belonging to a role.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserRole<TKey> : IEntity where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        ///     RoleId for the role
        /// </summary>
        public virtual TKey RoleId
        {
            get; set;
        }

        /// <summary>
        ///     UserId for the user that is in the role
        /// </summary>
        public virtual TKey UserId
        {
            get; set;
        }
    }
}
