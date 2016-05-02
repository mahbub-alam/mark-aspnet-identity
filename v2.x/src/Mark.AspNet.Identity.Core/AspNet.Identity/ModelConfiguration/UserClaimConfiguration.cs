﻿//
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

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user claim entity configuration.
    /// </summary>
    /// <typeparam name="TUserClaim">User claim entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class UserClaimConfiguration<TUserClaim, TKey> : EntityConfiguration<TUserClaim>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            ToTable(Entities.UserClaim);
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName(UserClaimFields.Id);
            Property(p => p.ClaimType).HasColumnName(UserClaimFields.ClaimType);
            Property(p => p.ClaimValue).HasColumnName(UserClaimFields.ClaimValue);
            Property(p => p.UserId).HasColumnName(UserClaimFields.UserId);
        }
    }
}
