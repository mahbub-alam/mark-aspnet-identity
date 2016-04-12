// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user claim entity field names/identifers.
    /// </summary>
    public class UserClaimFields
    {
        private UserClaimFields() { }

        /// <summary>
        /// Id field name/identifier.
        /// </summary>
        public const string Id = "Id";
        /// <summary>
        /// Claim type field name/identifier.
        /// </summary>
        public const string ClaimType = "ClaimType";
        /// <summary>
        /// Claim value field name/identifier.
        /// </summary>
        public const string ClaimValue = "ClaimValue";
        /// <summary>
        /// User id field name/identifier.
        /// </summary>
        public const string UserId = "UserId";
    }
}
