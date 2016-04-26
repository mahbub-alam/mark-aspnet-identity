//
// Copyright 2016, Mahbub Alam (mahbub002@ymail.com)
// 
// Licensed under the Apache License, Version 2.0 (the "License"));
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
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user entity configuration.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User logins type.</typeparam>
    /// <typeparam name="TUserRole">User roles type.</typeparam>
    /// <typeparam name="TUserClaim">User claims type.</typeparam>
    public class UserConfiguration<TUser, TKey, TUserLogin, TUserRole, TUserClaim> : EntityConfiguration<TUser>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            ToTable(Entities.User);
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName(UserFields.Id);
            Property(p => p.UserName).HasColumnName(UserFields.UserName);
            Property(p => p.PasswordHash).HasColumnName(UserFields.PasswordHash);
            Property(p => p.SecurityStamp).HasColumnName(UserFields.SecurityStamp);
            Property(p => p.Email).HasColumnName(UserFields.Email);
            Property(p => p.EmailConfirmed).HasColumnName(UserFields.EmailConfirmed);
            Property(p => p.PhoneNumber).HasColumnName(UserFields.PhoneNumber);
            Property(p => p.PhoneNumberConfirmed).HasColumnName(UserFields.PhoneNumberConfirmed);
            Property(p => p.TwoFactorEnabled).HasColumnName(UserFields.TwoFactorEnabled);
            Property(p => p.LockoutEnabled).HasColumnName(UserFields.LockoutEnabled);
            Property(p => p.LockoutEndDateUtc).HasColumnName(UserFields.LockoutEndDateUtc);
            Property(p => p.AccessFailedCount).HasColumnName(UserFields.AccessFailedCount);
        }
    }

    /// <summary>
    /// Represents user entity configuration.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class UserConfiguration<TUser, TKey>
        : UserConfiguration<
            TUser,
            TKey,
            IdentityUserLogin<TKey>, 
            IdentityUserRole<TKey>, 
            IdentityUserClaim<TKey>>
        where TUser : IdentityUser<
            TKey, 
            IdentityUserLogin<TKey>, 
            IdentityUserRole<TKey>, 
            IdentityUserClaim<TKey>>
        where TKey : struct, IEquatable<TKey>
    {
    }
}
