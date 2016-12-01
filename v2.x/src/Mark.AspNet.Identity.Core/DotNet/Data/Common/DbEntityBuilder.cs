//
// Copyright 2016, Mahbub Alam (mahbub002@gmail.com)
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
using Mark.DotNet.Data.ModelConfiguration;
using System.Data.Common;

namespace Mark.DotNet.Data.Common
{
    /// <summary>
    /// Represents database entity builder that builds entity from data reader using entity configuration.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public class DbEntityBuilder<TEntity> where TEntity : IEntity, new()
    {
        private EntityConfiguration<TEntity> _configuration;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public DbEntityBuilder(EntityConfiguration<TEntity> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("Entity configuration null");
            }

            _configuration = configuration;
        }

        private void SetValue(DbDataReader reader, TEntity entity, PropertyConfiguration pc)
        {
            object value = reader.GetSafeValue(pc.ColumnName, pc.PropertyType, pc.DefaultValue);
            entity.GetType().GetProperty(pc.PropertyName).SetValue(entity, value);
        }

        /// <summary>
        /// Get entity configuration.
        /// </summary>
        protected EntityConfiguration<TEntity> Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Build a list of entities.
        /// </summary>
        /// <param name="reader">Data reader from database command execute reader.</param>
        /// <returns>Returns a list of entities if found; otherwise, returns empty list.</returns>
        public virtual ICollection<TEntity> BuildAll(DbDataReader reader)
        {
            ICollection<TEntity> entityList = new List<TEntity>();
            TEntity entity = default(TEntity);

            while (reader.Read())
            {
                entity = new TEntity();

                foreach (PropertyConfiguration pc in _configuration.PropertyConfigurations)
                {
                    SetValue(reader, entity, pc);
                }

                entityList.Add(entity);
            }

            return entityList;
        }

        /// <summary>
        /// Build an entity.
        /// </summary>
        /// <param name="reader">Data reader from database command execute reader.</param>
        /// <returns>Return an entity if found; otherwise, returns null.</returns>
        public virtual TEntity Build(DbDataReader reader)
        {
            TEntity entity = default(TEntity);

            if (reader.Read())
            {
                entity = new TEntity();

                foreach (PropertyConfiguration pc in _configuration.PropertyConfigurations)
                {
                    SetValue(reader, entity, pc);
                }
            }

            return entity;
        }
    }
}
