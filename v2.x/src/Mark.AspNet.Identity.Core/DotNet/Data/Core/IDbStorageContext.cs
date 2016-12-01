//
// Copyright 2016, Mahbub Alam (mahbub002@gmail.com)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.DotNet.Data
{
    /// <summary>
    /// Represents storage context with an ADO.NET connection type.
    /// </summary>
    public interface IDbStorageContext : IStorageContext
    {
        /// <summary>
        /// Get the current global transaction context associated with the storage context. If no 
        /// transaction context is found, a new one is created and returned.
        /// </summary>
        new IDbTransactionContext TransactionContext
        {
            get;
        }

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
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        EntityConfiguration<TEntity> GetEntityConfiguration<TEntity>() where TEntity : IEntity;

        /// <summary>
        /// Get specific entity configuration.
        /// </summary>
        /// <param name="entityType">Entity type.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        IEntityConfiguration<IEntity> GetEntityConfiguration(Type entityType);

        /// <summary>
        /// Add a command for execution.
        /// </summary>
        /// <param name="commandContext">Command to be executed.</param>
        void AddCommand(IDbCommandContext commandContext);

    }
}
