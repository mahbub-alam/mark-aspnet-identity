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
using System.Data.Common;

namespace Mark.DotNet.Data.Common
{
    /// <summary>
    /// Represents transaction context for ADO.NET style storage context.
    /// </summary>
    public class DbTransactionContext : Disposable, IDbTransactionContext
    {
        DbTransaction _transaction;

        /// <summary>
        /// Initialize a new instance of the class. 
        /// </summary>
        /// <param name="transaction">ADO.NET transaction type.</param>
        public DbTransactionContext(DbTransaction transaction)
        {
            _transaction = transaction;
        }

        /// <summary>
        /// Get the underlying transaction associated with the current storage context.
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
        }

        /// <summary>
        /// Commit changes.
        /// </summary>
        public void Commit()
        {
            ThrowIfDisposed();

            _transaction.Commit();
        }

        /// <summary>
        /// Rollback changes.
        /// </summary>
        public void Rollback()
        {
            ThrowIfDisposed();

            _transaction.Rollback();
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            _transaction.Dispose();
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _transaction = null;
        }
    }
}
