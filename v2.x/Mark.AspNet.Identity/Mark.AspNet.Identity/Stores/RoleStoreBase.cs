// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Mark.Core;

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
        where TKey : struct, IEquatable<TKey>
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
        where TKey : struct, IEquatable<TKey>
    {
    }
}
