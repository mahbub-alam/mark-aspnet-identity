//
// Copyright 2016, Mahbub Alam (mahbub002@ymail.com)
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

namespace Mark.Data
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
