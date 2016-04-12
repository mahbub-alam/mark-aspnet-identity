// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user login entity field names/identifiers.
    /// </summary>
    public class UserLoginFields
    {
        private UserLoginFields() { }

        /// <summary>
        /// Login provider field name/identifier.
        /// </summary>
        public const string LoginProvider = "LoginProvider";
        /// <summary>
        /// Provider key field name/identifier.
        /// </summary>
        public const string ProviderKey = "ProviderKey";
        /// <summary>
        /// User id field name/identifier.
        /// </summary>
        public const string UserId = "UserId";
    }
}
