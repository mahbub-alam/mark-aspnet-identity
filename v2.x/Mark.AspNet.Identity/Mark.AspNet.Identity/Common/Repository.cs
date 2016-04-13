// Writen by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.Common
{
    /// <summary>
    /// Represents base implementation for repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public abstract class Repository<TEntity> 
        : IRepository<TEntity>, IUnitOfWorkHandler where TEntity : IEntity
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
                throw new ArgumentNullException("UnitOfWork is null");
            }

            _unitOfWork = unitOfWork;
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
