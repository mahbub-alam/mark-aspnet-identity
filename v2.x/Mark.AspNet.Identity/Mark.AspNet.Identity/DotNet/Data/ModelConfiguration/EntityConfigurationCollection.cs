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

namespace Mark.DotNet.Data.ModelConfiguration
{
    /// <summary>
    /// Represents a collection of entity configuration.
    /// </summary>
    public class EntityConfigurationCollection
    {
        private Dictionary<string, IEntityConfiguration<IEntity>> _collection;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public EntityConfigurationCollection()
        {
            _collection = new Dictionary<string, IEntityConfiguration<IEntity>>();
        }

        /// <summary>
        /// Add entity configuration to the collection.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="configuration">Entity configuration to be added.</param>
        /// <returns>Returns entity configuration collection for chaining.</returns>
        public EntityConfigurationCollection Add<TEntity>(EntityConfiguration<TEntity> configuration) 
            where TEntity : IEntity
        {
            string typeKey = typeof(TEntity).Name;
            IEntityConfiguration<IEntity> cfg = configuration as IEntityConfiguration<IEntity>;

            if (_collection.ContainsKey(typeKey))
            {
                _collection[typeKey] = cfg;
            }
            else
            {
                _collection.Add(typeKey, cfg);
            }

            return this;
        }

        /// <summary>
        /// Get entity configuration from the collection.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public EntityConfiguration<TEntity> Get<TEntity>() where TEntity : IEntity
        {
            string typeKey = typeof(TEntity).Name;
            EntityConfiguration<TEntity> entityConfig = null;

            if (_collection.ContainsKey(typeKey))
            {
                entityConfig = _collection[typeKey] as EntityConfiguration<TEntity>;

                if (entityConfig == null)
                {
                    throw new InvalidOperationException("Invalid entity type for retrieving entity configuration");
                }
            }

            return entityConfig;
        }

        /// <summary>
        /// Get entity configuration from the collection.
        /// </summary>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public IEntityConfiguration<IEntity> Get(Type entityType)
        {
            string typeKey = entityType.Name;
            IEntityConfiguration<IEntity> entityConfig = null;

            if (_collection.ContainsKey(typeKey))
            {
                entityConfig = _collection[typeKey] as IEntityConfiguration<IEntity>;

                if (entityConfig == null)
                {
                    throw new InvalidOperationException("Invalid entity type for retrieving entity configuration");
                }
            }

            return entityConfig;
        }

        /// <summary>
        /// Determine whether the collection contains the specified entity configuration.
        /// </summary>
        /// <returns>Returns true if found; otherwise, returns false.</returns>
        public bool ContainsConfiguration<TEntity>()
        {
            string typeKey = typeof(TEntity).Name;
            return _collection.ContainsKey(typeKey);
        }
    }
}
