﻿//
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
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user role entity configuration.
    /// </summary>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class UserRoleConfiguration<TUserRole, TKey> : EntityConfiguration<TUserRole>
        where TUserRole : IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            ToTable(Entities.UserRole);
            HasKey(p => new
            {
                p.UserId,
                p.RoleId
            });
            Property(p => p.UserId).HasColumnName(UserRoleFields.UserId);
            Property(p => p.RoleId).HasColumnName(UserRoleFields.RoleId);
        }
    }
}
