// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents base storage class for 'User' management.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    /// <typeparam name="TUserClaim">User claim type.</typeparam>
    public abstract class UserStoreBase<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>
        : DisposableStore, IUserStore<TUser, TKey>, IUserLoginStore<TUser, TKey>, IUserRoleStore<TUser, TKey>, 
        IUserClaimStore<TUser, TKey>, IUserPasswordStore<TUser, TKey>, IUserSecurityStampStore<TUser, TKey>, 
        IUserEmailStore<TUser, TKey>, IUserPhoneNumberStore<TUser, TKey>, IUserTwoFactorStore<TUser, TKey>, 
        IUserLockoutStore<TUser, TKey>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole> 
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TKey : struct

    {
        /// <summary>
        /// Add new user claim.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="claim">Claim to be added.</param>
        /// <returns></returns>
        public abstract Task AddClaimAsync(TUser user, Claim claim);

        /// <summary>
        /// Add new user login from remote login provider.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="login">Login to be added.</param>
        /// <returns></returns>
        public abstract Task AddLoginAsync(TUser user, UserLoginInfo login);

        /// <summary>
        /// Add user to new role.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="roleName">Target role.</param>
        /// <returns></returns>
        public abstract Task AddToRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="user">User to be created.</param>
        /// <returns></returns>
        public abstract Task CreateAsync(TUser user);

        /// <summary>
        /// Delete specified user.
        /// </summary>
        /// <param name="user">User to be deleted.</param>
        /// <returns></returns>
        public abstract Task DeleteAsync(TUser user);

        /// <summary>
        /// Find user by remote login information.
        /// </summary>
        /// <param name="login">Remote login information.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public abstract Task<TUser> FindAsync(UserLoginInfo login);

        /// <summary>
        /// Find user by email.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public abstract Task<TUser> FindByEmailAsync(string email);

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public abstract Task<TUser> FindByIdAsync(TKey userId);

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="userName">User's username.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public abstract Task<TUser> FindByNameAsync(string userName);

        /// <summary>
        /// Get access failed count value.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns current value.</returns>
        public abstract Task<int> GetAccessFailedCountAsync(TUser user);

        /// <summary>
        /// Get a list of claims belong to the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns a list of claims if found; otherwise, returns an empty list.</returns>
        public abstract Task<IList<Claim>> GetClaimsAsync(TUser user);

        /// <summary>
        /// Get user's email.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns email.</returns>
        public abstract Task<string> GetEmailAsync(TUser user);

        /// <summary>
        /// Get user's email confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns confirmation.</returns>
        public abstract Task<bool> GetEmailConfirmedAsync(TUser user);

        /// <summary>
        /// Get whether the account lockout option is enabled for the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns true if enabled; false if disabled.</returns>
        public abstract Task<bool> GetLockoutEnabledAsync(TUser user);

        /// <summary>
        /// Get when the lockout will end.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns date of the lockout ending.</returns>
        public abstract Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user);

        /// <summary>
        /// Get a list of logins made by the user using remote login provider.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns a list of logins if found; otherwise, returns an empty list.</returns>
        public abstract Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);

        /// <summary>
        /// Get user's password hash.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns password hash.</returns>
        public abstract Task<string> GetPasswordHashAsync(TUser user);

        /// <summary>
        /// Get user's phone number.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns phone number.</returns>
        public abstract Task<string> GetPhoneNumberAsync(TUser user);

        /// <summary>
        /// Get user's phone number confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Retrurns true if confirmed; otherwise, returns false.</returns>
        public abstract Task<bool> GetPhoneNumberConfirmedAsync(TUser user);
        
        /// <summary>
        /// Get a list of roles the user belongs to.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns a list of roles if found; otherwise, returns an empty list.</returns>
        public abstract Task<IList<string>> GetRolesAsync(TUser user);

        /// <summary>
        /// Get user's security stamp.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns security stamp.</returns>
        public abstract Task<string> GetSecurityStampAsync(TUser user);

        /// <summary>
        /// Get whether the two factor authentication is enabled.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns true if enabled; otherwise, returns false.</returns>
        public abstract Task<bool> GetTwoFactorEnabledAsync(TUser user);

        /// <summary>
        /// Check whether the user has password.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns true if user has password; otherwise, returns false.</returns>
        public abstract Task<bool> HasPasswordAsync(TUser user);

        /// <summary>
        /// Record the event when an attempt to access the user account has failed.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns>Returns new failed count.</returns>
        public abstract Task<int> IncrementAccessFailedCountAsync(TUser user);

        /// <summary>
        /// Check whether the user belongs to the given role.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="roleName">Role name.</param>
        /// <returns>Returns true if belongs; otherwise, returns false.</returns>
        public abstract Task<bool> IsInRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Remove user claim.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="claim">Removing claim.</param>
        /// <returns></returns>
        public abstract Task RemoveClaimAsync(TUser user, Claim claim);

        /// <summary>
        /// Remove the user from the given role.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="roleName">Role name.</param>
        /// <returns></returns>
        public abstract Task RemoveFromRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Remove user's remote login that matchs the given remote login provider.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="login">Remote login information.</param>
        /// <returns></returns>
        public abstract Task RemoveLoginAsync(TUser user, UserLoginInfo login);

        /// <summary>
        /// Reset the record that tracks the user account access failed attempts. It is 
        /// usually done after the account is successfully accessed.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns></returns>
        public abstract Task ResetAccessFailedCountAsync(TUser user);

        /// <summary>
        /// Set user's email.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="email">User's new email.</param>
        /// <returns></returns>
        public abstract Task SetEmailAsync(TUser user, string email);

        /// <summary>
        /// Set user's email confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="confirmed">Whether confirmed.</param>
        /// <returns></returns>
        public abstract Task SetEmailConfirmedAsync(TUser user, bool confirmed);

        /// <summary>
        /// Set whether given user's account lockout option is enabled.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="enabled">If enabled.</param>
        /// <returns></returns>
        public abstract Task SetLockoutEnabledAsync(TUser user, bool enabled);

        /// <summary>
        /// Set when the lockout will end for the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="lockoutEnd">Lockout end date.</param>
        /// <returns></returns>
        public abstract Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd);

        /// <summary>
        /// Set user's password hash.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="passwordHash">Password hash.</param>
        /// <returns></returns>
        public abstract Task SetPasswordHashAsync(TUser user, string passwordHash);

        /// <summary>
        /// Set user's phone number.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="phoneNumber">New phone number.</param>
        /// <returns></returns>
        public abstract Task SetPhoneNumberAsync(TUser user, string phoneNumber);

        /// <summary>
        /// Set user's phone number confirmation.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="confirmed">Whether confirmed.</param>
        /// <returns></returns>
        public abstract Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed);

        /// <summary>
        /// Set user's security stamp.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="stamp">Security stamp.</param>
        /// <returns></returns>
        public abstract Task SetSecurityStampAsync(TUser user, string stamp);

        /// <summary>
        /// Set whether to enable two factor authentication for the user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="enabled">Whether enabled.</param>
        /// <returns></returns>
        public abstract Task SetTwoFactorEnabledAsync(TUser user, bool enabled);

        /// <summary>
        /// Update the given user.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <returns></returns>
        public abstract Task UpdateAsync(TUser user);
    }

    /// <summary>
    /// Represents base storage class for 'User' management.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public abstract class UserStoreBase<TUser, TKey>
        : UserStoreBase<TUser, 
            IdentityRole<TKey>, 
            TKey, 
            IdentityUserLogin<TKey>, 
            IdentityUserRole<TKey>, 
            IdentityUserClaim<TKey>> 
        where TUser : IdentityUser<TKey>
        where TKey : struct 
    {
    }
}
