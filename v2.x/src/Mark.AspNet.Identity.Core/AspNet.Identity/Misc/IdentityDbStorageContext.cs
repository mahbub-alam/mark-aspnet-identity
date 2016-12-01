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
using System.Data.Common;
using Mark.DotNet.Data.ModelConfiguration;
using Mark.DotNet.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents default ADO.NET style storage context.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TRole">Role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login entity type.</typeparam>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TUserClaim">User claim entity type.</typeparam>
    public class IdentityDbStorageContext< 
        TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> : DbStorageContext
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class which uses the "DefaultConnection" connection string name.
        /// </summary>
        public IdentityDbStorageContext()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given Connection string name.
        /// </summary>
        /// <param name="connectionStringName">Connection string name.</param>
        public IdentityDbStorageContext(string connectionStringName) : base(connectionStringName)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given connection string and provider name.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="providerName">Provider name.</param>
        public IdentityDbStorageContext(string connectionString, string providerName) 
            : base(connectionString, providerName)
        {
        }

        /// <summary>
        /// Configure entities.
        /// </summary>
        /// <param name="entityConfigs">Passed entity configuration collection.</param>
        protected override void OnConfiguringEntities(EntityConfigurationCollection entityConfigs)
        {
            entityConfigs.Add(new RoleConfiguration<TRole, TKey, TUserRole>());
            entityConfigs.Add(new UserConfiguration<TUser, TKey, TUserLogin, TUserRole, TUserClaim>());
            entityConfigs.Add(new UserLoginConfiguration<TUserLogin, TKey>());
            entityConfigs.Add(new UserRoleConfiguration<TUserRole, TKey>());
            entityConfigs.Add(new UserClaimConfiguration<TUserClaim, TKey>());
        }
    }

    /// <summary>
    /// Represents default ADO.NET style storage context.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityDbStorageContext<TUser, TKey>
        : IdentityDbStorageContext<
            TUser,
            IdentityRole<TKey>,
            TKey,
            IdentityUserLogin<TKey>,
            IdentityUserRole<TKey>,
            IdentityUserClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class which uses the "DefaultConnection" connection string name.
        /// </summary>
        public IdentityDbStorageContext()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given Connection string name.
        /// </summary>
        /// <param name="connectionStringName">Connection string name.</param>
        public IdentityDbStorageContext(string connectionStringName) : base(connectionStringName)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given connection string and provider name.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="providerName">Provider name.</param>
        public IdentityDbStorageContext(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }

    /// <summary>
    /// Represents default ADO.NET style storage context.
    /// </summary>
    /// <typeparam name="TRole">Role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    public class IdentityDbStorageContext<TRole, TKey, TUserRole>
        : IdentityDbStorageContext<
            IdentityUser<TKey, IdentityUserLogin<TKey>, TUserRole, IdentityUserClaim<TKey>>,
            TRole,
            TKey,
            IdentityUserLogin<TKey>,
            TUserRole,
            IdentityUserClaim<TKey>>
        where TRole : IdentityRole<TKey, TUserRole> 
        where TUserRole : IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class which uses the "DefaultConnection" connection string name.
        /// </summary>
        public IdentityDbStorageContext()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given Connection string name.
        /// </summary>
        /// <param name="connectionStringName">Connection string name.</param>
        public IdentityDbStorageContext(string connectionStringName) : base(connectionStringName)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given connection string and provider name.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="providerName">Provider name.</param>
        public IdentityDbStorageContext(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }

    /// <summary>
    /// Represents default ADO.NET style storage context.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityDbStorageContext<TKey>
        : IdentityDbStorageContext<
            IdentityUser<TKey>,
            IdentityRole<TKey>,
            TKey,
            IdentityUserLogin<TKey>,
            IdentityUserRole<TKey>,
            IdentityUserClaim<TKey>>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class which uses the "DefaultConnection" connection string name.
        /// </summary>
        public IdentityDbStorageContext()
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given Connection string name.
        /// </summary>
        /// <param name="connectionStringName">Connection string name.</param>
        public IdentityDbStorageContext(string connectionStringName) : base(connectionStringName)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given connection string and provider name.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="providerName">Provider name.</param>
        public IdentityDbStorageContext(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }
}
