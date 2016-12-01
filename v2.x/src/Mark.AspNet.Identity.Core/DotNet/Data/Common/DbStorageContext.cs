//
// Copyright 2016, Mahbub Alam (mahbub002@gmail.com)
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
using System.Data.Common;
using System.Configuration;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.DotNet.Data.Common
{
    /// <summary>
    /// Represents default ADO.NET style storage context.
    /// </summary>
    public class DbStorageContext : Disposable, IDbStorageContext 
    {
        private EntityConfigurationCollection _entityConfigs;
        private DbConnection _conn;
        private bool _isConnOpenAlready;
        private IDbTransactionContext _tContext;
        private List<IDbCommandContext> _cmdList;

        /// <summary>
        /// Initialize a new instance of the class which uses the "DefaultConnection" connection string name.
        /// </summary>
        public DbStorageContext() : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given connection string name.
        /// </summary>
        /// <param name="connectionStringName">Connection string name.</param>
        public DbStorageContext(string connectionStringName)
        {
            string connectionString = "";
            string providerName = "";

            connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ConfigurationErrorsException(
                    "'connectionString' attribute is missing or has invalid value " +
                    "in the database connection string entry");
            }

            if (String.IsNullOrWhiteSpace(providerName))
            {
                throw new ConfigurationErrorsException(
                    "'providerName' attribute is missing or has invalid value " +
                    "in the database connection string entry");
            }

            Init(connectionString, providerName);
        }

        /// <summary>
        /// Initialize a new instance of the class with the given connection string and provider name.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="providerName">Provider name.</param>
        public DbStorageContext(string connectionString, string providerName)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(
                    "'connectionString' parameter is null or has invalid value");
            }

            if (String.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentException(
                    "'providerName' parameter is null or has invalid value");
            }

            Init(connectionString, providerName);
        }

        private void Init(string connectionString, string providerName)
        {
            _entityConfigs = new EntityConfigurationCollection();
            _conn = CreateConnection(providerName);
            _conn.ConnectionString = connectionString;
            _cmdList = new List<IDbCommandContext>();

            OnConfiguringEntities(_entityConfigs);
        }

        private DbConnection CreateConnection(string providerName)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

            if (factory == null)
            {
                throw new NullReferenceException(
                    String.Format("Database provider with the name [{0}] not found in the system", providerName));
            }

            return factory.CreateConnection();
        }

        /// <summary>
        /// Get the underlying database connection.
        /// </summary>
        protected DbConnection Connection
        {
            get { return _conn; }
        }

        /// <summary>
        /// Open database connection if not opened yet.
        /// </summary>
        public void Open()
        {
            ThrowIfDisposed();

            if (_conn.State == System.Data.ConnectionState.Open)
            {
                _isConnOpenAlready = true;
            }
            else
            {
                _conn.Open();
                _isConnOpenAlready = false;
            }
        }

        /// <summary>
        /// Close database connection. If the connection was already opened before calling 
        /// <see cref="Open()"/> method, it is not closed unless closed forcibly.
        /// </summary>
        /// <param name="forceClose">Force closing the connection.</param>
        public void Close(bool forceClose = false)
        {
            ThrowIfDisposed();

            if (!_isConnOpenAlready || forceClose)
            {
                 _conn.Close();
            }
        }

        /// <summary>
        /// Create database command object.
        /// </summary>
        /// <returns>Returns command.</returns>
        public DbCommand CreateCommand()
        {
            ThrowIfDisposed();

            return _conn.CreateCommand();
        }

        /// <summary>
        /// Create a new transaction context.
        /// </summary>
        /// <param name="createPrivate">Whether to create private transaction context.</param>
        /// <returns>Returns a new transaction context.</returns>
        public IDbTransactionContext CreateTransactionContext(bool createPrivate = false)
        {
            ThrowIfDisposed();

            IDbTransactionContext transactionContext = NewTransactionContext();

            if (!createPrivate)
            {
                _tContext = transactionContext;
            }

            return transactionContext;
        }

        /// <summary>
        /// Create a new transaction context.
        /// </summary>
        /// <returns>Returns a new transaction context.</returns>
        protected virtual IDbTransactionContext NewTransactionContext()
        {
            Open();
            return new DbTransactionContext(_conn.BeginTransaction());
        }

        /// <summary>
        /// Whether there is a global transaction exists within the storage context.
        /// </summary>
        public bool TransactionExists
        {
            get
            {
                return _tContext != null;
            }
        }

        /// <summary>
        /// Get the current global transaction context associated with the storage context. If no 
        /// transaction context is found, a new one is created and returned.
        /// </summary>
        ITransactionContext IStorageContext.TransactionContext
        {
            get
            {
                return this.TransactionContext;
            }
        }

        /// <summary>
        /// Get the current global transaction context associated with the storage context. If no 
        /// transaction context is found, a new one is created and returned.
        /// </summary>
        public IDbTransactionContext TransactionContext
        {
            get
            {
                ThrowIfDisposed();

                if (!TransactionExists)
                {
                    _tContext = CreateTransactionContext();
                }

                return _tContext;
            }
        }

        /// <summary>
        /// Get specific entity configuration.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public EntityConfiguration<TEntity> GetEntityConfiguration<TEntity>() where TEntity : IEntity
        {
            ThrowIfDisposed();

            return _entityConfigs.Get<TEntity>();
        }

        /// <summary>
        /// Get specific entity configuration.
        /// </summary>
        /// <param name="entityType">Entity type.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public IEntityConfiguration<IEntity> GetEntityConfiguration(Type entityType)
        {
            ThrowIfDisposed();

            return _entityConfigs.Get(entityType);
        }

        /// <summary>
        /// Configure entities.
        /// </summary>
        /// <param name="entityConfigs">Passed entity configuration collection.</param>
        protected virtual void OnConfiguringEntities(EntityConfigurationCollection entityConfigs)
        {
        }

        /// <summary>
        /// Add a command for execution.
        /// </summary>
        /// <param name="commandContext">Command to be executed.</param>
        public void AddCommand(IDbCommandContext commandContext)
        {
            ThrowIfDisposed();

            if (commandContext != null)
            {
                _cmdList.Add(commandContext);
            }
        }

        /// <summary>
        /// Save all changes to a storage.
        /// </summary>
        /// <returns>Returns number of objects saved to the storage.</returns>
        public int SaveChanges()
        {
            ThrowIfDisposed();

            int retCount = 0;
            IDbTransactionContext privateTContext = null;

            Open();

            // MySQL does not support nested transaction as if this class is used with 
            // unit of work which wraps operation inside a transaction. So, checking for 
            // existing transaction before creating private transaction context.
            if (!TransactionExists)
            {
                privateTContext = this.CreateTransactionContext(true);
            }
            
            try
            {
                foreach (IDbCommandContext cmdContext in _cmdList)
                {
                    retCount += cmdContext.Execute();
                    cmdContext.Dispose();
                }

                if (privateTContext != null)
                {
                    privateTContext.Commit();
                }
            }
            catch (Exception)
            {
                if (privateTContext != null)
                {
                    privateTContext.Rollback();
                }

                throw;
            }
            finally
            {
                if (privateTContext != null)
                {
                    privateTContext.Dispose();
                }

                _cmdList.Clear();
                Close();
            }

            return retCount;
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            if (_tContext != null)
            {
                _tContext.Dispose();
            }

            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
            }
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _conn = null;
            _cmdList = null;
            _tContext = null;
            _entityConfigs = null;
        }
    }
}
