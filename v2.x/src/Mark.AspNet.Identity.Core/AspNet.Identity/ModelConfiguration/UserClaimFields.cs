//
// Copyright 2016, Mahbub Alam (mahbub002@gmail.com)
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
