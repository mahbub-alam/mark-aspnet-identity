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
using Mark.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents entity mapping user claim entity.
    /// </summary>
    /// <typeparam name="TUserClaim">User claim type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserClaimMap<TUserClaim, TKey> : EntityMap<TUserClaim>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public IdentityUserClaimMap(EntityConfiguration<TUserClaim> configuration) : base(configuration)
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
            Property(p => p.UserId)
                .HasColumnName(Configuration.Property(p => p.UserId).ColumnName);

            Property(p => p.ClaimType)
                .HasMaxLength(255)
                .HasColumnName(Configuration.Property(p => p.ClaimType).ColumnName);

            Property(p => p.ClaimValue)
                .HasMaxLength(255)
                .HasColumnName(Configuration.Property(p => p.ClaimValue).ColumnName);
        }
    }
}
