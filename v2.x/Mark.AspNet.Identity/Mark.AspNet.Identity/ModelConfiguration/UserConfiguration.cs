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

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user entity configuration.
    /// </summary>
    public class UserConfiguration : EntityConfiguration
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            TableName = Entities.User;
            this[UserFields.Id] = UserFields.Id;
            this[UserFields.UserName] = UserFields.UserName;
            this[UserFields.PasswordHash] = UserFields.PasswordHash;
            this[UserFields.SecurityStamp] = UserFields.SecurityStamp;
            this[UserFields.Email] = UserFields.Email;
            this[UserFields.EmailConfirmed] = UserFields.EmailConfirmed;
            this[UserFields.PhoneNumber] = UserFields.PhoneNumber;
            this[UserFields.PhoneNumberConfirmed] = UserFields.PhoneNumberConfirmed;
            this[UserFields.TwoFactorEnabled] = UserFields.TwoFactorEnabled;
            this[UserFields.LockoutEnabled] = UserFields.LockoutEnabled;
            this[UserFields.LockoutEndDateUtc] = UserFields.LockoutEndDateUtc;
            this[UserFields.AccessFailedCount] = UserFields.AccessFailedCount;
        }
    }
}
