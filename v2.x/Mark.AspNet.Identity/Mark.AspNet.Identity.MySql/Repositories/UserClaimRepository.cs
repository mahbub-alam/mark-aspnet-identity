// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Mark.AspNet.Identity.Common;
using System.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents user claim repository.
    /// </summary>
    /// <typeparam name="TUserClaim">User claim entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    internal class UserClaimRepository<TUserClaim, TKey>
        : DbRepository<TUserClaim>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TKey : struct
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
            DbCommand command = StorageContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"INSERT INTO {0} ({1}, {2}, {3}) VALUES (@{4}, @{5}, @{6});",
                StorageContext[Entities.UserClaim].TableName,
                // Configured field names
                StorageContext[Entities.UserClaim][UserClaimFields.ClaimType],
                StorageContext[Entities.UserClaim][UserClaimFields.ClaimValue],
                StorageContext[Entities.UserClaim][UserClaimFields.UserId],
                // Parameter names
                UserClaimFields.ClaimType,
                UserClaimFields.ClaimValue,
                UserClaimFields.UserId);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUserClaim> cmdContext = new DbCommandContext<TUserClaim>(command,
                new List<TUserClaim> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserClaimFields.ClaimType].Value = entity.ClaimType;
                parameters[UserClaimFields.ClaimValue].Value = entity.ClaimValue;
                parameters[UserClaimFields.UserId].Value = entity.UserId;
            });

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
            DbCommand command = StorageContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"DELETE FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                StorageContext[Entities.UserClaim].TableName,
                // Configured field names
                StorageContext[Entities.UserClaim][UserClaimFields.ClaimType],
                StorageContext[Entities.UserClaim][UserClaimFields.ClaimValue],
                StorageContext[Entities.UserClaim][UserClaimFields.UserId],
                // Parameter names
                UserClaimFields.ClaimType,
                UserClaimFields.ClaimValue,
                UserClaimFields.UserId);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUserClaim> cmdContext = new DbCommandContext<TUserClaim>(command,
                new List<TUserClaim> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserClaimFields.ClaimType].Value = entity.ClaimType;
                parameters[UserClaimFields.ClaimValue].Value = entity.ClaimValue;
                parameters[UserClaimFields.UserId].Value = entity.UserId;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find all user claims by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user claims if found; otherwise, returns empty list.</returns>
        public ICollection<TUserClaim> FindAllByUserId(TKey userId)
        {
            DbCommand command = StorageContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                StorageContext[Entities.UserClaim].TableName,
                // Configured field names
                StorageContext[Entities.UserClaim][UserClaimFields.UserId],
                // Parameter names
                UserClaimFields.UserId);

            DbCommandContext<TUserClaim> cmdContext = new DbCommandContext<TUserClaim>(command);
            cmdContext.Parameters[UserClaimFields.UserId].Value = userId;

            DbConnection conn = StorageContext.Connection;
            DbDataReader reader = null;
            List<TUserClaim> list = new List<TUserClaim>();
            TUserClaim userClaim = default(TUserClaim);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    userClaim = new TUserClaim();
                    userClaim.Id = (TKey)reader.GetValue(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.Id]));
                    userClaim.ClaimType = reader.GetString(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.ClaimType]));
                    userClaim.ClaimValue = reader.GetString(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.ClaimValue]));
                    userClaim.UserId = (TKey)reader.GetValue(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.UserId]));

                    list.Add(userClaim);
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

                conn.Close();
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
            DbCommand command = StorageContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                StorageContext[Entities.UserClaim].TableName,
                // Configured field names
                StorageContext[Entities.UserClaim][UserClaimFields.UserId],
                StorageContext[Entities.UserClaim][UserClaimFields.ClaimType],
                StorageContext[Entities.UserClaim][UserClaimFields.ClaimValue],
                // Parameter names
                UserClaimFields.UserId,
                UserClaimFields.ClaimType,
                UserClaimFields.ClaimValue);

            DbCommandContext<TUserClaim> cmdContext = new DbCommandContext<TUserClaim>(command);
            cmdContext.Parameters[UserClaimFields.UserId].Value = userId;
            cmdContext.Parameters[UserClaimFields.ClaimType].Value = claim.Type;
            cmdContext.Parameters[UserClaimFields.ClaimValue].Value = claim.Value;

            DbConnection conn = StorageContext.Connection;
            DbDataReader reader = null;
            List<TUserClaim> list = new List<TUserClaim>();
            TUserClaim userClaim = default(TUserClaim);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    userClaim = new TUserClaim();
                    userClaim.Id = (TKey)reader.GetValue(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.Id]));
                    userClaim.ClaimType = reader.GetString(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.ClaimType]));
                    userClaim.ClaimValue = reader.GetString(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.ClaimValue]));
                    userClaim.UserId = (TKey)reader.GetValue(reader.GetOrdinal(
                        StorageContext[Entities.UserClaim][UserClaimFields.UserId]));

                    list.Add(userClaim);
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

                conn.Close();
            }

            return list;
        }

    }
}
