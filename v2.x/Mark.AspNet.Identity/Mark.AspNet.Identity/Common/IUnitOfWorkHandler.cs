// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.Common
{
    /// <summary>
    /// Represents a handler where one of the methods depending on the registration type 
    /// will be called by the unit of work to handle the registered (added, changed or removed) 
    /// item for persistent concern.
    /// </summary>
    public interface IUnitOfWorkHandler
    {
        /// <summary>
        /// Save the item that is registered to be added to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        void SaveAddedItem(IEntity item);

        /// <summary>
        /// Save the item that is registered to be changed to a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        void SaveChangedItem(IEntity item);

        /// <summary>
        /// Save the item that is registered to be remvoed from a persistent storage.
        /// </summary>
        /// <param name="item">Entity item.</param>
        void SaveRemovedItem(IEntity item);
    }
}
