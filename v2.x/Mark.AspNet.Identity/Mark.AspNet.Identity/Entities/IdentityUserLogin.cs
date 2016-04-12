// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Entity type for a user's login (i.e. facebook, google).
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserLogin<TKey> : IEntity where TKey : struct
    {
        /// <summary>
        /// The login provider for the login (i.e. facebook, google).
        /// </summary>
        public virtual string LoginProvider
        {
            get; set;
        }

        /// <summary>
        /// Key representing the login for the provider.
        /// </summary>
        public virtual string ProviderKey
        {
            get; set;
        }

        /// <summary>
        /// User Id for the user who owns this login.
        /// </summary>
        public virtual TKey UserId
        {
            get; set;
        }
    }
}
