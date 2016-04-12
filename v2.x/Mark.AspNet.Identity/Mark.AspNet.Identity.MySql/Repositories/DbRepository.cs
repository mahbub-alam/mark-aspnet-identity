// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.AspNet.Identity.Common;
using Mark.AspNet.Identity;
using MySql.Data.MySqlClient;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents base class for database repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    internal abstract class DbRepository<TEntity> : Repository<TEntity> 
        where TEntity : IEntity
    {
        private DbStorageContext<MySqlConnection> _dbContext;

        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public DbRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = this.UnitOfWork.StorageContext as DbStorageContext<MySqlConnection>;

            if (_dbContext == null)
            {
                throw new InvalidCastException("Wrong storage context");
            }
        }

        /// <summary>
        /// Get database context.
        /// </summary>
        protected DbStorageContext<MySqlConnection> DbContext
        {
            get { return _dbContext; }
        }
    }
}
