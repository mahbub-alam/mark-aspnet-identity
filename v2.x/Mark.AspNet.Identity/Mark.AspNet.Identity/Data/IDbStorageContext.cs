// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.Data
{
    /// <summary>
    /// Represents storage context with an ADO.NET connection type.
    /// </summary>
    public interface IDbStorageContext : IStorageContext
    {
        /// <summary>
        /// Open database connection if it is not opened yet.
        /// </summary>
        void Open();

        /// <summary>
        /// Close database connection. If the connection was already opened before calling 
        /// <see cref="Open()"/> method, it is not closed unless closed forcibly.
        /// </summary>
        /// <param name="forceClose">Force closing the connection.</param>
        void Close(bool forceClose = false);

        /// <summary>
        /// Create database command object.
        /// </summary>
        /// <returns>Returns the object.</returns>
        DbCommand CreateCommand();

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
        /// <param name="commandContext">Command to be executed.</param>
        void AddCommand(IDbCommandContext commandContext);
    }
}
