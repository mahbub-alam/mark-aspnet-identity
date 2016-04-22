﻿//
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
