﻿// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity;
using Mark.AspNet.Identity.Common;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents user role repository.
    /// </summary>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    internal class UserRoleRepository<TUserRole, TKey>
        : DbRepository<TUserRole> 
        where TUserRole : IdentityUserRole<TKey>, new()
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TUserRole item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(@"INSERT INTO {0} ({1}, {2}) VALUES (@{3}, @{4});",
                DbContext[Entities.UserRole].TableName,
                // Configured field names
                DbContext[Entities.UserRole][UserRoleFields.UserId],
                DbContext[Entities.UserRole][UserRoleFields.RoleId],
                // Parameter names
                UserRoleFields.UserId,
                UserRoleFields.RoleId);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUserRole> cmdContext = new DbCommandContext<TUserRole>(command,
                new List<TUserRole> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserRoleFields.UserId].Value = entity.UserId;
                parameters[UserRoleFields.RoleId].Value = entity.RoleId;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// (Not implemented) Save the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TUserRole item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TUserRole item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(@"DELETE FROM {0} WHERE {1} = @{3} AND {2} = @{4};",
                DbContext[Entities.UserRole].TableName,
                // Configured field names
                DbContext[Entities.UserRole][UserRoleFields.UserId],
                DbContext[Entities.UserRole][UserRoleFields.RoleId], 
                // Parameter names
                UserRoleFields.UserId, 
                UserRoleFields.RoleId);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TUserRole> cmdContext = new DbCommandContext<TUserRole>(command,
                new List<TUserRole> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[UserRoleFields.UserId].Value = entity.UserId;
                parameters[UserRoleFields.RoleId].Value = entity.RoleId;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find all user roles by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of user roles if found; otherwise, returns empty list.</returns>
        public ICollection<TUserRole> FindAllByUserId(TKey userId)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = @{2};",
                DbContext[Entities.UserRole].TableName,
                // Configured field names
                DbContext[Entities.UserRole][UserRoleFields.UserId],
                // Parameter names
                UserRoleFields.UserId);

            DbCommandContext<TUserRole> cmdContext = new DbCommandContext<TUserRole>(command);
            cmdContext.Parameters[UserRoleFields.UserId].Value = userId;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            List<TUserRole> list = new List<TUserRole>();
            TUserRole userRole = default(TUserRole);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    userRole = new TUserRole();
                    userRole.UserId = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.UserRole][UserRoleFields.UserId]));
                    userRole.RoleId = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.UserRole][UserRoleFields.RoleId]));

                    list.Add(userRole);
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
        /// Check whether the user belongs to the given role.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <param name="roleName">Target role name.</param>
        /// <returns>Returns true if belongs; otherwise, returns false.</returns>
        public bool IsInRole(TKey userId, string roleName)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT {2} FROM {0} INNER JOIN {1} ON ({0}.{2} = {1}.{3}) 
                    WHERE {4} = @{6} AND LOWER({5}) = LOWER(@{7});",
                DbContext[Entities.UserRole].TableName,
                DbContext[Entities.Role].TableName,
                // Configured field names
                DbContext[Entities.UserRole][UserRoleFields.RoleId],
                DbContext[Entities.Role][RoleFields.Id],
                DbContext[Entities.UserRole][UserRoleFields.UserId],
                DbContext[Entities.Role][RoleFields.Name],
                // Parameter names
                UserRoleFields.UserId, 
                RoleFields.Name);

            DbCommandContext<IEntity> cmdContext = new DbCommandContext<IEntity>(command);
            cmdContext.Parameters[UserRoleFields.UserId].Value = userId;
            cmdContext.Parameters[RoleFields.Name].Value = roleName;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            bool inRole = false;

            conn.Open();

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

                conn.Close();
            }

            return inRole;
        }

    }
}