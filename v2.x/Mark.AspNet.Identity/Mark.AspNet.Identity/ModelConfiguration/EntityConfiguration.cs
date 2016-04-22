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

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents entity configuration like table name, fields etc.
    /// </summary>
    public abstract class EntityConfiguration
    {
        private string _tableName;
        private Dictionary<string, string> _fieldNames;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        public EntityConfiguration()
        {
            _tableName = "";
            _fieldNames = new Dictionary<string, string>(16);
            Configure();
        }

        /// <summary>
        /// Get the given field name.
        /// </summary>
        /// <param name="fieldIdentifier">Field identifier.</param>
        /// <returns>Returns field name if set; otherwise, returns empty string.</returns>
        public string GetFieldName(string fieldIdentifier)
        {
            if (_fieldNames.ContainsKey(fieldIdentifier))
            {
                return _fieldNames[fieldIdentifier];
            }

            return "";
        }

        /// <summary>
        /// Set field name.
        /// </summary>
        /// <param name="fieldIdentifier">Field identifier.</param>
        /// <param name="fieldName">Field name to set.</param>
        protected void SetFieldName(string fieldIdentifier, string fieldName)
        {
            if (_fieldNames.ContainsKey(fieldIdentifier))
            {
                _fieldNames[fieldIdentifier] = fieldName;
            }
            else
            {
                _fieldNames.Add(fieldIdentifier, fieldName?? "");
            }
        }

        /// <summary>
        /// Get table name.
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            protected set { _tableName = value; }
        }

        /// <summary>
        /// Get field name.
        /// </summary>
        /// <param name="fieldIdentifier">Entity field identifier.</param>
        /// <returns>Returns field name if set; otherwise, returns empty string.</returns>
        public string this[string fieldIdentifier]
        {
            get
            {
                return GetFieldName(fieldIdentifier);
            }

            protected set
            {
                SetFieldName(fieldIdentifier, value);
            }
        }

        /// <summary>
        /// Configure entity.
        /// </summary>
        protected abstract void Configure();
    }
}
