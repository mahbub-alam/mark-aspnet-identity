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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Mark.DotNet.Threading.Tasks;
using Mark.DotNet;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents default storage class for 'User' management.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    /// <typeparam name="TUserClaim">User claim type.</typeparam>
    public class UserStore<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>
        : UserStoreBase<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>, IQueryableUserStore<TUser, TKey>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private DbContext _context;
        private EntityStore<TRole, TKey> _roleStore;
        private EntityStore<TUser, TKey> _userStore;
        private DbSet<TUserLogin> _userLogins;
        private DbSet<TUserRole> _userRoles;
        private DbSet<TUserClaim> _userClaims;

        /// <summary>
        /// Initialize a new instance of the class with the database context.
        /// </summary>
        /// <param name="context">Database context.</param>
        public UserStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("'context' parameter null");
            }

            _context = context;
            AutoSaveChanges = true;
            _roleStore = new EntityStore<TRole, TKey>(context);
            _userStore = new EntityStore<TUser, TKey>(context);
            _userLogins = _context.Set<TUserLogin>();
            _userRoles = _context.Set<TUserRole>();
            _userClaims = _context.Set<TUserClaim>();
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="Disposable.DisposeExtra()"/> method since, that method will 
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
            _userStore.Dispose();
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
            _userStore = null;
            _userLogins = null;
            _userRoles = null;
            _userClaims = null;
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
        /// Get the underlying user entity set.
        /// </summary>
        public IQueryable<TUser> Users
        {
            get { return _userStore.EntitySet; }
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
            if (this.AutoSaveChanges)
            {
                await _context.SaveChangesAsync().WithCurrentCulture();
            }
        }

        /// <summary>
        /// Add new user claim.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="claim">Claim to be added.</param>
        /// <returns></returns>
        public override async Task AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("'claim' parameter null");
            }

            TUserClaim userClaim = new TUserClaim();
            userClaim.UserId = user.Id;
            userClaim.ClaimType = claim.Type;
            userClaim.ClaimValue = claim.Value;

            _userClaims.Add(userClaim);

            await Task.FromResult(0);
        }

        /// <summary>
        /// Add new user login from remote login provider.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="login">Login to be added.</param>
        /// <returns></returns>
        public override async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            if (login == null)
            {
                throw new ArgumentNullException("'login' parameter null");
            }

            TUserLogin userLogin = new TUserLogin();
            userLogin.UserId = user.Id;
            userLogin.LoginProvider = login.LoginProvider;
            userLogin.ProviderKey = login.ProviderKey;

            _userLogins.Add(userLogin);

            await Task.FromResult(0);
        }

        /// <summary>
        /// Add user to new role.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="roleName">Target role.</param>
        /// <returns></returns>
        public override async Task AddToRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("'roleName' parameter cannot be null or empty");
            }

            TRole role = await _roleStore.EntitySet.Where(p => p.Name.ToLower() == roleName.ToLower())
                .SingleOrDefaultAsync().WithCurrentCulture();

            if (role == null)
            {
                throw new InvalidOperationException("Given role name not found");
            }

            TUserRole userRole = new TUserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = role.Id;

            _userRoles.Add(userRole);
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="user">User to be created.</param>
        /// <returns></returns>
        public override async Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            _userStore.Create(user);
            await SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        /// Delete specified user.
        /// </summary>
        /// <param name="user">User to be deleted.</param>
        /// <returns></returns>
        public override async Task DeleteAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            _userStore.Delete(user);
            await SaveChangesAsync().WithCurrentCulture();
        }

        private async Task IncludeLoginsAsync(TUser user)
        {
            DbEntityEntry<TUser> userEntry = _context.Entry(user);

            if (!userEntry.Collection(p => p.Logins).IsLoaded)
            {
                await _userLogins.Where(p => p.UserId.Equals(user.Id))
                    .LoadAsync().WithCurrentCulture();
                userEntry.Collection(p => p.Logins).IsLoaded = true;
            }
        }

        private async Task IncludeRolesAsync(TUser user)
        {
            DbEntityEntry<TUser> userEntry = _context.Entry(user);

            if (!userEntry.Collection(p => p.Roles).IsLoaded)
            {
                await _userRoles.Where(p => p.UserId.Equals(user.Id))
                    .LoadAsync().WithCurrentCulture();
                userEntry.Collection(p => p.Roles).IsLoaded = true;
            }
        }

        private async Task IncludeClaimsAsync(TUser user)
        {
            DbEntityEntry<TUser> userEntry = _context.Entry(user);

            if (!userEntry.Collection(p => p.Claims).IsLoaded)
            {
                await _userClaims.Where(p => p.UserId.Equals(user.Id))
                    .LoadAsync().WithCurrentCulture();
                userEntry.Collection(p => p.Claims).IsLoaded = true;
            }
        }

        private async Task<TUser> GetUserAggregateAsync(Expression<Func<TUser, bool>> filter)
        {
            TUser user = await _userStore.EntitySet.Where(filter).SingleOrDefaultAsync().WithCurrentCulture();

            if (user != null)
            {
                await IncludeLoginsAsync(user).WithCurrentCulture();
                await IncludeRolesAsync(user).WithCurrentCulture();
                await IncludeClaimsAsync(user).WithCurrentCulture();
            }

            return user;
        }

        /// <summary>
        /// Find user by remote login information.
        /// </summary>
        /// <param name="login">Remote login information.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public override async Task<TUser> FindAsync(UserLoginInfo login)
        {
            ThrowIfDisposed();

            if (login == null)
            {
                throw new ArgumentNullException("'login' parameter null");
            }

            TUser user;
            TUserLogin userLogin = await _userLogins.Where(p => (p.LoginProvider == login.LoginProvider) &&
            (p.ProviderKey == login.ProviderKey)).SingleOrDefaultAsync();

            if (userLogin == null)
            {
                user = default(TUser);
            }
            else
            {
                user = await GetUserAggregateAsync(p => p.Id.Equals(userLogin.UserId)).WithCurrentCulture();
            }

            return user;
        }

        /// <summary>
        /// Find user by email.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public override async Task<TUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();

            TUser user = null;

            if (!String.IsNullOrWhiteSpace(email))
            {
                user = await GetUserAggregateAsync(p => p.Email.ToLower() == email.ToLower()).WithCurrentCulture();
            }

            return user;
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public override async Task<TUser> FindByIdAsync(TKey userId)
        {
            ThrowIfDisposed();

            TUser user = await GetUserAggregateAsync(p => p.Id.Equals(userId)).WithCurrentCulture();

            return user;
        }

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="userName">User's username.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public override async Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();

            TUser user = null;

            if (!String.IsNullOrWhiteSpace(userName))
            {
                user = await GetUserAggregateAsync(p => p.UserName.ToLower() == userName.ToLower())
                .WithCurrentCulture();
            }

            return user;
        }

        /// <summary>
        /// Get access failed count value.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns current value.</returns>
        public override async Task<int> GetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Get a list of claims belong to the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns a list of claims if found; otherwise, returns an empty list.</returns>
        public override async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            await IncludeClaimsAsync(user).WithCurrentCulture();

            return user.Claims.Select(p => new Claim(p.ClaimType, p.ClaimValue)).ToList();
        }

        /// <summary>
        /// Get user's email.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns email.</returns>
        public override async Task<string> GetEmailAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.Email);
        }

        /// <summary>
        /// Get user's email confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns confirmation.</returns>
        public override async Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Get whether the account lockout option is enabled for the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns true if enabled; false if disabled.</returns>
        public override async Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Get when the lockout will end.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns date of the lockout ending.</returns>
        public override async Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            DateTimeOffset dateTimeOffset;

            if (user.LockoutEndDateUtc.HasValue)
            {
                dateTimeOffset = new DateTimeOffset(DateTime.SpecifyKind(
                    user.LockoutEndDateUtc.Value, DateTimeKind.Utc));
            }
            else
            {
                dateTimeOffset = new DateTimeOffset();
            }

            return await Task.FromResult(dateTimeOffset);
        }

        /// <summary>
        /// Get a list of logins made by the user using remote login provider.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns a list of logins if found; otherwise, returns an empty list.</returns>
        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            await IncludeLoginsAsync(user).WithCurrentCulture();

            return user.Logins.Select(p => new UserLoginInfo(p.LoginProvider, p.ProviderKey)).ToList();
        }

        /// <summary>
        /// Get user's password hash.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns password hash.</returns>
        public override async Task<string> GetPasswordHashAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Get user's phone number.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns phone number.</returns>
        public override async Task<string> GetPhoneNumberAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Get user's phone number confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Retrurns true if confirmed; otherwise, returns false.</returns>
        public override async Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Get a list of roles the user belongs to.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns a list of roles if found; otherwise, returns an empty list.</returns>
        public override async Task<IList<string>> GetRolesAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            // Role ids that belong to the given user from user roles table
            var userRolesQuery = _userRoles.Where(p => p.UserId.Equals(user.Id));
            // Get role names by joining role ids with ids in role table.
            var roleNamesQuery = userRolesQuery.Join(_roleStore.EntitySet, p => p.RoleId, q => q.Id, (p, q) => q.Name);

            return await roleNamesQuery.ToListAsync().WithCurrentCulture();
        }

        /// <summary>
        /// Get user's security stamp.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns security stamp.</returns>
        public override async Task<string> GetSecurityStampAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Get whether the two factor authentication is enabled.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns true if enabled; otherwise, returns false.</returns>
        public override async Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            return await Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Check whether the user has password.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns true if user has password; otherwise, returns false.</returns>
        public override async Task<bool> HasPasswordAsync(TUser user)
        {
            ThrowIfDisposed();

            return await Task.FromResult(user.PasswordHash != null);
        }

        /// <summary>
        /// Record the event when an attempt to access the user account has failed.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns new failed count.</returns>
        public override async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.AccessFailedCount = user.AccessFailedCount + 1;
            return await Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Check whether the user belongs to the given role.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="roleName">Role name.</param>
        /// <returns>Returns true if belongs; otherwise, returns false.</returns>
        public override async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("'roleName' parameter cannot be null or empty");
            }

            bool inRole = false;
            TRole role = await _roleStore.EntitySet.Where(p => p.Name.ToLower() == roleName.ToLower())
                .SingleOrDefaultAsync().WithCurrentCulture();

            if (role == null)
            {
                return inRole;
            }

            inRole = await _userRoles.AnyAsync(p => p.RoleId.Equals(role.Id) && p.UserId.Equals(user.Id))
                .WithCurrentCulture();

            return inRole;
        }

        /// <summary>
        /// Remove user claims.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="claim">Target claim.</param>
        /// <returns></returns>
        public override async Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("'claim' parameter null");
            }

            IEnumerable<TUserClaim> list = null;

            if (_context.Entry(user).Collection(p => p.Claims).IsLoaded)
            {
                list = user.Claims.Where(p => p.ClaimValue == claim.Value &&
                p.ClaimType == claim.Type).ToList();
            }
            else
            {
                list = await _userClaims.Where(p => p.ClaimValue == claim.Value &&
                p.ClaimType == claim.Type && p.UserId.Equals(user.Id)).ToListAsync().WithCurrentCulture();
            }

            _userClaims.RemoveRange(list);
        }

        /// <summary>
        /// Remove the user from the given role.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="roleName">Role name.</param>
        /// <returns></returns>
        public override async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("'roleName' parameter cannot be null or empty");
            }

            TRole role = await _roleStore.EntitySet.Where(p => p.Name.ToLower() == roleName.ToLower())
                .SingleOrDefaultAsync().WithCurrentCulture();

            if (role == null)
            {
                return;

            }

            TUserRole userRole = await _userRoles.Where(p => p.RoleId.Equals(role.Id) && p.UserId.Equals(user.Id))
                .SingleOrDefaultAsync().WithCurrentCulture();

            if (userRole != null)
            {
                _userRoles.Remove(userRole);
            }
        }

        /// <summary>
        /// Remove user's remote login that matchs the given remote login provider.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="login">Remote login information.</param>
        /// <returns></returns>
        public override async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            if (login == null)
            {
                throw new ArgumentNullException("'login' parameter null");
            }

            TUserLogin userLogin = null;

            if (_context.Entry(user).Collection(p => p.Logins).IsLoaded)
            {
                userLogin = user.Logins.Where(p => p.LoginProvider == login.LoginProvider &&
                p.ProviderKey == login.ProviderKey).SingleOrDefault();
            }
            else
            {
                userLogin = await _userLogins.Where(p => p.LoginProvider == login.LoginProvider &&
                p.ProviderKey == login.ProviderKey && p.UserId.Equals(user.Id))
                .SingleOrDefaultAsync().WithCurrentCulture();
            }

            if (userLogin != null)
            {
                _userLogins.Remove(userLogin);
            }
        }

        /// <summary>
        /// Reset the record that tracks the user account access failed attempts. It is 
        /// usually done after the account is successfully accessed.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns></returns>
        public override async Task ResetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.AccessFailedCount = 0;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set user's email.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="email">User's new email.</param>
        /// <returns></returns>
        public override async Task SetEmailAsync(TUser user, string email)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.Email = email;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set user's email confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="confirmed">Whether confirmed.</param>
        /// <returns></returns>
        public override async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.EmailConfirmed = confirmed;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set whether given user's account lockout option is enabled.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="enabled">If enabled.</param>
        /// <returns></returns>
        public override async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.LockoutEnabled = enabled;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set when the lockout will end for the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="lockoutEnd">Lockout end date.</param>
        /// <returns></returns>
        public override async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            DateTime? value = null;

            if (lockoutEnd != DateTimeOffset.MinValue)
            {
                value = new DateTime?(lockoutEnd.UtcDateTime);
            }

            user.LockoutEndDateUtc = value;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set user's password hash.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="passwordHash">Password hash.</param>
        /// <returns></returns>
        public override async Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.PasswordHash = passwordHash;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set user's phone number.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="phoneNumber">New phone number.</param>
        /// <returns></returns>
        public override async Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.PhoneNumber = phoneNumber;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set user's phone number confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="confirmed">Whether confirmed.</param>
        /// <returns></returns>
        public override async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.PhoneNumberConfirmed = confirmed;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set user's security stamp.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="stamp">Security stamp.</param>
        /// <returns></returns>
        public override async Task SetSecurityStampAsync(TUser user, string stamp)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.SecurityStamp = stamp;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Set whether to enable two factor authentication for the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="enabled">Whether enabled.</param>
        /// <returns></returns>
        public override async Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            user.TwoFactorEnabled = enabled;

            await Task.FromResult(0);
        }

        /// <summary>
        /// Update the given user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns></returns>
        public override async Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

            _userStore.Update(user);
            await SaveChangesAsync().WithCurrentCulture();
        }
    }

    /// <summary>
    /// Represents default storage class for 'User' management.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class UserStore<TUser, TKey>
        : UserStore<
            TUser,
            IdentityRole<TKey>,
            TKey,
            IdentityUserLogin<TKey>,
            IdentityUserRole<TKey>,
            IdentityUserClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with the database context.
        /// </summary>
        /// <param name="context">Database context.</param>
        public UserStore(DbContext context) : base(context)
        {
        }
    }

    /// <summary>
    /// Represents default storage class for 'User' management.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    public class UserStore<TUser>
        : UserStore<
            TUser,
            IdentityRole,
            string,
            IdentityUserLogin,
            IdentityUserRole,
            IdentityUserClaim>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Initialize a new instance of the class with the database context.
        /// </summary>
        /// <param name="context">Database context.</param>
        public UserStore(DbContext context) : base(context)
        {
        }
    }
}
