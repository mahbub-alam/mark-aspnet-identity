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
using System.Data.Common;
using Mark.DotNet.Data;
using Mark.DotNet.Data.Common;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents role repository.
    /// </summary>
    /// <typeparam name="TRole">Role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    public class RoleRepository<TRole, TKey, TUserRole>
        : MySqlRepository<TRole>
        where TRole : IdentityRole<TKey, TUserRole>, new()
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save adding of the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TRole item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetInsertCommand(
                new List<TRole> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TRole item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetUpdateCommand(
                new List<TRole> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TRole item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetDeleteCommand(
                new List<TRole> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find role by id.
        /// </summary>
        /// <param name="id">Target role id.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public TRole FindById(TKey id)
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
            TRole role = default(TRole);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                role = EntityBuilder.Build(reader);
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

            return role;
        }

        /// <summary>
        /// Find role by role name.
        /// </summary>
        /// <param name="roleName">Target role name.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public TRole FindByName(string roleName)
        {
            PropertyConfiguration namePropCfg = Configuration.Property(p => p.Name);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(namePropCfg.ColumnName),
                // Parameter names
                namePropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[namePropCfg.PropertyName].Value = roleName;

            DbDataReader reader = null;
            TRole role = default(TRole);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();
                role = EntityBuilder.Build(reader);
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

            return role;
        }

        /// <summary>
        /// Find a list of role names by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of roles if found; otherwise, returns empty list.</returns>
        public IList<string> FindRoleNamesByUserId(TKey userId)
        {
            EntityConfiguration<TUserRole> userRoleCfg = StorageContext.GetEntityConfiguration<TUserRole>();
            PropertyConfiguration userIdPropCfg = userRoleCfg.Property(p => p.UserId);
            PropertyConfiguration roleNamePropCfg = Configuration.Property(p => p.Name);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT {2} FROM {0} INNER JOIN {1} ON ({0}.{3} = {1}.{4}) WHERE {5} = @{6};",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                QueryBuilder.GetQuotedIdentifier(userRoleCfg.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(roleNamePropCfg.ColumnName),
                QueryBuilder.GetQuotedIdentifier(Configuration.Property(p => p.Id).ColumnName),
                QueryBuilder.GetQuotedIdentifier(userRoleCfg.Property(p => p.RoleId).ColumnName),
                QueryBuilder.GetQuotedIdentifier(userIdPropCfg.ColumnName),
                // Parameter names
                userIdPropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[userIdPropCfg.PropertyName].Value = userId;

            DbDataReader reader = null;
            List<string> list = new List<string>();
            string roleName = null;

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    roleName = reader.GetSafeString(roleNamePropCfg.ColumnName);

                    list.Add(roleName);
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

        /// <summary>
        /// Check whether the user belongs to the given role.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <param name="roleName">Target role name.</param>
        /// <returns>Returns true if belongs; otherwise, returns false.</returns>
        public bool IsInRole(TKey userId, string roleName)
        {
            EntityConfiguration<TUserRole> userRoleCfg = StorageContext.GetEntityConfiguration<TUserRole>();
            PropertyConfiguration userIdPropCfg = userRoleCfg.Property(p => p.UserId);
            PropertyConfiguration roleNamePropCfg = Configuration.Property(p => p.Name);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT {2} FROM {0} INNER JOIN {1} ON ({0}.{2} = {1}.{3}) 
                    WHERE {4} = @{6} AND LOWER({5}) = LOWER(@{7});",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                QueryBuilder.GetQuotedIdentifier(userRoleCfg.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(Configuration.Property(p => p.Id).ColumnName),
                QueryBuilder.GetQuotedIdentifier(userRoleCfg.Property(p => p.RoleId).ColumnName),
                QueryBuilder.GetQuotedIdentifier(userIdPropCfg.ColumnName),
                QueryBuilder.GetQuotedIdentifier(roleNamePropCfg.ColumnName),
                // Parameter names
                userIdPropCfg.PropertyName,
                roleNamePropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[userIdPropCfg.PropertyName].Value = userId;
            cmdContext.Parameters[roleNamePropCfg.PropertyName].Value = roleName;

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
