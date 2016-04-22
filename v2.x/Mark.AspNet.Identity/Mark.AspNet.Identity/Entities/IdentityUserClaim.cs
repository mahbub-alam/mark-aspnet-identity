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
using Mark.Data;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Entity type that represents one specific user claim.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserClaim<TKey> : IEntity where TKey : struct, IEquatable<TKey>
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
