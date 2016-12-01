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
using Microsoft.AspNet.Identity;
using Mark.DotNet.Data;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Entity type that represents role for the user.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class IdentityRole<TKey, TUserRole> : IRole<TKey>, IEntity
        where TUserRole : IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public IdentityRole()
        {
            Users = new List<TUserRole>();
        }

        /// <summary>
        /// Get or set primary key.
        /// </summary>
        public virtual TKey Id
        {
            get; set;
        }

        /// <summary>
        /// Get or set role name.
        /// </summary>
        public virtual string Name
        {
            get; set;
        }

        /// <summary>
        /// Navigation property for users in the role.
        /// </summary>
        public virtual ICollection<TUserRole> Users
        {
            get; private set;
        }
    }

    /// <summary>
    /// Entity type that represents role for the user.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityRole<TKey> : IdentityRole<TKey, IdentityUserRole<TKey>>
         where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public IdentityRole()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given role name.
        /// </summary>
        public IdentityRole(string roleName) : this()
        {
            this.Name = roleName;
        }
    }

    /// <summary>
    /// Entity type that represents role for the user.
    /// </summary>
    public class IdentityRole : IdentityRole<string, IdentityUserRole>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public IdentityRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initialize a new instance of the class with the given role name.
        /// </summary>
        public IdentityRole(string roleName) : this()
        {
            this.Name = roleName;
        }
    }
}
