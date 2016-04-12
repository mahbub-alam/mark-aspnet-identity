// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Entity type that represents one specific user claim.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserClaim<TKey> : IEntity where TKey : struct
    {
        /// <summary>
        /// Get or set primary key.
        /// </summary>
        public virtual TKey Id
        {
            get; set;
        }

        /// <summary>
        /// Get or set claim type.
        /// </summary>
        public virtual string ClaimType
        {
            get; set;
        }

        /// <summary>
        /// Get or set claim value.
        /// </summary>
        public virtual string ClaimValue
        {
            get; set;
        }

        /// <summary>
        /// Get or set user id for the user who owns this login.
        /// </summary>
        public virtual TKey UserId
        {
            get; set;
        }
    }
}
