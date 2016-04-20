// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Data;
using Mark.Data.Common;
using System.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.SqlServer
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
        /// Save adding of the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveAddedItem(TRole item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"INSERT INTO {0} ({1}) VALUES (@{2});",
                StorageContext[Entities.Role].TableName,
                // Configured field names
                StorageContext[Entities.Role][RoleFields.Name],
                // Parameter names
                RoleFields.Name);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TRole>((parameters, entity) =>
            {
                parameters[RoleFields.Name].Value = entity.Name;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveChangedItem(TRole item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"UPDATE {0} SET {1} = @{3} WHERE {2} = @{4};",
                StorageContext[Entities.Role].TableName,
                // Configured field names
                StorageContext[Entities.Role][RoleFields.Name],
                StorageContext[Entities.Role][RoleFields.Id],
                // Parameter names
                RoleFields.Name,
                RoleFields.Id);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TRole>((parameters, entity) =>
            {
                parameters[RoleFields.Name].Value = entity.Name;
                parameters[RoleFields.Id].Value = entity.Id;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected override void SaveRemovedItem(TRole item)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"DELETE FROM {0} WHERE {1} = @{2};",
                StorageContext[Entities.Role].TableName,
                // Configured field names
                StorageContext[Entities.Role][RoleFields.Id],
                // Parameter names
                RoleFields.Id);

            if (StorageContext.TransactionExists)
            {
                command.Transaction = StorageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command,
                new List<IEntity> { item });

            cmdContext.SetParametersForEach<TRole>((parameters, entity) =>
            {
                parameters[RoleFields.Id].Value = entity.Id;
            });

            StorageContext.AddCommand(cmdContext);
        }

        /// <summary>
        /// Find role by id.
        /// </summary>
        /// <param name="id">Target role id.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public TRole FindById(TKey id)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE {1} = @{2};",
                StorageContext[Entities.Role].TableName,
                // Configured field names
                StorageContext[Entities.Role][RoleFields.Id],
                // Parameter names
                RoleFields.Id);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[RoleFields.Id].Value = id;

            DbDataReader reader = null;
            TRole role = default(TRole);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    role = new TRole();
                    role.Id = id;
                    role.Name = reader.GetString(reader.GetOrdinal(
                        StorageContext[Entities.Role][RoleFields.Name]));
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

            return role;
        }

        /// <summary>
        /// Find role by role name.
        /// </summary>
        /// <param name="roleName">Target role name.</param>
        /// <returns>Returns the role if found; otherwise, returns null.</returns>
        public TRole FindByName(string roleName)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT * FROM {0} WHERE LOWER({1}) = LOWER(@{2});",
                StorageContext[Entities.Role].TableName,
                // Configured field names
                StorageContext[Entities.Role][RoleFields.Name],
                // Parameter names
                RoleFields.Name);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[RoleFields.Name].Value = roleName;

            DbDataReader reader = null;
            TRole role = default(TRole);

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                if (reader.Read())
                {
                    role = new TRole();
                    role.Id = (TKey)reader.GetSafeValue(
                        StorageContext[Entities.Role][RoleFields.Id]);
                    role.Name = reader.GetSafeString(
                        StorageContext[Entities.Role][RoleFields.Name]);
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

            return role;
        }

        /// <summary>
        /// Find a list of role names by user id.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <returns>Returns a list of roles if found; otherwise, returns empty list.</returns>
        public IList<string> FindRoleNamesByUserId(TKey userId)
        {
            DbCommand command = StorageContext.CreateCommand();
            command.CommandText = String.Format(
                @"SELECT {2} FROM {0} INNER JOIN {1} ON ({0}.{3} = {1}.{4}) WHERE {5} = @{6};",
                StorageContext[Entities.Role].TableName,
                StorageContext[Entities.UserRole].TableName,
                // Configured field names
                StorageContext[Entities.Role][RoleFields.Name],
                StorageContext[Entities.Role][RoleFields.Id],
                StorageContext[Entities.UserRole][UserRoleFields.RoleId],
                StorageContext[Entities.UserRole][UserRoleFields.UserId],
                // Parameter names
                UserRoleFields.UserId);

            DbCommandContext cmdContext = new DbCommandContext(command);
            cmdContext.Parameters[UserRoleFields.UserId].Value = userId;

            DbDataReader reader = null;
            List<string> list = new List<string>();
            string roleName = null;

            StorageContext.Open();

            try
            {
                reader = cmdContext.ExecuteReader();

                while (reader.Read())
                {
                    roleName = reader.GetSafeString(
                        StorageContext[Entities.Role][RoleFields.Name]);

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
    }

}
