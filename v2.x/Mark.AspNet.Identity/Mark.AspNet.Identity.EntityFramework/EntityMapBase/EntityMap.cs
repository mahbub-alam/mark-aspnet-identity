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
using System.Data.Entity.ModelConfiguration;
using Mark.Data.ModelConfiguration;
using Mark.Data;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents base entity mapping. 
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public abstract class EntityMap<TEntity> : EntityTypeConfiguration<TEntity> 
        where TEntity : class, IEntity
    {
        EntityConfiguration<TEntity> _configuration;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        protected EntityMap(EntityConfiguration<TEntity> configuration)
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
        protected EntityConfiguration<TEntity> Configuration
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
