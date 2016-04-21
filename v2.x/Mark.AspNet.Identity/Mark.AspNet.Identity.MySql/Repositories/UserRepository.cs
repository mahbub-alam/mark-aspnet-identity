// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Core;
using Mark.Data;
using Mark.Data.Common;
using System.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents User repository.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login entity type.</typeparam>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TUserClaim">User claim entity type.</typeparam>
    internal class UserRepository<TUser, TKey, TUserLogin, TUserRole, TUserClaim>
        : DbRepository<TUser>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>, new()
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct, IEquatable<TKey>
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
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}) 
                    VALUES (@{12}, @{13}, @{14}, @{15}, @{16}, @{17}, @{18}, @{19}, @{20}, 
                @{21}, @{22});",
                StorageContext[Entities.User].TableName,
                // Configured field names
                StorageContext[Entities.User][UserFields.UserName],
                StorageContext[Entities.User][UserFields.PasswordHash],
                StorageContext[Entities.User][UserFields.SecurityStamp],
                StorageContext[Entities.User][UserFields.Email],
                StorageContext[Entities.User][UserFields.EmailConfirmed],
                StorageContext[Entities.User][UserFields.PhoneNumber],
                StorageContext[Entities.User][UserFields.PhoneNumberConfirmed],
                StorageContext[Entities.User][UserFields.TwoFactorEnabled],
                StorageContext[Entities.User][UserFields.LockoutEnabled],
                StorageContext[Entities.User][UserFields.LockoutEndDateUtc],
                StorageContext[Entities.User][UserFields.AccessFailedCount],
                // Parameter names
                UserFields.UserName,
                UserFields.PasswordHash,
                UserFields.SecurityStamp,
                UserFields.Email,
                UserFields.EmailConfirmed,
                UserFields.PhoneNumber,
                UserFields.PhoneNumberConfirmed,
                UserFields.TwoFactorEnabled,
                UserFields.LockoutEnabled,
                UserFields.LockoutEndDateUtc,
                UserFields.AccessFailedCount);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TUser>((parameters, entity) =>
            {
                parameters[UserFields.UserName].Value = entity.UserName;
                parameters[UserFields.PasswordHash].Value = entity.PasswordHash.GetDBNullIfNull();
                parameters[UserFields.SecurityStamp].Value = entity.SecurityStamp.GetDBNullIfNull();
                parameters[UserFields.Email].Value = entity.Email.GetDBNullIfNull();
                parameters[UserFields.EmailConfirmed].Value = entity.EmailConfirmed;
                parameters[UserFields.PhoneNumber].Value = entity.PhoneNumber.GetDBNullIfNull();
                parameters[UserFields.PhoneNumberConfirmed].Value = entity.PhoneNumberConfirmed;
                parameters[UserFields.TwoFactorEnabled].Value = entity.TwoFactorEnabled;
                parameters[UserFields.LockoutEnabled].Value = entity.LockoutEnabled;
                parameters[UserFields.LockoutEndDateUtc].Value = entity.LockoutEndDateUtc.GetDBNullIfNull();
                parameters[UserFields.AccessFailedCount].Value = entity.AccessFailedCount;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUser item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"UPDATE {0} SET 
                {1} = @{13}, 
                {2} = @{14}, 
                {3} = @{15}, 
                {4} = @{16}, 
                {5} = @{17}, 
                {6} = @{18}, 
                {7} = @{19}, 
                {8} = @{20}, 
                {9} = @{21}, 
                {10} = @{22},  
                {11} = @{23}
                WHERE {12} = @{24};",
                StorageContext[Entities.User].TableName,
                // Configured field names
                StorageContext[Entities.User][UserFields.UserName],
                StorageContext[Entities.User][UserFields.PasswordHash],
                StorageContext[Entities.User][UserFields.SecurityStamp],
                StorageContext[Entities.User][UserFields.Email],
                StorageContext[Entities.User][UserFields.EmailConfirmed],
                StorageContext[Entities.User][UserFields.PhoneNumber],
                StorageContext[Entities.User][UserFields.PhoneNumberConfirmed],
                StorageContext[Entities.User][UserFields.TwoFactorEnabled],
                StorageContext[Entities.User][UserFields.LockoutEnabled],
                StorageContext[Entities.User][UserFields.LockoutEndDateUtc],
                StorageContext[Entities.User][UserFields.AccessFailedCount],
                StorageContext[Entities.User][UserFields.Id],
                // Parameter names
                UserFields.UserName,
                UserFields.PasswordHash,
                UserFields.SecurityStamp,
                UserFields.Email,
                UserFields.EmailConfirmed,
                UserFields.PhoneNumber,
                UserFields.PhoneNumberConfirmed,
                UserFields.TwoFactorEnabled,
                UserFields.LockoutEnabled,
                UserFields.LockoutEndDateUtc,
                UserFields.AccessFailedCount,
                UserFields.Id);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TUser>((parameters, entity) =>
            {
                parameters[UserFields.Id].Value = entity.Id;
                parameters[UserFields.UserName].Value = entity.UserName;
                parameters[UserFields.PasswordHash].Value = entity.PasswordHash.GetDBNullIfNull();
                parameters[UserFields.SecurityStamp].Value = entity.SecurityStamp.GetDBNullIfNull();
                parameters[UserFields.Email].Value = entity.Email;
                parameters[UserFields.EmailConfirmed].Value = entity.EmailConfirmed;
                parameters[UserFields.PhoneNumber].Value = entity.PhoneNumber.GetDBNullIfNull();
                parameters[UserFields.PhoneNumberConfirmed].Value = entity.PhoneNumberConfirmed;
                parameters[UserFields.TwoFactorEnabled].Value = entity.TwoFactorEnabled;
                parameters[UserFields.LockoutEnabled].Value = entity.LockoutEnabled;
                parameters[UserFields.LockoutEndDateUtc].Value = entity.LockoutEndDateUtc.GetDBNullIfNull();
                parameters[UserFields.AccessFailedCount].Value = entity.AccessFailedCount;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUser item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"DELETE FROM {0} WHERE {1} = @{2};",
                StorageContext[Entities.User].TableName,
                // Configured field names
                StorageContext[Entities.User][UserFields.Id],
                // Parameter names
                UserFields.Id);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TUser>((parameters, entity) =>
            {
                parameters[UserFields.Id].Value = entity.Id;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="id">Target user id.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindById(TKey id)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                StorageContext[Entities.User].TableName,
                // Configured field names
                StorageContext[Entities.User][UserFields.Id],
                // Parameter names
                UserFields.Id);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserFields.Id].Value = id;

            DbDataReader reader = null;
            TUser user = default(TUser);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    user = new TUser();

                    user.Id = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.User][UserFields.Id]);
                    user.UserName = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.UserName]);
                    user.PasswordHash = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.PasswordHash]);
                    user.SecurityStamp = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.SecurityStamp]);
                    user.Email = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.Email]);
                    user.EmailConfirmed = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.EmailConfirmed]);
                    user.PhoneNumber = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.PhoneNumber]);
                    user.PhoneNumberConfirmed = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.PhoneNumberConfirmed]);
                    user.TwoFactorEnabled = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.TwoFactorEnabled]);
                    user.LockoutEnabled = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.LockoutEnabled]);
                    user.LockoutEndDateUtc = reader.GetSafeDateTime(
                        StorageContext[Entities.User][UserFields.LockoutEndDateUtc]);
                    user.AccessFailedCount = reader.GetSafeInt32(
                        StorageContext[Entities.User][UserFields.AccessFailedCount]);
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

            return user;
        }

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="userName">Target user username.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindByUserName(string userName)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                StorageContext[Entities.User].TableName,
                // Configured field names
                StorageContext[Entities.User][UserFields.UserName],
                // Parameter names
                UserFields.UserName);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserFields.UserName].Value = userName;

            DbDataReader reader = null;
            TUser user = default(TUser);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    user = new TUser();

                    user.Id = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.User][UserFields.Id]);
                    user.UserName = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.UserName]);
                    user.PasswordHash = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.PasswordHash]);
                    user.SecurityStamp = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.SecurityStamp]);
                    user.Email = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.Email]);
                    user.EmailConfirmed = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.EmailConfirmed]);
                    user.PhoneNumber = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.PhoneNumber]);
                    user.PhoneNumberConfirmed = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.PhoneNumberConfirmed]);
                    user.TwoFactorEnabled = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.TwoFactorEnabled]);
                    user.LockoutEnabled = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.LockoutEnabled]);
                    user.LockoutEndDateUtc = reader.GetSafeDateTime(
                        StorageContext[Entities.User][UserFields.LockoutEndDateUtc]);
                    user.AccessFailedCount = reader.GetSafeInt32(
                        StorageContext[Entities.User][UserFields.AccessFailedCount]);
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

            return user;
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="email">Target user email.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindByEmail(string email)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                StorageContext[Entities.User].TableName,
                // Configured field names
                StorageContext[Entities.User][UserFields.Email],
                // Parameter names
                UserFields.Email);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserFields.Email].Value = email;

            DbDataReader reader = null;
            TUser user = default(TUser);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    user = new TUser();

                    user.Id = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.User][UserFields.Id]);
                    user.UserName = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.UserName]);
                    user.PasswordHash = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.PasswordHash]);
                    user.SecurityStamp = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.SecurityStamp]);
                    user.Email = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.Email]);
                    user.EmailConfirmed = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.EmailConfirmed]);
                    user.PhoneNumber = reader.GetSafeString(
                        StorageContext[Entities.User][UserFields.PhoneNumber]);
                    user.PhoneNumberConfirmed = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.PhoneNumberConfirmed]);
                    user.TwoFactorEnabled = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.TwoFactorEnabled]);
                    user.LockoutEnabled = reader.GetSafeBoolean(
                        StorageContext[Entities.User][UserFields.LockoutEnabled]);
                    user.LockoutEndDateUtc = reader.GetSafeDateTime(
                        StorageContext[Entities.User][UserFields.LockoutEndDateUtc]);
                    user.AccessFailedCount = reader.GetSafeInt32(
                        StorageContext[Entities.User][UserFields.AccessFailedCount]);
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

            return user;
        }
    }

}
