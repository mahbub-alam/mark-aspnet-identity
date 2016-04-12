// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents base entity mapping. 
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public abstract class EntityMap<TEntity> : EntityTypeConfiguration<TEntity> 
        where TEntity : class
    {
        EntityConfiguration _configuration;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        protected EntityMap(EntityConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("Configuration parameter null");
            }

            _configuration = configuration;
            ToTable(_configuration.TableName);
            MapPrimaryKey();
            MapFields();
            MapRelationships();
        }

        /// <summary>
        /// Get configuration.
        /// </summary>
        protected EntityConfiguration Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected virtual void MapPrimaryKey()
        {
        }

        /// <summary>
        /// Map all non-key fields.
        /// </summary>
        protected virtual void MapFields()
        {
        }

        /// <summary>
        /// Map relationship among entities.
        /// </summary>
        protected virtual void MapRelationships()
        {
        }
    }
}
