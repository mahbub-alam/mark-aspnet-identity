using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.Common
{
    /// <summary>
    /// Represents interface for a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get the reference of the storage context.
        /// </summary>
        IStorageContext StorageContext { get; }

        /// <summary>
        /// Register the given item to be added for saving to a persistent storage.
        /// </summary>
        /// <param name="item">Item to be registered as added.</param>
        /// <param name="handler">The handler that will do the saving. It will be called by 
        /// unit of work during saving the item.</param>
        void RegisterAdded(IEntity item, IUnitOfWorkHandler handler);

        /// <summary>
        /// Register the given item that was changed for saving to a persistent storage.
        /// </summary>
        /// <param name="item">Item to be register as changed.</param>
        /// <param name="handler">The handler that will do the saving. It will be called by 
        /// unit of work during saving the item.</param>
        void RegisterChanged(IEntity item, IUnitOfWorkHandler handler);

        /// <summary>
        /// Register the given item that is to be removed from a persistent storage.
        /// </summary>
        /// <param name="item">Item to be register as removed.</param>
        /// <param name="handler">The handler that will do the removing. It will be called by 
        /// unit of work during removing the item.</param>
        void RegisterRemoved(IEntity item, IUnitOfWorkHandler handler);

        /// <summary>
        /// Save all changes to the persistent storage.
        /// </summary>
        /// <returns>Returns number of objects saved to the storage.</returns>
        int SaveChanges();
    }
}
