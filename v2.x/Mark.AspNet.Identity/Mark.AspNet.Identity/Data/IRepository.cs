// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.Data
{
    /// <summary>
    /// Represents base repository interface.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface IRepository<TEntity> 
        where TEntity : IEntity
    {
        /// <summary>
        /// Add entity to the repository.
        /// </summary>
        /// <param name="item">entity to be added.</param>
        void Add(TEntity item);

        /// <summary>
        /// Remove entity from the repository.
        /// </summary>
        /// <param name="item">entity to be removed.</param>
        void Remove(TEntity item);

        /// <summary>
        /// Update change of entity to the repository.
        /// </summary>
        /// <param name="item">entity to be updated.</param>
        void Change(TEntity item);
    }
}
