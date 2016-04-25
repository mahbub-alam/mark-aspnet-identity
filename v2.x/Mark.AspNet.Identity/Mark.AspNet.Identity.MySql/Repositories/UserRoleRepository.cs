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
using Mark.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents user role repository.
    /// </summary>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class UserRoleRepository<TUserRole, TKey>
        : MySqlRepository<TUserRole> 
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
            DbCommandContext cmdContext = CommandBuilder.GetInsertCommand(
                new List<TUserRole> { item });

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
            DbCommandContext cmdContext = CommandBuilder.GetDeleteCommand(
                new List<TUserRole> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find all user roles by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user roles if found; otherwise, returns empty list.</returns>
        public ICollection<TUserRole> FindAllByUserId(TKey userId)
        {
            PropertyConfiguration userIdPropCfg = Configuration.Property(p => p.UserId);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(userIdPropCfg.ColumnName),
                // Parameter names
                userIdPropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[userIdPropCfg.PropertyName].Value = userId;

            DbDataReader reader = null;
            ICollection<TUserRole> list = null;

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

                StorageContext.Close();
            }

            return list;
        }
    }
}
