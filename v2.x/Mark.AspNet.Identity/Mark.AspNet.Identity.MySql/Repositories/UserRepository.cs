// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity.Common;
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
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TUser item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}) 
                    VALUES (@{12}, @{13}, @{14}, @{15}, @{16}, @{17}, @{18}, @{19}, @{20}, 
                @{21}, @{22});",
                DbContext[Entities.User].TableName,
                // Configured field names
                DbContext[Entities.User][UserFields.UserName],
                DbContext[Entities.User][UserFields.PasswordHash],
                DbContext[Entities.User][UserFields.SecurityStamp],
                DbContext[Entities.User][UserFields.Email],
                DbContext[Entities.User][UserFields.EmailConfirmed],
                DbContext[Entities.User][UserFields.PhoneNumber],
                DbContext[Entities.User][UserFields.PhoneNumberConfirmed],
                DbContext[Entities.User][UserFields.TwoFactorEnabled],
                DbContext[Entities.User][UserFields.LockoutEnabled],
                DbContext[Entities.User][UserFields.LockoutEndDateUtc],
                DbContext[Entities.User][UserFields.AccessFailedCount],
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

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUser> cmdContext = new DbCommandContext<TUser>(command,
                new List<TUser> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserFields.UserName].Value = entity.UserName;
                parameters[UserFields.PasswordHash].Value = entity.PasswordHash;
                parameters[UserFields.SecurityStamp].Value = entity.SecurityStamp;
                parameters[UserFields.Email].Value = entity.Email;
                parameters[UserFields.EmailConfirmed].Value = entity.EmailConfirmed;
                parameters[UserFields.PhoneNumber].Value = entity.PhoneNumber;
                parameters[UserFields.PhoneNumberConfirmed].Value = entity.PhoneNumberConfirmed;
                parameters[UserFields.TwoFactorEnabled].Value = entity.TwoFactorEnabled;
                parameters[UserFields.LockoutEnabled].Value = entity.LockoutEnabled;
                parameters[UserFields.LockoutEndDateUtc].Value = entity.LockoutEndDateUtc;
                parameters[UserFields.AccessFailedCount].Value = entity.AccessFailedCount;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUser item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"UPDATE {0} SET 
                {1} = @{12}, 
                {2} = @{13}, 
                {3} = @{14}, 
                {4} = @{15}, 
                {5} = @{16}, 
                {6} = @{17}, 
                {7} = @{18}, 
                {8} = @{19}, 
                {9} = @{20}, 
                {10} = @{21}, 
                WHERE {11} = @{22};",
                DbContext[Entities.User].TableName,
                // Configured field names
                DbContext[Entities.User][UserFields.PasswordHash],
                DbContext[Entities.User][UserFields.SecurityStamp],
                DbContext[Entities.User][UserFields.Email],
                DbContext[Entities.User][UserFields.EmailConfirmed],
                DbContext[Entities.User][UserFields.PhoneNumber],
                DbContext[Entities.User][UserFields.PhoneNumberConfirmed],
                DbContext[Entities.User][UserFields.TwoFactorEnabled],
                DbContext[Entities.User][UserFields.LockoutEnabled],
                DbContext[Entities.User][UserFields.LockoutEndDateUtc],
                DbContext[Entities.User][UserFields.AccessFailedCount],
                DbContext[Entities.User][UserFields.Id],
                // Parameter names
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

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUser> cmdContext = new DbCommandContext<TUser>(command,
                new List<TUser> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserFields.Id].Value = entity.Id;
                parameters[UserFields.PasswordHash].Value = entity.PasswordHash;
                parameters[UserFields.SecurityStamp].Value = entity.SecurityStamp;
                parameters[UserFields.Email].Value = entity.Email;
                parameters[UserFields.EmailConfirmed].Value = entity.EmailConfirmed;
                parameters[UserFields.PhoneNumber].Value = entity.PhoneNumber;
                parameters[UserFields.PhoneNumberConfirmed].Value = entity.PhoneNumberConfirmed;
                parameters[UserFields.TwoFactorEnabled].Value = entity.TwoFactorEnabled;
                parameters[UserFields.LockoutEnabled].Value = entity.LockoutEnabled;
                parameters[UserFields.LockoutEndDateUtc].Value = entity.LockoutEndDateUtc;
                parameters[UserFields.AccessFailedCount].Value = entity.AccessFailedCount;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUser item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"DELETE FROM {0} WHERE {1} = @{2};",
                DbContext[Entities.User].TableName,
                // Configured field names
                DbContext[Entities.User][UserFields.Id],
                // Parameter names
                UserFields.Id);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUser> cmdContext = new DbCommandContext<TUser>(command,
                new List<TUser> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserFields.Id].Value = entity.Id;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="id">Target user id.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindById(TKey id)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                DbContext[Entities.User].TableName,
                // Configured field names
                DbContext[Entities.User][UserFields.Id],
                // Parameter names
                UserFields.Id);

            DbCommandContext<TUser> cmdContext = new DbCommandContext<TUser>(command);
            cmdContext.Parameters[UserFields.Id].Value = id;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            TUser user = default(TUser);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    user = new TUser();

                    user.Id = id;

                    user.UserName = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.UserName]));

                    user.PasswordHash = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PasswordHash]));

                    user.SecurityStamp = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.SecurityStamp]));

                    user.Email = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.Email]));

                    user.EmailConfirmed = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.EmailConfirmed]));

                    user.PhoneNumber = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PhoneNumber]));

                    user.PhoneNumberConfirmed = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PhoneNumberConfirmed]));

                    user.TwoFactorEnabled = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.TwoFactorEnabled]));

                    user.LockoutEnabled = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.LockoutEnabled]));

                    user.LockoutEndDateUtc = reader.GetDateTime(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.LockoutEndDateUtc]));

                    user.AccessFailedCount = reader.GetInt32(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.AccessFailedCount]));
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

            return user;
        }

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="userName">Target user username.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindByUserName(string userName)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                DbContext[Entities.User].TableName,
                // Configured field names
                DbContext[Entities.User][UserFields.UserName],
                // Parameter names
                UserFields.UserName);

            DbCommandContext<TUser> cmdContext = new DbCommandContext<TUser>(command);
            cmdContext.Parameters[UserFields.UserName].Value = userName;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            TUser user = default(TUser);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    user = new TUser();

                    user.Id = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.Id]));

                    user.UserName = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.UserName]));

                    user.PasswordHash = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PasswordHash]));

                    user.SecurityStamp = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.SecurityStamp]));

                    user.Email = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.Email]));

                    user.EmailConfirmed = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.EmailConfirmed]));

                    user.PhoneNumber = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PhoneNumber]));

                    user.PhoneNumberConfirmed = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PhoneNumberConfirmed]));

                    user.TwoFactorEnabled = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.TwoFactorEnabled]));

                    user.LockoutEnabled = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.LockoutEnabled]));

                    user.LockoutEndDateUtc = reader.GetDateTime(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.LockoutEndDateUtc]));

                    user.AccessFailedCount = reader.GetInt32(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.AccessFailedCount]));
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

            return user;
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="email">Target user email.</param>
        /// <returns>Returns the user if found; otherwise, returns null.</returns>
        public TUser FindByEmail(string email)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                DbContext[Entities.User].TableName,
                // Configured field names
                DbContext[Entities.User][UserFields.Email],
                // Parameter names
                UserFields.Email);

            DbCommandContext<TUser> cmdContext = new DbCommandContext<TUser>(command);
            cmdContext.Parameters[UserFields.Email].Value = email;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            TUser user = default(TUser);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    user = new TUser();

                    user.Id = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.Id]));

                    user.UserName = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.UserName]));

                    user.PasswordHash = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PasswordHash]));

                    user.SecurityStamp = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.SecurityStamp]));

                    user.Email = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.Email]));

                    user.EmailConfirmed = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.EmailConfirmed]));

                    user.PhoneNumber = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PhoneNumber]));

                    user.PhoneNumberConfirmed = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.PhoneNumberConfirmed]));

                    user.TwoFactorEnabled = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.TwoFactorEnabled]));

                    user.LockoutEnabled = reader.GetBoolean(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.LockoutEnabled]));

                    user.LockoutEndDateUtc = reader.GetDateTime(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.LockoutEndDateUtc]));

                    user.AccessFailedCount = reader.GetInt32(reader.GetOrdinal(
                        DbContext[Entities.User][UserFields.AccessFailedCount]));
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

            return user;
        }
    }

}
