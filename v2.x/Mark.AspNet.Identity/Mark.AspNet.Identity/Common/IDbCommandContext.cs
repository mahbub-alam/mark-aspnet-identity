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
    /// Represents command context interface for ADO.NET style storage context.
    /// </summary>
    public interface IDbCommandContext : IDisposable 
    {
        /// <summary>
        /// Set command parameters for each entity in the given entity collection.
        /// </summary>
        /// <param name="setAction">Action that will execute for each entity in the collection.</param>
        void SetParametersForEach<TEntity>(Action<IDbParameterCollection, TEntity> setAction) where TEntity : IEntity;

        /// <summary>
        /// Get the underlying command.
        /// </summary>
        DbCommand Command
        {
            get;
        }

        /// <summary>
        /// Get command parameter collection.
        /// </summary>
        IDbParameterCollection Parameters
        {
            get;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <returns>Returns the number of rows affected.</returns>
        int Execute();

        /// <summary>
        /// Execute the command and returns a data reader.
        /// </summary>
        /// <returns>Returns a data reader.</returns>
        DbDataReader ExecuteReader();
    }
}
