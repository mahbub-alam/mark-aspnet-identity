﻿// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity.Common;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents unit of work.
    /// </summary>
    public class UnitOfWork : Disposable, IUnitOfWork
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
                throw new ArgumentNullException("Persistent context is null");
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
            _workList.Add(new Work(item, handler, WorkType.Removed));
        }

        /// <summary>
        /// Save all changes to the storage.
        /// </summary>
        /// <returns>Returns number of objects saved to the storage.</returns>
        public int SaveChanges()
        {
            int retCount = 0;

            using (ITransactionContext tContext = _storageContext.TransactionContext)
            {
                foreach (Work work in _workList.OrderBy(w => w.EntryDate))
                {
                    work.Execute();
                }

                retCount = _storageContext.SaveChanges();

                tContext.Commit();
            }

            _workList.Clear();

            return retCount;
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields (managed/unmanaged) to null. This method 
        /// will be called whether the <see cref="Disposable.Dispose()"/> method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _storageContext = null;
            _workList = null;
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside <see cref="DisposeExtra()"/> method since, 
        /// that method will be called whether the <see cref="Disposable.Dispose()"/> method is called by the finalizer or 
        /// your code.
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
            Added,
            Changed,
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
            private DateTime _entryDate;

            /// <summary>
            /// Initialize a new instance of the class.
            /// </summary>
            /// <param name="item">Entity item to be processed.</param>
            /// <param name="handler">Work handler.</param>
            /// <param name="workType">Work type.</param>
            public Work(IEntity item, IUnitOfWorkHandler handler, WorkType workType)
            {
                _item = item;
                _handler = handler;
                _workType = workType;
                _entryDate = DateTime.UtcNow;
            }

            /// <summary>
            /// Get date and time of the work entry.
            /// </summary>
            public DateTime EntryDate
            {
                get
                {
                    return _entryDate;
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
