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
using Mark.DotNet.Data;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Entity type for a user's login (i.e. facebook, google).
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserLogin<TKey> : IEntity where TKey : IEquatable<TKey>
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

    /// <summary>
    /// Entity type for a user's login (i.e. facebook, google).
    /// </summary>
    public class IdentityUserLogin : IdentityUserLogin<string>
    {
    }
}
