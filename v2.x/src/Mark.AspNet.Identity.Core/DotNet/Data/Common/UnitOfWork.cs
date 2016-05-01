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

namespace Mark.DotNet.Data.Common
{
    /// <summary>
    /// Represents unit of work.
    /// </summary>
    public sealed class UnitOfWork : Disposable, IUnitOfWork
    {
        private IStorageContext _storageContext;
        private List<Work> _workList;

        /// <summary>
        /// Initialize a new instance of the class with the storage context reference.
        /// </summary>
        /// <param name="storageContext">Storage context to be used.</param>
        public UnitOfWork(IStorageContext storageContext)
        {
            if (storageContext == null)
            {
                throw new ArgumentNullException("Storage context is null");
            }

            _storageContext = storageContext;

            _workList = new List<Work>();
        }

        /// <summary>
        /// Get the reference of the storage context.
        /// </summary>
        public IStorageContext StorageContext
        {
            get
            {
                ThrowIfDisposed();

                return _storageContext;
            }
        }

        /// <summary>
        /// Register the given item to be added for saving to a persistent storage.
        /// </summary>
        /// <param name="item">Item to be registered as added.</param>
        /// <param name="handler">The handler that will do the saving. It will be called by 
        /// unit of work during saving the item.</param>
        public void RegisterAdded(IEntity item, IUnitOfWorkHandler handler)
        {
            ThrowIfDisposed();

            _workList.Add(new Work(item, handler, WorkType.Added));
        }

        /// <summary>
        /// Register the given item that was changed for saving to a persistent storage.
        /// </summary>
        /// <param name="item">Item to be register as changed.</param>
        /// <param name="handler">The handler that will do the saving. It will be called by 
        /// unit of work during saving the item.</param>
        public void RegisterChanged(IEntity item, IUnitOfWorkHandler handler)
        {
            ThrowIfDisposed();

            _workList.Add(new Work(item, handler, WorkType.Changed));
        }

        /// <summary>
        /// Register the given item that is to be removed from a persistent storage.
        /// </summary>
        /// <param name="item">Item to be register as removed.</param>
        /// <param name="handler">The handler that will do the removing. It will be called by 
        /// unit of work during removing the item.</param>
        public void RegisterRemoved(IEntity item, IUnitOfWorkHandler handler)
        {
            ThrowIfDisposed();

            _workList.Add(new Work(item, handler, WorkType.Removed));
        }

        /// <summary>
        /// Save all changes to the storage.
        /// </summary>
        /// <returns>Returns number of objects saved to the storage.</returns>
        public int SaveChanges()
        {
            ThrowIfDisposed();

            int retCount = 0;
            ITransactionContext tContext = null;

            try
            {
                tContext = _storageContext.CreateTransactionContext();
            
                // Execute all unit of work handlers that act upon storage context
                foreach (Work work in _workList.OrderBy(w => w.EntryDateTime))
                {
                    work.Execute();
                }

                // Save all acts performed by unit of handlers.
                retCount = _storageContext.SaveChanges();

                tContext.Commit();
            }
            catch (Exception)
            {
                if (tContext != null)
                {
                    tContext.Rollback();
                }

                throw;
            }
            finally
            {
                if (tContext != null)
                {
                    tContext.Dispose();
                }

                // Must be cleared
                _workList.Clear();
            }

            return retCount;
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _storageContext = null;
            _workList = null;
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            _storageContext.Dispose();
        }

        #region Work type implementation

        /// <summary>
        /// Represents work type in a unit of work.
        /// </summary>
        private enum WorkType
        {
            /// <summary>
            /// Process entity as added.
            /// </summary>
            Added,
            /// <summary>
            /// Process entity as changed.
            /// </summary>
            Changed,
            /// <summary>
            /// Process entity as removed.
            /// </summary>
            Removed
        }

        /// <summary>
        /// Represents work in a unit of work.
        /// </summary>
        private sealed class Work
        {
            private IEntity _item;
            private IUnitOfWorkHandler _handler;
            private WorkType _workType;
            private DateTime _entryDateTime;

            /// <summary>
            /// Initialize a new instance of the class.
            /// </summary>
            /// <param name="item">Entity item.</param>
            /// <param name="handler">Work handler.</param>
            /// <param name="workType">Work type.</param>
            public Work(IEntity item, IUnitOfWorkHandler handler, WorkType workType)
            {
                _item = item;
                _handler = handler;
                _workType = workType;
                _entryDateTime = DateTime.UtcNow;
            }

            /// <summary>
            /// Get date and time of the work when entered.
            /// </summary>
            public DateTime EntryDateTime
            {
                get
                {
                    return _entryDateTime;
                }
            }

            /// <summary>
            /// Execute the work.
            /// </summary>
            public void Execute()
            {
                switch (_workType)
                {
                    case WorkType.Added:
                        _handler.SaveAddedItem(_item);
                        break;

                    case WorkType.Changed:
                        _handler.SaveChangedItem(_item);
                        break;

                    case WorkType.Removed:
                        _handler.SaveRemovedItem(_item);
                        break;
                }
            }
        }

        #endregion End of - Work type implementation
    }
}
