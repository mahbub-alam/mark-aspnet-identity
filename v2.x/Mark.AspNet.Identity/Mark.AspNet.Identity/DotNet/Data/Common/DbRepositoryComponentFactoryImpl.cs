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

namespace Mark.DotNet.Data.Common
{
    /// <summary>
    /// Represents factory implementation for building component related to database repository.
    /// </summary>
    internal class DbRepositoryComponentFactoryImpl : DbRepositoryComponentFactory
    {
        /// <summary>
        /// Create database query builder.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <typeparam name="TQueryBuilder">Query builder type.</typeparam>
        /// <param name="storageContext">Database storage context.</param>
        /// <returns>Returns a query builder.</returns>
        public override DbQueryBuilder<TEntity> CreateQueryBuilder<TEntity, TQueryBuilder>(
            IDbStorageContext storageContext)
        {
            return (DbQueryBuilder<TEntity>)Activator.CreateInstance(typeof(TQueryBuilder),
                new object[] { storageContext.GetEntityConfiguration<TEntity>() });
        }

        /// <summary>
        /// Create database command builder.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="queryBuilder">Query builder.</param>
        /// <param name="storageContext">Database storage context.</param>
        /// <returns>Returns a command builder.</returns>
        public override DbCommandBuilder<TEntity> CreateCommandBuilder<TEntity>(
            DbQueryBuilder<TEntity> queryBuilder, IDbStorageContext storageContext)
        {
            return new DbCommandBuilder<TEntity>(queryBuilder, storageContext);
        }

        /// <summary>
        /// Create entity builder.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="storageContext">Database storage context.</param>
        /// <returns>Returns an entity builder.</returns>
        public override DbEntityBuilder<TEntity> CreateEntityBuilder<TEntity>(
            IDbStorageContext storageContext)
        {
            return new DbEntityBuilder<TEntity>(
                storageContext.GetEntityConfiguration<TEntity>());
        }
    }
}
