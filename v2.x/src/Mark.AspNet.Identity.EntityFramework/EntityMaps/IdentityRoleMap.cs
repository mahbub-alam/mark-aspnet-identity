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
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents entity mapping for role entity.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class IdentityRoleMap<TRole, TKey, TUserRole>
        : EntityMap<TRole>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityRoleMap(EntityConfiguration<TRole> configuration) : base(configuration)
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
            Property(p => p.Name)
                .HasColumnName(Configuration.Property(p => p.Name).ColumnName)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UK_Role_Name") { IsUnique = true }));
        }

        /// <summary>
        /// Map relationship among entities.
        /// </summary>
        protected override void MapRelationships()
        {
            HasMany(p => p.Users)
                .WithRequired()
                .HasForeignKey(p => p.RoleId);
        }
    }

    /// <summary>
    /// Represents entity mapping for role entity.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class IdentityRoleMap<TRole, TUserRole>
        : EntityMap<TRole>
        where TRole : IdentityRole<string, TUserRole>
        where TUserRole : IdentityUserRole
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityRoleMap(EntityConfiguration<TRole> configuration) : base(configuration)
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
            Property(p => p.Name)
                .HasColumnName(Configuration.Property(p => p.Name).ColumnName)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UK_Role_Name") { IsUnique = true }));
        }

        /// <summary>
        /// Map relationship among entities.
        /// </summary>
        protected override void MapRelationships()
        {
            HasMany(p => p.Users)
                .WithRequired()
                .HasForeignKey(p => p.RoleId);
        }
    }

}
