// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Mark.AspNet.Identity.Common;
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
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public UserLoginRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TUserLogin item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(@"INSERT INTO {0} ({1}, {2}, {3}) VALUES (@{4}, @{5}, @{6});",
                DbContext[Entities.UserLogin].TableName,
                // Configured field names
                DbContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                DbContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                DbContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey,
                UserLoginFields.UserId);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUserLogin> cmdContext = new DbCommandContext<TUserLogin>(command,
                new List<TUserLogin> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserLoginFields.LoginProvider].Value = entity.LoginProvider;
                parameters[UserLoginFields.ProviderKey].Value =  entity.ProviderKey;
                parameters[UserLoginFields.UserId].Value = entity.UserId;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// (Not implemented) Save the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUserLogin item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUserLogin item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(@"DELETE FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                DbContext[Entities.UserLogin].TableName,
                // Configured field names
                DbContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                DbContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                DbContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey,
                UserLoginFields.UserId);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUserLogin> cmdContext = new DbCommandContext<TUserLogin>(command,
                new List<TUserLogin> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserLoginFields.LoginProvider].Value = entity.LoginProvider;
                parameters[UserLoginFields.ProviderKey].Value = entity.ProviderKey;
                parameters[UserLoginFields.UserId].Value = entity.UserId;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find user login by login information.
        /// </summary>
        /// <param name="loginInfo">User login information.</param>
        /// <returns>Returns the user login if found; otherwise, returns null.</returns>
        public TUserLogin Find(UserLoginInfo loginInfo)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = @{3} AND {2} = @{4};",
                DbContext[Entities.UserLogin].TableName,
                // Configured field names
                DbContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                DbContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey);

            DbCommandContext<TUserLogin> cmdContext = new DbCommandContext<TUserLogin>(command);
            cmdContext.Parameters[UserLoginFields.LoginProvider].Value = loginInfo.LoginProvider;
            cmdContext.Parameters[UserLoginFields.ProviderKey].Value = loginInfo.ProviderKey;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            TUserLogin UserLogin = default(TUserLogin);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    UserLogin = new TUserLogin();

                    UserLogin.LoginProvider = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.LoginProvider]));

                    UserLogin.ProviderKey = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.ProviderKey]));

                    UserLogin.UserId = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.UserId]));
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
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{4} AND {2} = @{5} AND {3} = @{6};",
                DbContext[Entities.UserLogin].TableName,
                // Configured field names
                DbContext[Entities.UserLogin][UserLoginFields.LoginProvider],
                DbContext[Entities.UserLogin][UserLoginFields.ProviderKey],
                DbContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.LoginProvider,
                UserLoginFields.ProviderKey,
                UserLoginFields.UserId);

            DbCommandContext<TUserLogin> cmdContext = new DbCommandContext<TUserLogin>(command);
            cmdContext.Parameters[UserLoginFields.LoginProvider].Value = loginInfo.LoginProvider;
            cmdContext.Parameters[UserLoginFields.ProviderKey].Value = loginInfo.ProviderKey;
            cmdContext.Parameters[UserLoginFields.UserId].Value = userId;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            TUserLogin UserLogin = default(TUserLogin);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    UserLogin = new TUserLogin();

                    UserLogin.LoginProvider = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.LoginProvider]));

                    UserLogin.ProviderKey = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.ProviderKey]));

                    UserLogin.UserId = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.UserId]));
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

            return UserLogin;
        }

        /// <summary>
        /// Find all user UserLogins by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user UserLogins if found; otherwise, returns empty list.</returns>
        public ICollection<TUserLogin> FindAllByUserId(TKey userId)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = @{2};",
                DbContext[Entities.UserLogin].TableName,
                // Configured field names
                DbContext[Entities.UserLogin][UserLoginFields.UserId],
                // Parameter names
                UserLoginFields.UserId);

            DbCommandContext<TUserLogin> cmdContext = new DbCommandContext<TUserLogin>(command);
            cmdContext.Parameters[UserLoginFields.UserId].Value = userId;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            List<TUserLogin> list = new List<TUserLogin>();
            TUserLogin UserLogin = default(TUserLogin);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    UserLogin = new TUserLogin();

                    UserLogin.LoginProvider = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.LoginProvider]));

                    UserLogin.ProviderKey = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.ProviderKey]));

                    UserLogin.UserId = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.UserLogin][UserLoginFields.UserId]));

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

                conn.Close();
            }

            return list;
        }
    }
}
