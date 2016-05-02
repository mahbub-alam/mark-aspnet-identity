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
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Mark.DotNet.Data.ModelConfiguration
{
    /// <summary>
    /// Represents entity configuration like table name, fields etc.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public abstract class EntityConfiguration<TEntity> : IEntityConfiguration<TEntity> where TEntity : IEntity
    {
        private string _tableName;
        private Dictionary<string, PropertyConfiguration> _propertyNameToConfigMaps;
        private List<PropertyConfiguration> _propertyConfigList;
        private List<PropertyConfiguration> _keyPropertyConfigList;
        private bool _allowPropertyConfigCreation;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        protected EntityConfiguration()
        {
            _tableName = "";
            _keyPropertyConfigList = new List<PropertyConfiguration>();
            _propertyConfigList = new List<PropertyConfiguration>();
            _propertyNameToConfigMaps = new Dictionary<string, PropertyConfiguration>();

            _allowPropertyConfigCreation = true;
            Configure();
            _allowPropertyConfigCreation = false;
        }

        /// <summary>
        /// Set table name associated with entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Returns entity configuration.</returns>
        public IEntityConfiguration<TEntity> ToTable(string tableName)
        {
            _tableName = tableName;

            return this;
        }

        /// <summary>
        /// Get table name.
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }

        /// <summary>
        /// Whether the entity has composite key.
        /// </summary>
        public bool HasCompositeKey
        {
            get { return _keyPropertyConfigList.Count > 1; }
        }

        /// <summary>
        /// Set entity key.
        /// </summary>
        /// <param name="propertyNames">A list of key property names.</param>
        /// <returns>Retruns entity configuration.</returns>
        public IEntityConfiguration<TEntity> HasKey(params string[] propertyNames)
        {
            if (propertyNames.Length == 0)
            {
                throw new ArgumentException("Property name(s)s is/are not passed for key");
            }

            PropertyConfiguration pc = null;

            // Composite key
            if(propertyNames.Length > 1)
            {
                for (int i = 0; i < propertyNames.Length; ++i)
                {
                    pc = GetPropertyConfigurationInternal(propertyNames[i]);

                    if (!pc.IsKey)
                    {
                        pc.AsKeyInternal(i);
                        _keyPropertyConfigList.Add(pc);
                    }
                }
            }
            else
            {
                pc = GetPropertyConfigurationInternal(propertyNames[0]);

                if (!pc.IsKey)
                {
                    pc.AsKeyInternal();
                    _keyPropertyConfigList.Add(pc);
                }
            }

            return this;
        }

        /// <summary>
        /// Set entity key.
        /// </summary>
        /// <typeparam name="TKey">Id type.</typeparam>
        /// <param name="expr">Expression to define entity key.</param>
        /// <returns>Returns entity configuration.</returns>
        public EntityConfiguration<TEntity> HasKey<TKey>(Expression<Func<TEntity, TKey>> expr) 
        {
            MemberExpression mExpr = expr.Body as MemberExpression;
            NewExpression newExpr = expr.Body as NewExpression;

            if (mExpr == null && newExpr == null)
            {
                throw new ArgumentException("Key should be an entity member property or an anonymous type " + 
                    "with multiple entity member properties for a composite key");
            }

            PropertyConfiguration pc = null;

            if (mExpr != null)
            {
                pc = GetPropertyConfigurationInternal(mExpr.Member);

                if (!pc.IsKey)
                {
                    pc.AsKeyInternal();
                    _keyPropertyConfigList.Add(pc);
                }
            }
            else
            {
                // Composite key
                for (int i = 0; i < newExpr.Members.Count; ++i)
                {
                    pc = GetPropertyConfigurationInternal(newExpr.Members[i]);

                    if (!pc.IsKey)
                    {
                        pc.AsKeyInternal(i);
                        _keyPropertyConfigList.Add(pc);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Add property for configuration if not yet added; otherwise, returns the saved
        /// property configuration.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Returns property configuration.</returns>
        public PropertyConfiguration Property(string propertyName)
        {
            return GetPropertyConfigurationInternal(propertyName);
        }

        /// <summary>
        /// Get property configuration of the given entity property. It returns configuration 
        /// if already saved. If not saved, it creates a new one only when this method is called 
        /// inside the <see cref="Configure()"/> method; otherwise, returns null.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="expr">Entity property expression.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public PropertyConfiguration Property<T>(Expression<Func<TEntity, T>> expr) where T : IEquatable<T>
        {
            return GetPropertyConfigurationInternal(expr);
        }

        /// <summary>
        /// Get property configuration of the given entity property. It returns configuration 
        /// if already saved. If not saved, it creates a new one only when this method is called 
        /// inside the <see cref="Configure()"/> method; otherwise, returns null.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="expr">Entity property expression.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public PropertyConfiguration Property<T>(Expression<Func<TEntity, T?>> expr) where T : struct
        {
            return GetPropertyConfigurationInternal(expr);
        }

        /// <summary>
        /// Get property configuration of the given entity property. It returns configuration 
        /// if already saved. If not saved, it creates a new one only when this method is called 
        /// inside the <see cref="Configure()"/> method; otherwise, returns null.
        /// </summary>
        /// <param name="expr">Entity property expression.</param>
        /// <returns>Returns configuration if found; otherwise, returns null.</returns>
        public PropertyConfiguration Property(Expression<Func<TEntity, string>> expr)
        {
            return GetPropertyConfigurationInternal(expr);
        }

        /// <summary>
        /// Get a list of key property configurations associated with the entity.
        /// </summary>
        public ReadOnlyCollection<PropertyConfiguration> KeyPropertyConfigurations
        {
            get { return _keyPropertyConfigList.AsReadOnly(); }
        }

        /// <summary>
        /// Get a list of property configurations associated with the entity.
        /// </summary>
        public ReadOnlyCollection<PropertyConfiguration> PropertyConfigurations
        {
            get { return _propertyConfigList.AsReadOnly(); }
        }

        /// <summary>
        /// Configure entity.
        /// </summary>
        protected abstract void Configure();


        private PropertyConfiguration GetSavedPropertyConfigurationInternal(string propertyName)
        {
            if (_propertyNameToConfigMaps.ContainsKey(propertyName))
            {
                return _propertyNameToConfigMaps[propertyName];
            }

            return null;
        }

        private PropertyConfiguration GetPropertyConfigurationInternal(PropertyInfo pi)
        {
            PropertyConfiguration pc = GetSavedPropertyConfigurationInternal(pi.Name);

            if (pc == null && _allowPropertyConfigCreation)
            {
                pc = new PropertyConfiguration(pi.Name, pi.PropertyType);
                _propertyNameToConfigMaps.Add(pc.PropertyName, pc);
                _propertyConfigList.Add(pc);
            }

            return pc;
        }

        private PropertyConfiguration GetPropertyConfigurationInternal(string propertyName)
        {
            PropertyInfo pi = typeof(TEntity).GetProperty(propertyName);
            return GetPropertyConfigurationInternal(pi);
        }

        private PropertyConfiguration GetPropertyConfigurationInternal(MemberInfo mInfo)
        {
            PropertyInfo pi = mInfo as PropertyInfo;
            return GetPropertyConfigurationInternal(pi);
        }

        private PropertyConfiguration GetPropertyConfigurationInternal<T>(Expression<Func<TEntity, T>> expr)
        {
            MemberExpression mExpr = expr.Body as MemberExpression;

            if (mExpr == null)
            {
                throw new ArgumentException("Property should be an entity member");
            }

            PropertyInfo pi = mExpr.Member as PropertyInfo;
            return GetPropertyConfigurationInternal(pi);
        }

    }
}
