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
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents entity mapping for user entity.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    /// <typeparam name="TUserClaim">User claim type.</typeparam>
    public class IdentityUserMap<TUser, TKey, TUserLogin, TUserRole, TUserClaim>
        : EntityMap<TUser>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserMap(EntityConfiguration<TUser> configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => p.Id);
            Property(p => p.Id)
                .HasColumnName(Configuration.Property(p => p.Id).ColumnName);
        }

        /// <summary>
        /// Map all non-key fields.
        /// </summary>
        protected override void MapFields()
        {
            Property(p => p.UserName)
                .HasColumnName(Configuration.Property(p => p.UserName).ColumnName)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UK_User_UserName") { IsUnique = true }));

            Property(p => p.SecurityStamp)
                .HasMaxLength(255)
                .HasColumnName(Configuration.Property(p => p.SecurityStamp).ColumnName);

            Property(p => p.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName(Configuration.Property(p => p.PasswordHash).ColumnName);

            Property(p => p.Email)
                .HasMaxLength(64)
                .HasColumnName(Configuration.Property(p => p.Email).ColumnName);

            Property(p => p.EmailConfirmed)
                .HasColumnName(Configuration.Property(p => p.EmailConfirmed).ColumnName);

            Property(p => p.PhoneNumber)
                .HasMaxLength(16)
                .HasColumnName(Configuration.Property(p => p.PhoneNumber).ColumnName);

            Property(p => p.PhoneNumberConfirmed)
                .HasColumnName(Configuration.Property(p => p.PhoneNumberConfirmed).ColumnName);

            Property(p => p.TwoFactorEnabled)
                .HasColumnName(Configuration.Property(p => p.TwoFactorEnabled).ColumnName);

            Property(p => p.LockoutEnabled)
                .HasColumnName(Configuration.Property(p => p.LockoutEnabled).ColumnName);

            Property(p => p.LockoutEndDateUtc)
                .HasColumnName(Configuration.Property(p => p.LockoutEndDateUtc).ColumnName);

            Property(p => p.AccessFailedCount)
                .HasColumnName(Configuration.Property(p => p.AccessFailedCount).ColumnName);
        }

        /// <summary>
        /// Map relationship among entities.
        /// </summary>
        protected override void MapRelationships()
        {
            HasMany(p => p.Roles)
                .WithRequired()
                .HasForeignKey(p => p.UserId);

            HasMany(p => p.Claims)
                .WithRequired()
                .HasForeignKey(p => p.UserId);

            HasMany(p => p.Logins)
                .WithRequired()
                .HasForeignKey(p => p.UserId);
        }
    }

    /// <summary>
    /// Represents entity mapping for user entity.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserMap<TUser, TKey>
        : IdentityUserMap<TUser, TKey, 
            IdentityUserLogin<TKey>, 
            IdentityUserRole<TKey>, 
            IdentityUserClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserMap(EntityConfiguration<TUser> configuration) : base(configuration)
        {
        }
    }
}
