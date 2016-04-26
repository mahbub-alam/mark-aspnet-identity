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

namespace Mark.DotNet
{
    /// <summary>
    /// Represents general utility extensions.
    /// </summary>
    public static class UtilExtensions
    {
        /// <summary>
        /// Get DB null if the value is null; otherwise, get the actual value.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Returns DB null if null; otherwise, returns actual value.</returns>
        public static object GetDBNullIfNull(this object target)
        {
            if (target == null)
            {
                return DBNull.Value;
            }
            else
            {
                Type type = target.GetType();

                // Whether the type is actually a nullable struct
                if (type.IsValueType)
                {
                    Type uType = Nullable.GetUnderlyingType(type);
                    bool isNullable = uType != null;


                }


            }

            return target;
        }

        /// <summary>
        /// Get DB null if the value is null; otherwise, get the actual value.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Returns DB null if null; otherwise, returns actual value.</returns>
        public static object GetDBNullIfNull(this string target)
        {
            if (target == null)
            {
                return DBNull.Value;
            }

            return target;
        }

        /// <summary>
        /// Get DB null if the value is null; otherwise, get the actual value.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Returns DB null if null; otherwise, returns actual value.</returns>
        public static object GetDBNullIfNull(this DateTime? target)
        {
            if (target == null || target == DateTime.MinValue)
            {
                return DBNull.Value;
            }

            return target.Value;
        }

        /// <summary>
        /// Get DB null if the value is null; otherwise, get the actual value.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Returns DB null if null; otherwise, returns actual value.</returns>
        public static object GetDBNullIfNull<T>(this T? target) where T : struct
        {
            if (target == null)
            {
                return DBNull.Value;
            }

            return target.Value;
        }

        /// <summary>
        /// Get default value of the given type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Returns default value.</returns>
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
