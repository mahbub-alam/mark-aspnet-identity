// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.Core
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
    }
}
