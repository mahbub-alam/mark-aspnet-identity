// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Mark.AspNet.Identity.Common
{
    /// <summary>
    /// Represents transaction context interface for ADO.NET style storage context.
    /// </summary>
    public interface IDbTransactionContext : ITransactionContext
    {
        /// <summary>
        /// Get the underlying transaction associated with the current storage context.
        /// </summary>
        DbTransaction Transaction
        {
            get;
        }
    }
}
