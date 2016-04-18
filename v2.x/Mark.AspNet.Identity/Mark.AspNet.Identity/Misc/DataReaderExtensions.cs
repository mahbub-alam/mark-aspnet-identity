﻿// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents ADO.NET data reader extensions.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static string GetSafeString(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetString(ordinal);
            }

            return "";
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static bool GetSafeBoolean(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetBoolean(ordinal);
            }

            return false;
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static DateTime GetSafeDateTime(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetDateTime(ordinal);
            }

            return new DateTime();
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static int GetSafeInt32(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetInt32(ordinal);
            }

            return default(int);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static long GetSafeInt64(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetInt64(ordinal);
            }

            return default(long);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static short GetSafeInt16(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetInt16(ordinal);
            }

            return default(short);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static byte GetSafeByte(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetByte(ordinal);
            }

            return default(byte);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static char GetSafeChar(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetChar(ordinal);
            }

            return default(char);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static decimal GetSafeDecimal(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetDecimal(ordinal);
            }

            return default(decimal);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static float GetSafeFloat(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetFloat(ordinal);
            }

            return default(float);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static double GetSafeDouble(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetDouble(ordinal);
            }

            return default(double);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static Guid GetSafeGuid(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetGuid(ordinal);
            }

            return default(Guid);
        }

        /// <summary>
        /// Get the value of the specified field.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="fieldName">Table column name.</param>
        /// <returns>Returns value if found; otherwise, returns default value.</returns>
        public static object GetSafeValue(this IDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetValue(ordinal);
            }

            return default(object);
        }

    }
}
