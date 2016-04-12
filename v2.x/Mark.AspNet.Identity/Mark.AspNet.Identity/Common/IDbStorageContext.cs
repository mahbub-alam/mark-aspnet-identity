// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.Common
{
    /// <summary>
    /// Represents storage context with an ADO.NET connection type.
    /// </summary>
    public interface IDbStorageContext : IStorageContext
    {
        /// <summary>
        /// Get database connection.
        /// </summary>
        DbConnection Connection
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
        /// Create a new transaction context that is not saved as public transaction context.
        /// </summary>
        /// <returns>Returns a new transaction context.</returns>
        IDbTransactionContext CreateTransactionContext();

        /// <summary>
        /// Get specific entity configuration.
        /// </summary>
        /// <param name="entityIdentifier">Entity identifier.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        EntityConfiguration this[string entityIdentifier]
        {
            get;
        }

        /// <summary>
        /// Get specific entity configuration.
        /// </summary>
        /// <param name="entityIdentifier">Entity identifier.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        EntityConfiguration GetEntityConfiguration(string entityIdentifier);

        /// <summary>
        /// Add a command for execution.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="commandContext">Command to be executed.</param>
        void AddCommand<TEntity>(DbCommandContext<TEntity> commandContext) where TEntity : IEntity;
    }
}
