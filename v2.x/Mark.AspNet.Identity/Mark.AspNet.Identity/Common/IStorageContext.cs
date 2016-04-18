// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.Common
{
    /// <summary>
    /// Represents storage context.
    /// </summary>
    public interface IStorageContext : IDisposable
    {
        /// <summary>
        /// Create a new transaction context.
        /// </summary>
        /// <param name="createPrivate">Whether to create private transaction context.</param>
        /// <returns>Returns a new transaction context.</returns>
        IDbTransactionContext CreateTransactionContext(bool createPrivate = false);

        /// <summary>
        /// Get the current global transaction context associated with the storage context. If no 
        /// transaction context is found, a new one is created and returned.
        /// </summary>
        ITransactionContext TransactionContext
        {
            get;
        }

        /// <summary>
        /// Whether there is a transaction exists within the storage context.
        /// </summary>
        bool TransactionExists
        {
            get;
        }

        /// <summary>
        /// Save all changes to a storage.
        /// </summary>
        /// <returns>Returns number of objects saved to the storage.</returns>
        int SaveChanges();
    }
}
