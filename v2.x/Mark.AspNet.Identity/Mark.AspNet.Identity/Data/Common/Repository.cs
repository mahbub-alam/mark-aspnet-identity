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
using Mark.Core;

namespace Mark.Data.Common
{
    /// <summary>
    /// Represents base implementation for repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public abstract class Repository<TEntity> 
        : Disposable, IRepository<TEntity>, IUnitOfWorkHandler where TEntity : IEntity
    {
        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        protected Repository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("'unitOfWork' parameter null");
            }

            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            // Nothing
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _unitOfWork = null;
        }

        /// <summary>
        /// Get the unit of work reference.
        /// </summary>
        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        /// <summary>
        /// Set unit of work reference to be used by the repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference.</param>
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Add the entity to the repository.
        /// </summary>
        /// <param name="item">entity to be added.</param>
        public void Add(TEntity item)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.RegisterAdded(item, this);
            }
        }

        /// <summary>
        /// Remove the entity from the repository.
        /// </summary>
        /// <param name="item">entity to be removed.</param>
        public void Remove(TEntity item)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.RegisterRemoved(item, this);
            }
        }

        /// <summary>
        /// Update change of entity to the repository.
        /// </summary>
        /// <param name="item">entity to be updated.</param>
        public void Change(TEntity item)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.RegisterChanged(item, this);
            }
        }

        #region IUnitOfWorkHandler interface members implementation

        /// <summary>
        /// Save the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        public virtual void SaveAddedItem(IEntity item)
        {
            SaveAddedItem((TEntity)item);
        }

        /// <summary>
        /// Save the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        public virtual void SaveChangedItem(IEntity item)
        {
            SaveChangedItem((TEntity)item);
        }

        /// <summary>
        /// Save the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        public virtual void SaveRemovedItem(IEntity item)
        {
            SaveRemovedItem((TEntity)item);
        }

        #endregion IUnitOfWorkHandler interface members implementation

        /// <summary>
        /// Save adding of the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected abstract void SaveAddedItem(TEntity item);

        /// <summary>
        /// Save changes in the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected abstract void SaveChangedItem(TEntity item);

        /// <summary>
        /// Save the removing of the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        protected abstract void SaveRemovedItem(TEntity item);
    }
}
