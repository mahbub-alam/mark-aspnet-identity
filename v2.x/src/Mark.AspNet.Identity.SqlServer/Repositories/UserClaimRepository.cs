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
using System.Security.Claims;
using System.Data.Common;
using Mark.DotNet.Data;
using Mark.DotNet.Data.Common;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.SqlServer
{
    /// <summary>
    /// Represents user claim repository.
    /// </summary>
    /// <typeparam name="TUserClaim">User claim entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    internal class UserClaimRepository<TUserClaim, TKey>
        : SqlRepository<TUserClaim>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public UserClaimRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save adding of the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TUserClaim item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetInsertCommand(
                new List<TUserClaim> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// (Not implemented) Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUserClaim item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUserClaim item)
        {
            DbCommandContext cmdContext = CommandBuilder.GetDeleteCommand(
                new List<TUserClaim> { item });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find all user claims by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user claims if found; otherwise, returns empty list.</returns>
        public ICollection<TUserClaim> FindAllByUserId(TKey userId)
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
            ICollection<TUserClaim> list = null;

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

        /// <summary>
        /// Find all user claims by user id and the given claim.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <param name="claim">Target claim.</param>
        /// <returns>Returns a list of user claims if found; otherwise, returns empty list.</returns>
        public ICollection<TUserClaim> FindAllByUserId(TKey userId, Claim claim)
        {
            PropertyConfiguration userIdPropCfg = Configuration.Property(p => p.UserId);
            PropertyConfiguration claimTypePropCfg = Configuration.Property(p => p.ClaimType);
            PropertyConfiguration claimValuePropCfg = Configuration.Property(p => p.ClaimValue);
            DbCommand command = StorageContext.CreateCommand();

            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                QueryBuilder.GetQuotedIdentifier(Configuration.TableName),
                // Configured field names
                QueryBuilder.GetQuotedIdentifier(userIdPropCfg.ColumnName),
                QueryBuilder.GetQuotedIdentifier(claimTypePropCfg.ColumnName),
                QueryBuilder.GetQuotedIdentifier(claimValuePropCfg.ColumnName),
                // Parameter names
                userIdPropCfg.PropertyName,
                claimTypePropCfg.PropertyName,
                claimValuePropCfg.PropertyName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[userIdPropCfg.PropertyName].Value = userId;
            cmdContext.Parameters[claimTypePropCfg.PropertyName].Value = claim.Type;
            cmdContext.Parameters[claimValuePropCfg.PropertyName].Value = claim.Value;

            DbDataReader reader = null;
            ICollection<TUserClaim> list = null;

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
