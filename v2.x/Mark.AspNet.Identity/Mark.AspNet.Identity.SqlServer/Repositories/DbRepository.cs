// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Data;
using Mark.Data.Common;
using Mark.AspNet.Identity;
using System.Data.SqlClient;

namespace Mark.AspNet.Identity.SqlServer
{
    /// <summary>
    /// Represents base class for database repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    internal abstract class DbRepository<TEntity> : Repository<TEntity> 
        where TEntity : IEntity
    {
        private DbStorageContext<SqlConnection> _storageContext;

        /// <summary>
        /// Initialize a new instance of the class with the unit of work reference.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference to be used.</param>
        public DbRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _storageContext = this.UnitOfWork.StorageContext as DbStorageContext<SqlConnection>;

            if (_storageContext == null)
            {
                throw new InvalidCastException("Wrong storage context");
            }
        }

        /// <summary>
        /// Get database context.
        /// </summary>
        protected DbStorageContext<SqlConnection> StorageContext
        {
            get { return _storageContext; }
        }
    }
}
