// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Mark.AspNet.Identity.Common;
using System.Security.Claims;

namespace Mark.AspNet.Identity.MySql
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
        : UserStoreBase<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>, new()
        where TRole : IdentityRole<TKey, TUserRole>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TKey : struct
    {
        private IUnitOfWork _unitOfWork;
        private UserRepository<TUser, TKey, TUserLogin, TUserRole, TUserClaim> _userRepo;
        private RoleRepository<TRole, TKey, TUserRole> _roleRepo;
        private UserLoginRepository<TUserLogin, TKey> _userLoginRepo;
        private UserRoleRepository<TUserRole, TKey> _userRoleRepo;
        private UserClaimRepository<TUserClaim, TKey> _userClaimRepo;

        /// <summary>
        /// Initialize a new instance of the class with unit of work.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference.</param>
        public UserStore(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("UnitOfWork null");
            }

            _unitOfWork = unitOfWork;
            AutoSaveChanges = true;
            _userRepo = new UserRepository<TUser, TKey, TUserLogin, TUserRole, TUserClaim>(_unitOfWork);
            _roleRepo = new RoleRepository<TRole, TKey, TUserRole>(_unitOfWork);
            _userLoginRepo = new UserLoginRepository<TUserLogin, TKey>(_unitOfWork);
            _userRoleRepo = new UserRoleRepository<TUserRole, TKey>(_unitOfWork);
            _userClaimRepo = new UserClaimRepository<TUserClaim, TKey>(_unitOfWork);
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            // Do nothing
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _unitOfWork = null;
            _userRepo = null;
            _roleRepo = null;
            _userLoginRepo = null;
            _userRoleRepo = null;
            _userClaimRepo = null;
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

            _userClaimRepo.Add(userClaim);

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

            _userLoginRepo.Add(userLogin);

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

            TRole role = _roleRepo.FindByName(roleName);

            if (role == null)
            {
                throw new InvalidOperationException("Given role name not found");
            }

            TUserRole userRole = new TUserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = role.Id;

            _userRoleRepo.Add(userRole);

            await Task.FromResult(0);
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

            _userRepo.Add(user);
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

            _userRepo.Remove(user);
            await SaveChangesAsync().WithCurrentCulture();
        }

        private async Task IncludeLoginsAsync(TUser user)
        {
            user.Logins = _userLoginRepo.FindAllByUserId(user.Id);
            await Task.FromResult(0);
        }

        private async Task IncludeRolesAsync(TUser user)
        {
            user.Roles = _userRoleRepo.FindAllByUserId(user.Id);
            await Task.FromResult(0);
        }

        private async Task IncludeClaimsAsync(TUser user)
        {
            user.Claims = _userClaimRepo.FindAllByUserId(user.Id);
            await Task.FromResult(0);
        }

        private async Task<TUser> GetUserAggregateByIdAsync(TKey userId)
        {
            TUser user = _userRepo.FindById(userId);

            if (user != null)
            {
                await IncludeLoginsAsync(user).WithCurrentCulture();
                await IncludeRolesAsync(user).WithCurrentCulture();
                await IncludeClaimsAsync(user).WithCurrentCulture();
            }

            return user;
        }

        private async Task<TUser> GetUserAggregateByUserNameAsync(string userName)
        {
            TUser user = _userRepo.FindByUserName(userName);

            if (user != null)
            {
                await IncludeLoginsAsync(user).WithCurrentCulture();
                await IncludeRolesAsync(user).WithCurrentCulture();
                await IncludeClaimsAsync(user).WithCurrentCulture();
            }

            return user;
        }

        private async Task<TUser> GetUserAggregateByEmailAsync(string email)
        {
            TUser user = _userRepo.FindByEmail(email);

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
            TUserLogin userLogin = _userLoginRepo.Find(login);

            if (userLogin == null)
            {
                user = default(TUser);
            }
            else
            {
                user = await GetUserAggregateByIdAsync(userLogin.UserId).WithCurrentCulture();
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

            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("'email' parameter cannot be null or empty");
            }

            TUser user = await GetUserAggregateByEmailAsync(email).WithCurrentCulture();

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

            TUser user = await GetUserAggregateByIdAsync(userId).WithCurrentCulture();

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

            if (String.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("'userName' parameter cannot be null or empty");
            }

            TUser user = await GetUserAggregateByUserNameAsync(userName).WithCurrentCulture();

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

            IList<string> list = _roleRepo.FindRoleNamesByUserId(user.Id);

            return await Task.FromResult(list);
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

            if (user == null)
            {
                throw new ArgumentNullException("'user' parameter null");
            }

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

            bool inRole = _userRoleRepo.IsInRole(user.Id, roleName);

            return await Task.FromResult(inRole);
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

            IEnumerable<TUserClaim> list = _userClaimRepo.FindAllByUserId(user.Id, claim);

            if (list.Any())
            {
                foreach (TUserClaim c in list)
                {
                    _userClaimRepo.Remove(c);
                }
            }

            await Task.FromResult(0);
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

            TRole role = _roleRepo.FindByName(roleName);

            if (role == null)
            {
                return;
            }

            TUserRole userRole = new TUserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = role.Id;

            _userRoleRepo.Remove(userRole);

            await Task.FromResult(0);
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

            TUserLogin userLogin = _userLoginRepo.Find(user.Id, login);

            if (userLogin != null)
            {
                _userLoginRepo.Remove(userLogin);
            }

            await Task.FromResult(0);
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

            _userRepo.Change(user);
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
        where TUser : IdentityUser<TKey>, new()
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class with unit of work.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference.</param>
        public UserStore(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

}
