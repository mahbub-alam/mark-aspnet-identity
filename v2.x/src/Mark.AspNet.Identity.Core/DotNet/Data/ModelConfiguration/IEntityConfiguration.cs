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
using System.Reflection;
using System.Collections.ObjectModel;

namespace Mark.DotNet.Data.ModelConfiguration
{
    /// <summary>
    /// Represents entity configuration interface.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface IEntityConfiguration<out TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Set table name associated with entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Returns entity configuration.</returns>
        IEntityConfiguration<TEntity> ToTable(string tableName);

        /// <summary>
        /// Get table name.
        /// </summary>
        string TableName
        {
            get;
        }

        /// <summary>
        /// Whether the entity has composite key.
        /// </summary>
        bool HasCompositeKey
        {
            get;
        }

        /// <summary>
        /// Set entity key.
        /// </summary>
        /// <param name="propertyNames">A list of key property names.</param>
        /// <returns>Retruns entity configuration.</returns>
        IEntityConfiguration<TEntity> HasKey(params string[] propertyNames);

        /// <summary>
        /// Add property for configuration if not yet added; otherwise, returns the saved
        /// property configuration.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Returns property configuration.</returns>
        PropertyConfiguration Property(string propertyName);

        /// <summary>
        /// Get a list of key property configurations associated with the entity.
        /// </summary>
        ReadOnlyCollection<PropertyConfiguration> KeyPropertyConfigurations
        {
            get;
        }

        /// <summary>
        /// Get a list of property configurations associated with the entity.
        /// </summary>
        ReadOnlyCollection<PropertyConfiguration> PropertyConfigurations
        {
            get;
        }
    }
}
