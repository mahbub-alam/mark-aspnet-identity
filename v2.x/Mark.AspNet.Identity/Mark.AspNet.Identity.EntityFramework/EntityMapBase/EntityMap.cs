// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents entity map of the EntityTypeConfiguration type. 
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public abstract class EntityMap<T> : EntityTypeConfiguration<T> where T : class
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        protected EntityMap(string tableName)
        {
            ToTable(tableName);
            MapPrimaryKey();
            MapFields();
            MapRelationships();
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
