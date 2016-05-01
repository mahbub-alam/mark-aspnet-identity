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
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Mark.DotNet.Threading.Tasks;
using Mark.DotNet;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents default storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class RoleStore<TRole, TKey, TUserRole> : RoleStoreBase<TRole, TKey, TUserRole>, IQueryableRoleStore<TRole, TKey>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        private DbContext _context;
        private EntityStore<TRole, TKey> _roleStore;

        /// <summary>
        /// Initialize a new instance of the class with the database context.
        /// </summary>
        /// <param name="context">Database context.</param>
        public RoleStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("'context' parameter null");
            }

            _context = context;
            AutoSaveChanges = true;
            _roleStore = new EntityStore<TRole, TKey>(context);
        }

        /// <summary>
        /// Get database context.
        /// </summary>
        public DbContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Whether to dispose database context when this object is disposed.
        /// </summary>
        public bool DisposeContext
        {
            get; set;
        }

        /// <summary>
        /// Get the underlying role entity set.
        /// </summary>
        public IQueryable<TRole> Roles
        {
            get { return _roleStore.EntitySet; }
        }

        /// <summary>
        /// Whether Create/Update/Delete methods will call Context.SaveChangesAsync() method automatically.
        /// </summary>
        public bool AutoSaveChanges
        {
            get; set;
        }

        private async Task SaveChangesAsync()
        {
            if (AutoSaveChanges)
            {
                await _context.SaveChangesAsync().WithCurrentCulture();
            }
        }

        /// <summary>
        /// Create new role.
        /// </summary>
        /// <param name="role">Role to be added for storing.</param>
        /// <returns></returns>
        public override async Task CreateAsync(TRole role)
        {
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("'role' parameter null");
            }

            _roleStore.Create(role);
            await SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        /// Delete specified role.
        /// </summary>
        /// <param name="role">Role to be deleted.</param>
        /// <returns></returns>
        public override async Task DeleteAsync(TRole role)
        {
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("'role' parameter null");
            }

            _roleStore.Delete(role);
            await SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        /// Find role by id.
        /// </summary>
        /// <param name="roleId">Target role id.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public override async Task<TRole> FindByIdAsync(TKey roleId)
        {
            ThrowIfDisposed();

            return await _roleStore.FindByIdAsync(roleId).WithCurrentCulture();
        }

        /// <summary>
        /// Find the role by it's name.
        /// </summary>
        /// <param name="roleName">Target role's name.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public override async Task<TRole> FindByNameAsync(string roleName)
        {
            ThrowIfDisposed();

            TRole role = null;

            if (!String.IsNullOrWhiteSpace(roleName))
            {
                role = await this.Roles
                    .Where(p => p.Name.ToLower() == roleName.ToLower())
                    .SingleOrDefaultAsync().WithCurrentCulture();
            }

            return role;
        }

        /// <summary>
        /// Update specified role.
        /// </summary>
        /// <param name="role">Role to be updated.</param>
        /// <returns></returns>
        public override async Task UpdateAsync(TRole role)
        {
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("'role' parameter null");
            }

            _roleStore.Update(role);
            await SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            if (DisposeContext)
            {
                _context.Dispose();
            }

            _roleStore.Dispose();
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _context = null;
            _roleStore = null;
        }
    }

    /// <summary>
    /// Represents default storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class RoleStore<TRole, TKey> : RoleStore<TRole, TKey, IdentityUserRole<TKey>>
        where TRole : IdentityRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with the database context.
        /// </summary>
        /// <param name="context">Database context.</param>
        public RoleStore(DbContext context) : base(context)
        {
        }
    }
}
