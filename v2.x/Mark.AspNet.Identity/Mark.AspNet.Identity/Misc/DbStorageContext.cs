// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity.Common;
using System.Data.Common;
using System.Configuration;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents default ADO.NET style storage context.
    /// </summary>
    /// <typeparam name="TConnection">Database connection type.</typeparam>
    public class DbStorageContext<TConnection> : IDbStorageContext 
        where TConnection : DbConnection, new()
    {
        private string _connString;
        private Dictionary<string, EntityConfiguration> _entityConfigs;
        private DbConnection _conn;
        private IDbTransactionContext _tContext;
        private List<DbCommandContext<IEntity>> _cmdList;

        /// <summary>
        /// Initialize a new instance of the class which uses the "DefaultConnection" connectionString.
        /// </summary>
        public DbStorageContext()
        {
            Init(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        /// <summary>
        /// Initialize a new instance of the class with given connection string.
        /// </summary>
        /// <param name="connString">Connection string.</param>
        public DbStorageContext(string connString)
        {
            Init(connString);
        }

        private void Init(string connString)
        {
            _connString = connString;
            _entityConfigs = new Dictionary<string, EntityConfiguration>();
            _conn = new TConnection();
            _conn.ConnectionString = _connString;
            _cmdList = new List<DbCommandContext<IEntity>>();

            OnConfiguringEntities(_entityConfigs);
        }

        /// <summary>
        /// Get database connection.
        /// </summary>
        public DbConnection Connection
        {
            get { return _conn; }
        }

        /// <summary>
        /// Whether there is a transaction exists within the storage context.
        /// </summary>
        public bool TransactionExists
        {
            get
            {
                return _tContext != null;
            }
        }

        /// <summary>
        /// Get transaction context. If a context exists, it will be returned; otherwise, 
        /// a new one will be returned.
        /// </summary>
        ITransactionContext IStorageContext.TransactionContext
        {
            get
            {
                return this.TransactionContext;
            }
        }

        /// <summary>
        /// Get transaction context. If a context exists, it will be returned; otherwise, 
        /// a new one will be returned.
        /// </summary>
        public IDbTransactionContext TransactionContext
        {
            get
            {
                if (!TransactionExists)
                {
                    _tContext = CreateTransactionContext();
                }

                return _tContext;
            }
        }

        /// <summary>
        /// Create a new transaction context that is not saved as public transaction context.
        /// </summary>
        /// <returns>Returns a new transaction context.</returns>
        public IDbTransactionContext CreateTransactionContext()
        {
            return new DbTransactionContext(_conn.BeginTransaction());
        }

        /// <summary>
        /// Get specific entity configuration.
        /// </summary>
        /// <param name="entityIdentifier">Entity identifier.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public EntityConfiguration this[string entityIdentifier]
        {
            get
            {
                return GetEntityConfiguration(entityIdentifier);
            }
        }

        /// <summary>
        /// Get specific entity configuration.
        /// </summary>
        /// <param name="entityIdentifier">Entity identifier.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public EntityConfiguration GetEntityConfiguration(string entityIdentifier)
        {
            if (_entityConfigs.ContainsKey(entityIdentifier))
            {
                return _entityConfigs[entityIdentifier];
            }

            return null;
        }

        /// <summary>
        /// Configure entities.
        /// </summary>
        /// <param name="entityConfigs">Passed entity configuration collection.</param>
        protected virtual void OnConfiguringEntities(Dictionary<string, EntityConfiguration> entityConfigs)
        {
            entityConfigs.Add(Entities.Role, new RoleConfiguration());
            entityConfigs.Add(Entities.User, new UserConfiguration());
            entityConfigs.Add(Entities.UserLogin, new UserLoginConfiguration());
            entityConfigs.Add(Entities.UserRole, new UserRoleConfiguration());
            entityConfigs.Add(Entities.UserClaim, new UserClaimConfiguration());
        }

        /// <summary>
        /// Add a command for execution.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="commandContext">Command to be executed.</param>
        public void AddCommand<TEntity>(DbCommandContext<TEntity> commandContext) where TEntity : IEntity
        {
            if (commandContext != null)
            {
                _cmdList.Add(commandContext as DbCommandContext<IEntity>);
            }
        }

        /// <summary>
        /// Save all changes to a storage.
        /// </summary>
        /// <returns>Returns number of objects saved to the storage.</returns>
        public int SaveChanges()
        {
            int retCount = 0;

            if (_conn.State != System.Data.ConnectionState.Open)
            {
                _conn.Open();
            }

            // Using private transaction context because, it may conflict with context 
            // created for public context.
            IDbTransactionContext privateTContext = this.CreateTransactionContext();

            try
            {
                foreach (DbCommandContext<IEntity> cmdContext in _cmdList)
                {
                    retCount += cmdContext.Execute();
                    cmdContext.Dispose();
                }

                privateTContext.Commit();
                privateTContext.Dispose();
            }
            catch (Exception)
            {
                privateTContext.Rollback();
                throw;
            }
            finally
            {
                _conn.Close();
            }

            return retCount;
        }

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        /// <summary>
        /// Dispose managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _tContext.Dispose();
                    _conn.Close();
                    _conn.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _conn = null;
                _tContext = null;
                _connString = null;
                _entityConfigs = null;
                _disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        /// <summary>
        /// Finalizer.
        /// </summary>
        ~DbStorageContext()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Dispose managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
