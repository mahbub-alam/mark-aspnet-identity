// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents entity names/identifiers.
    /// </summary>
    public class Entities
    {
        private Entities() { }

        /// <summary>
        /// Role entity name/identifier.
        /// </summary>
        public const string Role = "Role";
        /// <summary>
        /// User entity name/identifier.
        /// </summary>
        public const string User = "User";
        /// <summary>
        /// User login entity name/identifier.
        /// </summary>
        public const string UserLogin = "UserLogin";
        /// <summary>
        /// User role entity name/identifier.
        /// </summary>
        public const string UserRole = "UserRole";
        /// <summary>
        /// User claim entity name/identifier.
        /// </summary>
        public const string UserClaim = "UserClaim";
    }
}
