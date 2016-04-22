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
using Microsoft.AspNet.Identity;
using Mark.Data;
using Mark.Data.Common;
using System.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents user login repository.
    /// </summary>
    /// <typeparam name="TUserLogin">User login entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    internal class UserLoginRepository<TUserLogin, TKey>
        : DbRepository<TUserLogin>
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public UserLoginRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save adding of the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TUserLogin item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"INSERT INTO {0} ({1}, {2}, {3}) VALUES (@{4}, @{5}, @{6});",
                StorageContext[Entities.UserLogin].TableName,
                // Configured field names
                StorageContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                StorageContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                StorageContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey,
                UserLoginFields.UserId);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TUserLogin>((parameters, entity) =>
            {
                parameters[UserLoginFields.LoginProvider].Value = entity.LoginProvider;
                parameters[UserLoginFields.ProviderKey].Value =  entity.ProviderKey;
                parameters[UserLoginFields.UserId].Value = entity.UserId;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// (Not implemented) Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUserLogin item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUserLogin item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"DELETE FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                StorageContext[Entities.UserLogin].TableName,
                // Configured field names
                StorageContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                StorageContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                StorageContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey,
                UserLoginFields.UserId);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TUserLogin>((parameters, entity) =>
            {
                parameters[UserLoginFields.LoginProvider].Value = entity.LoginProvider;
                parameters[UserLoginFields.ProviderKey].Value = entity.ProviderKey;
                parameters[UserLoginFields.UserId].Value = entity.UserId;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find user login by login information.
        /// </summary>
        /// <param name="loginInfo">User login information.</param>
        /// <returns>Returns the user login if found; otherwise, returns null.</returns>
        public TUserLogin Find(UserLoginInfo loginInfo)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{3} AND {2} = @{4};",
                StorageContext[Entities.UserLogin].TableName,
                // Configured field names
                StorageContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                StorageContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserLoginFields.LoginProvider].Value = loginInfo.LoginProvider;
            cmdContext.Parameters[UserLoginFields.ProviderKey].Value = loginInfo.ProviderKey;

            DbDataReader reader = null;
            TUserLogin UserLogin = default(TUserLogin);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    UserLogin = new TUserLogin();

                    UserLogin.LoginProvider = reader.GetSafeString(
                        StorageContext[Entities.UserLogin][UserLoginFields.LoginProvider]);
                    UserLogin.ProviderKey = reader.GetSafeString(
                        StorageContext[Entities.UserLogin][UserLoginFields.ProviderKey]);
                    UserLogin.UserId = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.UserLogin][UserLoginFields.UserId]);
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

                cmdContext.Dispose();
                StorageContext.Close();
            }

            return UserLogin;
        }

        /// <summary>
        /// Find user login by login information and user id.
        /// </summary>
        /// <param name="userId">Target user.</param>
        /// <param name="loginInfo">User login information.</param>
        /// <returns>Returns the user login if found; otherwise, returns null.</returns>
        public TUserLogin Find(TKey userId, UserLoginInfo loginInfo)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                StorageContext[Entities.UserLogin].TableName,
                // Configured field names
                StorageContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                StorageContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                StorageContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey,
                UserLoginFields.UserId);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserLoginFields.LoginProvider].Value = loginInfo.LoginProvider;
            cmdContext.Parameters[UserLoginFields.ProviderKey].Value = loginInfo.ProviderKey;
            cmdContext.Parameters[UserLoginFields.UserId].Value = userId;

            DbDataReader reader = null;
            TUserLogin UserLogin = default(TUserLogin);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    UserLogin = new TUserLogin();

                    UserLogin.LoginProvider = reader.GetSafeString(
                        StorageContext[Entities.UserLogin][UserLoginFields.LoginProvider]);
                    UserLogin.ProviderKey = reader.GetSafeString(
                        StorageContext[Entities.UserLogin][UserLoginFields.ProviderKey]);
                    UserLogin.UserId = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.UserLogin][UserLoginFields.UserId]);
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

                cmdContext.Dispose();
                StorageContext.Close();
            }

            return UserLogin;
        }

        /// <summary>
        /// Find all user UserLogins by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user UserLogins if found; otherwise, returns empty list.</returns>
        public ICollection<TUserLogin> FindAllByUserId(TKey userId)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                StorageContext[Entities.UserLogin].TableName,
                // Configured field names
                StorageContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.UserId);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserLoginFields.UserId].Value = userId;

            DbDataReader reader = null;
            List<TUserLogin> list = new List<TUserLogin>();
            TUserLogin UserLogin = default(TUserLogin);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    UserLogin = new TUserLogin();

                    UserLogin.LoginProvider = reader.GetSafeString(
                        StorageContext[Entities.UserLogin][UserLoginFields.LoginProvider]);
                    UserLogin.ProviderKey = reader.GetSafeString(
                        StorageContext[Entities.UserLogin][UserLoginFields.ProviderKey]);
                    UserLogin.UserId = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.UserLogin][UserLoginFields.UserId]);

                    list.Add(UserLogin);
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

                cmdContext.Dispose();
                StorageContext.Close();
            }

            return list;
        }
    }
}
