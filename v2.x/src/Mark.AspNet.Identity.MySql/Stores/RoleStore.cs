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
using Mark.DotNet;
using Mark.DotNet.Data;
using Mark.DotNet.Threading.Tasks;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents default storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class RoleStore<TRole, TKey, TUserRole> : RoleStoreBase<TRole, TKey, TUserRole>
        where TRole : IdentityRole<TKey, TUserRole>, new()
        where TUserRole : IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private IUnitOfWork _unitOfWork;
        private RoleRepository<TRole, TKey, TUserRole> _repo;

        /// <summary>
        /// Initialize a new instance of the class with unit of work.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference.</param>
        public RoleStore(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("UnitOfWork null");
            }

            _unitOfWork = unitOfWork;
            AutoSaveChanges = true;
            _repo = new RoleRepository<TRole, TKey, TUserRole>(_unitOfWork);
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
                _unitOfWork.SaveChanges();
                await Task.FromResult(0);
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

            _repo.Add(role);
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

            _repo.Remove(role);
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

            TRole role = _repo.FindById(roleId);

            return await Task.FromResult(role);
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
                role = _repo.FindByName(roleName);
            }

            return await Task.FromResult(role);
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

            _repo.Change(role);
            await SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _repo = null;
            _unitOfWork = null;
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="Disposable.DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            _repo.Dispose();
        }
    }

    /// <summary>
    /// Represents default storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class RoleStore<TRole, TKey> : RoleStore<TRole, TKey, IdentityUserRole<TKey>>
        where TRole : IdentityRole<TKey, IdentityUserRole<TKey>>, new()
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with unit of work.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference.</param>
        public RoleStore(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    /// <summary>
    /// Represents default storage class for 'Role' management.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    public class RoleStore<TRole> : RoleStore<TRole, string, IdentityUserRole>
        where TRole : IdentityRole, new()
    {
        /// <summary>
        /// Initialize a new instance of the class with unit of work.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference.</param>
        public RoleStore(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
