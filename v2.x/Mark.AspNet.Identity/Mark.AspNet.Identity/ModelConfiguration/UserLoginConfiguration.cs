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

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user login entity configuration.
    /// </summary>
    /// <typeparam name="TUserLogin">User login type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class UserLoginConfiguration<TUserLogin, TKey> : EntityConfiguration<TUserLogin>
        where TUserLogin : IdentityUserLogin<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            ToTable(Entities.UserLogin);
            HasKey(p => new
            {
                p.LoginProvider,
                p.ProviderKey,
                p.UserId
            });
            Property(p => p.LoginProvider).HasColumnName(UserLoginFields.LoginProvider);
            Property(p => p.ProviderKey).HasColumnName(UserLoginFields.ProviderKey);
            Property(p => p.UserId).HasColumnName(UserLoginFields.UserId);
        }
    }
}
