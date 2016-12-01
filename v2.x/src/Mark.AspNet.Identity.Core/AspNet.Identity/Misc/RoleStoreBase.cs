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
using Mark.DotNet;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents base storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public abstract class RoleStoreBase<TRole, TKey, TUserRole> : Disposable, IRoleStore<TRole, TKey>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserRole : IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Create new role.
        /// </summary>
        /// <param name="role">Role to be added for storing.</param>
        /// <returns></returns>
        public abstract Task CreateAsync(TRole role);

        /// <summary>
        /// Delete specified role.
        /// </summary>
        /// <param name="role">Role to be deleted.</param>
        /// <returns></returns>
        public abstract Task DeleteAsync(TRole role);

        /// <summary>
        /// Update specified role.
        /// </summary>
        /// <param name="role">Role to be updated.</param>
        /// <returns></returns>
        public abstract Task UpdateAsync(TRole role);

        /// <summary>
        /// Find role by id.
        /// </summary>
        /// <param name="roleId">Target role id.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public abstract Task<TRole> FindByIdAsync(TKey roleId);

        /// <summary>
        /// Find the role by it's name.
        /// </summary>
        /// <param name="roleName">Target role's name.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public abstract Task<TRole> FindByNameAsync(string roleName);

    }

    /// <summary>
    /// Represents base storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public abstract class RoleStoreBase<TRole, TKey> : RoleStoreBase<TRole, TKey, IdentityUserRole<TKey>>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
    }

    /// <summary>
    /// Represents base storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    public abstract class RoleStoreBase<TRole> : RoleStoreBase<TRole, string, IdentityUserRole>
        where TRole : IdentityRole
    {
    }
}
