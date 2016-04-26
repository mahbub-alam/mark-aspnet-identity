//
// Copyright 2016, Mahbub Alam (mahbub002@ymail.com)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user entity field names/identifiers.
    /// </summary>
    public class UserFields
    {
        private UserFields() { }

        /// <summary>
        /// id field name/identifier.
        /// </summary>
        public const string Id = "Id";
        /// <summary>
        /// Username field name/identifier.
        /// </summary>
        public const string UserName = "UserName";
        /// <summary>
        /// Password hash field name/identifier.
        /// </summary>
        public const string PasswordHash = "PasswordHash";
        /// <summary>
        /// Security stamp field name/identifier.
        /// </summary>
        public const string SecurityStamp = "SecurityStamp";
        /// <summary>
        /// Email field name/identifier.
        /// </summary>
        public const string Email = "Email";
        /// <summary>
        /// Email confirmed field name/identifier.
        /// </summary>
        public const string EmailConfirmed = "EmailConfirmed";
        /// <summary>
        /// Phone number field name/identifier.
        /// </summary>
        public const string PhoneNumber = "PhoneNumber";
        /// <summary>
        /// Phone number confirmed field name/identifier.
        /// </summary>
        public const string PhoneNumberConfirmed = "PhoneNumberConfirmed";
        /// <summary>
        /// Two factor enabled field name/identifier.
        /// </summary>
        public const string TwoFactorEnabled = "TwoFactorEnabled";
        /// <summary>
        /// Lockout enabled field name/identifier.
        /// </summary>
        public const string LockoutEnabled = "LockoutEnabled";
        /// <summary>
        /// Lockout end date UTC field name/identifier.
        /// </summary>
        public const string LockoutEndDateUtc = "LockoutEndDateUtc";
        /// <summary>
        /// Access failed count field name/identifier.
        /// </summary>
        public const string AccessFailedCount = "AccessFailedCount";
    }
}
