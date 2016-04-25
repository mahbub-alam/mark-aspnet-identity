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
using Mark.Data;
using Mark.Data.Common;
using Mark.Core;
using Mark.Data.ModelConfiguration;
using System.Data.Common;

namespace Mark.Data.Common
{
    /// <summary>
    /// Represents base class for database repository.
    /// </summary>
    /// <typeparam name="TConnection">Database connection type.</typeparam>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TQueryBuilder">SQL query builder type.</typeparam>
    public abstract class DbRepository<TConnection, TEntity, TQueryBuilder> : Repository<TEntity>
        where TEntity : IEntity, new()
        where TConnection : DbConnection, new() 
        where TQueryBuilder : DbQueryBuilder<TEntity>
    {
        private DbStorageContext<TConnection> _storageContext;
        private EntityConfiguration<TEntity> _configuration;
        private TQueryBuilder _queryBuilder;
        private DbCommandBuilder<TEntity> _cmdBuilder;
        private DbEntityBuilder<TEntity> _entityBuilder;

        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public DbRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _storageContext = this.UnitOfWork.StorageContext as DbStorageContext<TConnection>;

            if (_storageContext == null)
            {
                throw new InvalidCastException("Wrong storage context");
            }

            _configuration = _storageContext.GetEntityConfiguration<TEntity>();

            if (_configuration == null)
            {
                throw new NullReferenceException("Entity configuration null");
            }

            _queryBuilder = (TQueryBuilder)Activator.CreateInstance(typeof(TQueryBuilder), 
                new object[] { _configuration });
            _cmdBuilder = new DbCommandBuilder<TEntity>(_queryBuilder, _storageContext);
            _entityBuilder = new DbEntityBuilder<TEntity>(_configuration);
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            base.DisposeExtra();
            _storageContext = null;
            _configuration = null;
            _queryBuilder = null;
            _cmdBuilder = null;
            _entityBuilder = null;
        }

        /// <summary>
        /// Get database context.
        /// </summary>
        protected DbStorageContext<TConnection> StorageContext
        {
            get { return _storageContext; }
        }

        /// <summary>
        /// Get entity configuration.
        /// </summary>
        protected EntityConfiguration<TEntity> Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Get command builder.
        /// </summary>
        protected DbCommandBuilder<TEntity> CommandBuilder
        {
            get { return _cmdBuilder; }
        }

        /// <summary>
        /// Get entity builder.
        /// </summary>
        protected DbEntityBuilder<TEntity> EntityBuilder
        {
            get { return _entityBuilder; }
        }

        /// <summary>
        /// Get the underlying query builder.
        /// </summary>
        protected DbQueryBuilder<TEntity> QueryBuilder
        {
            get { return _queryBuilder; }
        }
    }

}
