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
using Mark.Data;
using Mark.Data.Common;
using System.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents user role repository.
    /// </summary>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    internal class UserRoleRepository<TUserRole, TKey>
        : DbRepository<TUserRole> 
        where TUserRole : IdentityUserRole<TKey>, new()
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save adding of the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TUserRole item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"INSERT INTO {0} ({1}, {2}) VALUES (@{3}, @{4});",
                StorageContext[Entities.UserRole].TableName,
                // Configured field names
                StorageContext[Entities.UserRole][UserRoleFields.UserId],
                StorageContext[Entities.UserRole][UserRoleFields.RoleId],
                // Parameter names
                UserRoleFields.UserId,
                UserRoleFields.RoleId);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TUserRole>((parameters, entity) =>
            {
                parameters[UserRoleFields.UserId].Value = entity.UserId;
                parameters[UserRoleFields.RoleId].Value = entity.RoleId;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// (Not implemented) Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUserRole item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUserRole item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"DELETE FROM {0} WHERE {1} = @{3} AND {2} = @{4};",
                StorageContext[Entities.UserRole].TableName,
                // Configured field names
                StorageContext[Entities.UserRole][UserRoleFields.UserId],
                StorageContext[Entities.UserRole][UserRoleFields.RoleId], 
                // Parameter names
                UserRoleFields.UserId, 
                UserRoleFields.RoleId);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TUserRole>((parameters, entity) =>
            {
                parameters[UserRoleFields.UserId].Value = entity.UserId;
                parameters[UserRoleFields.RoleId].Value = entity.RoleId;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find all user roles by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user roles if found; otherwise, returns empty list.</returns>
        public ICollection<TUserRole> FindAllByUserId(TKey userId)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                StorageContext[Entities.UserRole].TableName,
                // Configured field names
                StorageContext[Entities.UserRole][UserRoleFields.UserId],
                // Parameter names
                UserRoleFields.UserId);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserRoleFields.UserId].Value = userId;

            DbDataReader reader = null;
            List<TUserRole> list = new List<TUserRole>();
            TUserRole userRole = default(TUserRole);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    userRole = new TUserRole();
                    userRole.UserId = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.UserRole][UserRoleFields.UserId]);
                    userRole.RoleId = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.UserRole][UserRoleFields.RoleId]);

                    list.Add(userRole);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                StorageContext.Close();
            }

            return list;
        }

        /// <summary>
        /// Check whether the user belongs to the given role.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <param name="roleName">Target role name.</param>
        /// <returns>Returns true if belongs; otherwise, returns false.</returns>
        public bool IsInRole(TKey userId, string roleName)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT {2} FROM {0} INNER JOIN {1} ON ({0}.{2} = {1}.{3}) 
                    WHERE {4} = @{6} AND LOWER({5}) = LOWER(@{7});",
                StorageContext[Entities.UserRole].TableName,
                StorageContext[Entities.Role].TableName,
                // Configured field names
                StorageContext[Entities.UserRole][UserRoleFields.RoleId],
                StorageContext[Entities.Role][RoleFields.Id],
                StorageContext[Entities.UserRole][UserRoleFields.UserId],
                StorageContext[Entities.Role][RoleFields.Name],
                // Parameter names
                UserRoleFields.UserId, 
                RoleFields.Name);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserRoleFields.UserId].Value = userId;
            cmdContext.Parameters[RoleFields.Name].Value = roleName;

            DbDataReader reader = null;
            bool inRole = false;

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                inRole = reader.Read();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                StorageContext.Close();
            }

            return inRole;
        }

    }
}
