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
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents mapping configuration for 'UserRole' table.
    /// </summary>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserRoleMap<TUserRole, TKey> : EntityMap<TUserRole> 
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserRoleMap(EntityConfiguration<TUserRole> configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => new
            {
               p.UserId,
               p.RoleId
            });

            Property(p => p.UserId)
                .HasColumnName(Configuration.Property(p => p.UserId).ColumnName)
                .IsRequired();

            Property(p => p.RoleId)
                .HasColumnName(Configuration.Property(p => p.RoleId).ColumnName)
                .IsRequired();
        }
    }

    /// <summary>
    /// Represents mapping configuration for 'UserRole' table.
    /// </summary>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class IdentityUserRoleMap<TUserRole> : EntityMap<TUserRole>
        where TUserRole : IdentityUserRole 
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserRoleMap(EntityConfiguration<TUserRole> configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => new
            {
                p.UserId,
                p.RoleId
            });

            Property(p => p.UserId)
                .HasColumnName(Configuration.Property(p => p.UserId).ColumnName)
                .IsRequired();

            Property(p => p.RoleId)
                .HasColumnName(Configuration.Property(p => p.RoleId).ColumnName)
                .IsRequired();
        }
    }

}
