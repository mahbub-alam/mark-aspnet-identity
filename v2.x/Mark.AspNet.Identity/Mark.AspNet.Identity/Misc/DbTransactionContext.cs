// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity.Common;
using System.Data.Common;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents transaction context for ADO.NET style connection.
    /// </summary>
    public class DbTransactionContext : IDbTransactionContext
    {
        DbTransaction _transaction;

        /// <summary>
        /// Initialize a new instance of the class. 
        /// </summary>
        /// <param name="transaction">ADO.NET transaction type.</param>
        public DbTransactionContext(DbTransaction transaction)
        {
            _transaction = transaction;
        }

        /// <summary>
        /// Get transaction associated with the current connection.
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
        }

        /// <summary>
        /// Commit changes.
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// Rollback changes.
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
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
                    _transaction.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _transaction = null;
                _disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        /// <summary>
        /// Finalizer.
        /// </summary>
        ~DbTransactionContext()
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
