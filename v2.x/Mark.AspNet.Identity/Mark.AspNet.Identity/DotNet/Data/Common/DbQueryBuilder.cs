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
using Mark.DotNet.Data.ModelConfiguration;
using System.Collections.ObjectModel;

namespace Mark.DotNet.Data.Common
{
    /// <summary>
    /// Represents SQL builder that generates SQL from entity configuration.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public abstract class DbQueryBuilder<TEntity> where TEntity : IEntity
    {
        private EntityConfiguration<TEntity> _configuration;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        protected DbQueryBuilder(EntityConfiguration<TEntity> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("Entity configuration null");
            }

            _configuration = configuration;
        }

        /// <summary>
        /// Get entity configuration.
        /// </summary>
        protected EntityConfiguration<TEntity> Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Get the quoted identifier of the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier to be quoted.</param>
        /// <returns>Returns quoted identifier.</returns>
        public abstract string GetQuotedIdentifier(string identifier);

        /// <summary>
        /// Get insert SQL query. 
        /// </summary>
        /// <returns>Returns sql query.</returns>
        public virtual string GetInsertSql()
        {
            StringBuilder sql = new StringBuilder("INSERT INTO ");
            StringBuilder colNames = new StringBuilder("(");
            StringBuilder values = new StringBuilder("VALUES (");

            ReadOnlyCollection<PropertyConfiguration> propConfigs = _configuration.PropertyConfigurations;
            PropertyConfiguration pc = null;

            sql.Append(GetQuotedIdentifier(_configuration.TableName) + " ");

            for(int i = 0; i < propConfigs.Count; ++i)
            {
                pc = propConfigs[i];

                if (pc.IsKey && !_configuration.HasCompositeKey)
                {
                    continue;
                }

                if (i != propConfigs.Count - 1)
                {
                    colNames.Append(GetQuotedIdentifier(pc.ColumnName) + ", ");
                    values.Append("@" + pc.PropertyName + ", ");
                }
                else
                {
                    colNames.Append(GetQuotedIdentifier(pc.ColumnName) + ") ");
                    values.Append("@" + pc.PropertyName + ");");
                }
            }

            sql.Append(colNames);
            sql.Append(values);

            return sql.ToString();
        }

        /// <summary>
        /// Get update SQL query. 
        /// </summary>
        /// <returns>Returns sql query.</returns>
        public virtual string GetUpdateSql()
        {
            StringBuilder sql = new StringBuilder("UPDATE ");
            StringBuilder setClause = new StringBuilder("SET ");
            StringBuilder whereClause = new StringBuilder("WHERE ");

            ReadOnlyCollection<PropertyConfiguration> propConfigs = _configuration.PropertyConfigurations;
            ReadOnlyCollection<PropertyConfiguration> keyPropConfigs = _configuration.KeyPropertyConfigurations;
            PropertyConfiguration cfg = null;

            sql.Append(GetQuotedIdentifier(_configuration.TableName) + " ");

            for (int i = 0; i < propConfigs.Count; ++i)
            {
                cfg = propConfigs[i];

                if (cfg.IsKey)
                {
                    continue;
                }

                if (i != propConfigs.Count - 1)
                {
                    setClause.Append(GetQuotedIdentifier(cfg.ColumnName) + " = " + "@" + cfg.PropertyName + ", ");
                }
                else
                {
                    setClause.Append(GetQuotedIdentifier(cfg.ColumnName) + " = " + "@" + cfg.PropertyName + " ");
                }
            }

            for (int i = 0; i < keyPropConfigs.Count; ++i)
            {
                cfg = keyPropConfigs[i];

                if (i != keyPropConfigs.Count - 1)
                {
                    whereClause.Append(GetQuotedIdentifier(cfg.ColumnName) + " = " + "@" + cfg.PropertyName + " AND ");
                }
                else
                {
                    whereClause.Append(GetQuotedIdentifier(cfg.ColumnName) + " = " + "@" + cfg.PropertyName + ";");
                }
            }

            sql.Append(setClause);
            sql.Append(whereClause);

            return sql.ToString();
        }

        /// <summary>
        /// Get delete SQL query. 
        /// </summary>
        /// <returns>Returns sql query.</returns>
        public virtual string GetDeleteSql()
        {
            StringBuilder sql = new StringBuilder("DELETE FROM ");
            StringBuilder whereClause = new StringBuilder("WHERE ");

            ReadOnlyCollection<PropertyConfiguration> keyPropConfigs = _configuration.KeyPropertyConfigurations;
            PropertyConfiguration cfg = null;

            sql.Append(GetQuotedIdentifier(_configuration.TableName) + " ");

            for (int i = 0; i < keyPropConfigs.Count; ++i)
            {
                cfg = keyPropConfigs[i];

                if (i != keyPropConfigs.Count - 1)
                {
                    whereClause.Append(GetQuotedIdentifier(cfg.ColumnName) + " = " + "@" + cfg.PropertyName + " AND ");
                }
                else
                {
                    whereClause.Append(GetQuotedIdentifier(cfg.ColumnName) + " = " + "@" + cfg.PropertyName + ";");
                }
            }

            sql.Append(whereClause);

            return sql.ToString();
        }
    }
}
