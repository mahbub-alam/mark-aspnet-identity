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
    /// Represents transaction context interface ADO.NET style storage context.
    /// </summary>
    public interface IDbTransactionContext : ITransactionContext
    {
        /// <summary>
        /// Get current connection transaction.
        /// </summary>
        DbTransaction Transaction
        {
            get;
        }
    }
}
