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
        /// Get an instance of the transaction context for use with the storage context.
        /// </summary>
        ITransactionContext TransactionContext
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
