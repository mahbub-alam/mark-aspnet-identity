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
using Mark.DotNet.Data;
using Mark.DotNet.Data.Common;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.SqlServer
{
    /// <summary>
    /// Represents User repository.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login entity type.</typeparam>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TUserClaim">User claim entity type.</typeparam>
    public class UserRepository<TUser, TKey, TUserLogin, TUserRole, TUserClaim>
        : SqlRepository<TUser>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>, new()
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save adding of the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TUser item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetInsertCommand(
                new List<TUser> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUser item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetUpdateCommand(
                new List<TUser> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUser item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetDeleteCommand(
                new List<TUser> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="id">Target user id.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindById(TKey id)
        {
            PropertyConfiguration idPropCfg = Configuration.Property(p => p.Id);
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(idPropCfg.ColumnName),
                // Parameter names
                idPropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[idPropCfg.PropertyName].Value = id;

            DbDataReader reader = null;
            TUser user = default(TUser);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                user = EntityBuilder.Build(reader);
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

            return user;
        }

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="userName">Target user username.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindByUserName(string userName)
        {
            PropertyConfiguration userNamePropCfg = Configuration.Property(p => p.UserName);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(userNamePropCfg.ColumnName),
                // Parameter names
                userNamePropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[userNamePropCfg.PropertyName].Value = userName;

            DbDataReader reader = null;
            TUser user = default(TUser);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                user = EntityBuilder.Build(reader);
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

            return user;
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="email">Target user email.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindByEmail(string email)
        {
            PropertyConfiguration emailPropCfg = Configuration.Property(p => p.Email);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(emailPropCfg.ColumnName),
                // Parameter names
                emailPropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[emailPropCfg.PropertyName].Value = email;

            DbDataReader reader = null;
            TUser user = default(TUser);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                user = EntityBuilder.Build(reader);
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

            return user;
        }
    }

}
