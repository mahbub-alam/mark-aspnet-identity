// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.Data
{
    /// <summary>
    /// Represents transaction context for storage context.
    /// </summary>
    public interface ITransactionContext : IDisposable
    {
        /// <summary>
        /// Commit transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        void Rollback();
    }
}
