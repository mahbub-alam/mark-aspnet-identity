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
    /// Represents role repository.
    /// </summary>
    /// <typeparam name="TRole">Role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    internal class RoleRepository<TRole, TKey, TUserRole>
        : DbRepository<TRole>
        where TRole : IdentityRole<TKey, TUserRole>, new()
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TRole item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(@"INSERT INTO {0} ({1}) VALUES (@{2});",
                DbContext[Entities.Role].TableName,
                // Configured field names
                DbContext[Entities.Role][RoleFields.Name],
                // Parameter names
                RoleFields.Name);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TRole> cmdContext = new DbCommandContext<TRole>(command,
                new List<TRole> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[RoleFields.Name].Value = entity.Name;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TRole item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(@"UPDATE {0} SET {1} = @{3} WHERE {2} = @{4};",
                DbContext[Entities.Role].TableName,
                // Configured field names
                DbContext[Entities.Role][RoleFields.Name],
                DbContext[Entities.Role][RoleFields.Id],
                // Parameter names
                RoleFields.Name,
                RoleFields.Id);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TRole> cmdContext = new DbCommandContext<TRole>(command,
                new List<TRole> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[RoleFields.Name].Value = entity.Name;
                parameters[RoleFields.Id].Value = entity.Id;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TRole item)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(@"DELETE FROM {0} WHERE {1} = @{2};",
                DbContext[Entities.Role].TableName,
                // Configured field names
                DbContext[Entities.Role][RoleFields.Id],
                // Parameter names
                RoleFields.Id);

            if (DbContext.TransactionExists)
            {
                command.Transaction = DbContext.TransactionContext.Transaction;
            }

            DbCommandContext<TRole> cmdContext = new DbCommandContext<TRole>(command,
                new List<TRole> { item });

            cmdContext.SetParametersForEach((parameters, entity) =>
            {
                parameters[RoleFields.Id].Value = entity.Id;
            });

            DbContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find role by id.
        /// </summary>
        /// <param name="id">Target role id.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public TRole FindById(TKey id)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = @{2};",
                DbContext[Entities.Role].TableName,
                // Configured field names
                DbContext[Entities.Role][RoleFields.Id],
                // Parameter names
                RoleFields.Id);

            DbCommandContext<TRole> cmdContext = new DbCommandContext<TRole>(command);
            cmdContext.Parameters[RoleFields.Id].Value = id;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            TRole role = default(TRole);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    role = new TRole();
                    role.Id = id;
                    role.Name = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.Role][RoleFields.Name]));
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

            return role;
        }

        /// <summary>
        /// Find role by role name.
        /// </summary>
        /// <param name="roleName">Target role name.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public TRole FindByName(string roleName)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                DbContext[Entities.Role].TableName,
                // Configured field names
                DbContext[Entities.Role][RoleFields.Name],
                // Parameter names
                RoleFields.Name);

            DbCommandContext<TRole> cmdContext = new DbCommandContext<TRole>(command);
            cmdContext.Parameters[RoleFields.Name].Value = roleName;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            TRole role = default(TRole);

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    role = new TRole();
                    role.Id = (TKey)reader.GetValue(reader.GetOrdinal(
                        DbContext[Entities.Role][RoleFields.Id]));
                    role.Name = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.Role][RoleFields.Name]));
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

            return role;
        }

        /// <summary>
        /// Find a list of role names by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of roles if found; otherwise, returns empty list.</returns>
        public IList<string> FindRoleNamesByUserId(TKey userId)
        {
            DbCommand command = DbContext.Connection.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT {2} FROM {0} INNER JOIN {1} ON ({0}.{3} = {1}.{4}) WHERE {5} = @{6};",
                DbContext[Entities.Role].TableName,
                DbContext[Entities.UserRole].TableName,
                // Configured field names
                DbContext[Entities.Role][RoleFields.Name],
                DbContext[Entities.Role][RoleFields.Id],
                DbContext[Entities.UserRole][UserRoleFields.RoleId],
                DbContext[Entities.UserRole][UserRoleFields.UserId],
                // Parameter names
                UserRoleFields.UserId);

            DbCommandContext<TRole> cmdContext = new DbCommandContext<TRole>(command);
            cmdContext.Parameters[UserRoleFields.UserId].Value = userId;

            DbConnection conn = DbContext.Connection;
            DbDataReader reader = null;
            List<string> list = new List<string>();
            string roleName = null;

            conn.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    roleName = reader.GetString(reader.GetOrdinal(
                        DbContext[Entities.UserRole][RoleFields.Name]));

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

                conn.Close();
            }

            return list;
        }
    }

}
