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

namespace Mark.DotNet.Data.ModelConfiguration
{
    /// <summary>
    /// Represents property configuration for entity configuration.
    /// </summary>
    public sealed class PropertyConfiguration
    {
        private string _propertyName;
        private string _columnName;
        private Type _propertyType;
        private bool _isNullable;
        private bool _isKey;
        private bool _isIntegerKey;
        private int _keyColumnOrder;
        private object _defaultValue;
        private Func<object> _generateDefaultValue;

        /// <summary>
        /// Column order for single key.
        /// </summary>
        public const int SingleKeyColumnOrder = -1;

        /// <summary>
        /// Initialize a new instance of the class with given parameters.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Property type.</param>
        public PropertyConfiguration(string propertyName, Type propertyType)
        {
            _propertyName = propertyName;
            _columnName = propertyName;
            _isKey = false;
            _keyColumnOrder = SingleKeyColumnOrder;
            ParsePropertyType(propertyType);
        }

        /// <summary>
        /// Initialize a new instance of the class with given parameters.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Property type.</param>
        /// <param name="isKey">Whether the property is a key.</param>
        /// <param name="keyColumnOrder">Zero-based column order if it is a part of a composite key.</param>
        public PropertyConfiguration(string propertyName, Type propertyType, 
            bool isKey, int keyColumnOrder = SingleKeyColumnOrder) : this(propertyName, propertyType)
        {
            if (isKey)
            {
                _keyColumnOrder = keyColumnOrder;
                AsKeyInternal(keyColumnOrder);
            }
        }

        private void ParsePropertyType(Type type)
        {
            _propertyType = type;
            _defaultValue = type.GetDefault();

            if (type == typeof(string))
            {
                _isNullable = true;
            }
            else if (type.IsValueType)
            {
                _isNullable = Nullable.GetUnderlyingType(type) != null;
            }
            else
            {
                throw new ArgumentException("Property type must be primitive, nullable primitive or string");
            }
        }

        /// <summary>
        /// Set column name to be mapped with the property.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>Returns property configuration.</returns>
        public PropertyConfiguration HasColumnName(string columnName)
        {
            _columnName = columnName;
            return this;
        }

        /// <summary>
        /// Set default value for the property.
        /// </summary>
        /// <param name="value">Default value.</param>
        /// <returns>Returns property configuration.</returns>
        public PropertyConfiguration HasDefaultValue<T>(T value)
        {
            _defaultValue = value;
            return this;
        }

        /// <summary>
        /// Set a delegate for generating default value for the property.
        /// </summary>
        /// <param name="valueGenerator">Default value generator.</param>
        /// <returns>Returns property configuration.</returns>
        public PropertyConfiguration HasDefaultValue<T>(Func<T> valueGenerator)
        {
            _generateDefaultValue = () => valueGenerator;
            return this;
        }

        /// <summary>
        /// Set the property as a key.
        /// </summary>
        /// <param name="columnOrder">Zero based column order in a composite key.</param>
        /// <returns>Returns property configuration.</returns>
        internal PropertyConfiguration AsKeyInternal(int columnOrder = SingleKeyColumnOrder)
        {
            _isKey = true;
            _keyColumnOrder = columnOrder;
            _isIntegerKey = (_propertyType == typeof(int) ||
                _propertyType == typeof(uint) ||
                _propertyType == typeof(long) ||
                _propertyType == typeof(ulong) ||
                _propertyType == typeof(short) ||
                _propertyType == typeof(ushort) ||
                _propertyType == typeof(sbyte) ||
                _propertyType == typeof(byte));

            return this;
        }

        /// <summary>
        /// Get column name mapped with the proeprty.
        /// </summary>
        public string ColumnName
        {
            get { return _columnName; }
        }

        /// <summary>
        /// Get default value for the property.
        /// </summary>
        public object DefaultValue
        {
            get
            {
                if (_generateDefaultValue != null)
                {
                    return _generateDefaultValue();
                }
                else
                {
                    return _defaultValue;
                }
            }
        }

        /// <summary>
        /// Get property name.
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Get property type.
        /// </summary>
        public Type PropertyType
        {
            get { return _propertyType; }
        }

        /// <summary>
        /// Whether the property type is nullable.
        /// </summary>
        public bool IsNullable
        {
            get { return _isNullable; }
        }

        /// <summary>
        /// Whether the property is a key.
        /// </summary>
        public bool IsKey
        {
            get { return _isKey; }
        }

        /// <summary>
        /// Whether the property is a key with integer type.
        /// </summary>
        public bool IsIntegerKey
        {
            get { return _isIntegerKey; }
        }

        /// <summary>
        /// Get zero-based the column order if the property is a part of a composite key. 
        /// Negative value means not a part of a composite key.
        /// </summary>
        public int KeyColumnOrder
        {
            get { return _keyColumnOrder; }
        }
    }
}
