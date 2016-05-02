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
using System.Data.Common;
using Mark.DotNet.Data;
using Mark.DotNet.Data.Common;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.SqlServer
{
    /// <summary>
    /// Represents user login repository.
    /// </summary>
    /// <typeparam name="TUserLogin">User login entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class UserLoginRepository<TUserLogin, TKey>
        : SqlRepository<TUserLogin>
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TKey : IEquatable<TKey>
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
            DbCommandContext cmdContext = CommandBuilder.GetInsertCommand(
                new List<TUserLogin> { item });

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
            DbCommandContext cmdContext = CommandBuilder.GetDeleteCommand(
                new List<TUserLogin> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find user login by login information.
        /// </summary>
        /// <param name="loginInfo">User login information.</param>
        /// <returns>Returns the user login if found; otherwise, returns null.</returns>
        public TUserLogin Find(UserLoginInfo loginInfo)
        {
            PropertyConfiguration loginProviderPropCfg = Configuration.Property(p => p.LoginProvider);
            PropertyConfiguration providerKeyPropCfg = Configuration.Property(p => p.ProviderKey);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{3} AND {2} = @{4};",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(loginProviderPropCfg.ColumnName),
                QueryBuilder.GetQuotedIdentifier(providerKeyPropCfg.ColumnName),
                // Parameter names
                loginProviderPropCfg.PropertyName,
                providerKeyPropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[loginProviderPropCfg.PropertyName].Value = loginInfo.LoginProvider;
            cmdContext.Parameters[providerKeyPropCfg.PropertyName].Value = loginInfo.ProviderKey;

            DbDataReader reader = null;
            TUserLogin userLogin = default(TUserLogin);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                userLogin = EntityBuilder.Build(reader);
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

            return userLogin;
        }

        /// <summary>
        /// Find user login by login information and user id.
        /// </summary>
        /// <param name="userId">Target user.</param>
        /// <param name="loginInfo">User login information.</param>
        /// <returns>Returns the user login if found; otherwise, returns null.</returns>
        public TUserLogin Find(TKey userId, UserLoginInfo loginInfo)
        {
            PropertyConfiguration loginProviderPropCfg = Configuration.Property(p => p.LoginProvider);
            PropertyConfiguration providerKeyPropCfg = Configuration.Property(p => p.ProviderKey);
            PropertyConfiguration userIdPropCfg = Configuration.Property(p => p.UserId);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(loginProviderPropCfg.ColumnName),
                QueryBuilder.GetQuotedIdentifier(providerKeyPropCfg.ColumnName),
                QueryBuilder.GetQuotedIdentifier(userIdPropCfg.ColumnName),
                // Parameter names
                loginProviderPropCfg.PropertyName,
                providerKeyPropCfg.PropertyName,
                userIdPropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[loginProviderPropCfg.PropertyName].Value = loginInfo.LoginProvider;
            cmdContext.Parameters[providerKeyPropCfg.PropertyName].Value = loginInfo.ProviderKey;
            cmdContext.Parameters[userIdPropCfg.PropertyName].Value = userId;

            DbDataReader reader = null;
            TUserLogin userLogin = default(TUserLogin);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                userLogin = EntityBuilder.Build(reader);
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

            return userLogin;
        }

        /// <summary>
        /// Find all user UserLogins by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user UserLogins if found; otherwise, returns empty list.</returns>
        public ICollection<TUserLogin> FindAllByUserId(TKey userId)
        {
            PropertyConfiguration userIdPropCfg = Configuration.Property(p => p.UserId);
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                userIdPropCfg.ColumnName,
                // Parameter names
                userIdPropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[userIdPropCfg.PropertyName].Value = userId;

            DbDataReader reader = null;
            ICollection<TUserLogin> list = null;

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                list = EntityBuilder.BuildAll(reader);
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
