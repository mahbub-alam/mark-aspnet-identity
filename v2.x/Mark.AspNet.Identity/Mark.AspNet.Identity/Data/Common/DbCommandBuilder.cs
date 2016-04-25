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
using Mark.Data.ModelConfiguration;
using System.Data.Common;
using Mark.Core;

namespace Mark.Data.Common
{
    /// <summary>
    /// Represents database command builder for CRUD operation.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public class DbCommandBuilder<TEntity> where TEntity : IEntity
    {
        IDbStorageContext _storageContext;
        DbQueryBuilder<TEntity> _queryBuilder;
        EntityConfiguration<TEntity> _configuration;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="queryBuilder">Query builder.</param>
        /// <param name="storageContext">Storage context.</param>
        public DbCommandBuilder(DbQueryBuilder<TEntity> queryBuilder, IDbStorageContext storageContext)
        {
            _storageContext = storageContext;
            _configuration = _storageContext.GetEntityConfiguration<TEntity>();
            _queryBuilder = queryBuilder;
        }

        /// <summary>
        /// Get the underlying query builder.
        /// </summary>
        public DbQueryBuilder<TEntity> QueryBuilder
        {
            get { return _queryBuilder; }
        }

        private object GetPropertyValue(TEntity entity, PropertyConfiguration pc)
        {
            object value = entity.GetType().GetProperty(pc.PropertyName).GetValue(entity, null);

            // If property is nullable for class and nullable struct type
            if (pc.IsNullable)
            {
                if (value != null)
                {
                    // For nullable struct type with default value
                    object defaultValue = value.GetType().GetDefault();

                    if (value.Equals(defaultValue))
                    {
                        value = DBNull.Value;
                    }
                }
                else
                {
                    value = DBNull.Value;
                }
            }

            return value;
        }

        /// <summary>
        /// Get insert command.
        /// </summary>
        /// <param name="entities">A collection of entities.</param>
        /// <returns>Returns command.</returns>
        public DbCommandContext GetInsertCommand(ICollection<TEntity> entities)
        {
            DbCommand command = _storageContext.CreateCommand();
            command.CommandText = _queryBuilder.GetInsertSql();

            if (_storageContext.TransactionExists)
            {
                command.Transaction = _storageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command, entities.Cast<IEntity>().ToList());

            cmdContext.SetParametersForEach<TEntity>((parameters, entity) =>
            {
                foreach (PropertyConfiguration pc in _configuration.PropertyConfigurations)
                {
                    if (pc.IsKey && !_configuration.HasCompositeKey)
                    {
                        continue;
                    }

                    parameters[pc.PropertyName].Value = GetPropertyValue(entity, pc);
                }
            });

            return cmdContext;
        }

        /// <summary>
        /// Get update command.
        /// </summary>
        /// <param name="entities">A collection of entities.</param>
        /// <returns>Returns command.</returns>
        public DbCommandContext GetUpdateCommand(ICollection<TEntity> entities)
        {
            DbCommand command = _storageContext.CreateCommand();
            command.CommandText = _queryBuilder.GetUpdateSql();

            if (_storageContext.TransactionExists)
            {
                command.Transaction = _storageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command, entities.Cast<IEntity>().ToList());

            cmdContext.SetParametersForEach<TEntity>((parameters, entity) =>
            {
                foreach (PropertyConfiguration pc in _configuration.PropertyConfigurations)
                {
                    parameters[pc.PropertyName].Value = GetPropertyValue(entity, pc);
                }
            });

            return cmdContext;
        }

        /// <summary>
        /// Get delete command.
        /// </summary>
        /// <param name="entities">A collection of entities.</param>
        /// <returns>Returns command.</returns>
        public DbCommandContext GetDeleteCommand(ICollection<TEntity> entities)
        {
            DbCommand command = _storageContext.CreateCommand();
            command.CommandText = _queryBuilder.GetDeleteSql();

            if (_storageContext.TransactionExists)
            {
                command.Transaction = _storageContext.TransactionContext.Transaction;
            }

            DbCommandContext cmdContext = new DbCommandContext(command, entities.Cast<IEntity>().ToList());

            cmdContext.SetParametersForEach<TEntity>((parameters, entity) =>
            {
                foreach (PropertyConfiguration pc in _configuration.KeyPropertyConfigurations)
                {
                    parameters[pc.PropertyName].Value = GetPropertyValue(entity, pc);
                }
            });

            return cmdContext;
        }
    }
}
