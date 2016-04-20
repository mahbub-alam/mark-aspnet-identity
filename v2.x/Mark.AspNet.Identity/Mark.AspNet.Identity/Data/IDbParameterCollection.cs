// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Mark.Data
{
    /// <summary>
    /// Represents command parameter collection interface.
    /// </summary>
    public interface IDbParameterCollection
    {
        /// <summary>
        /// Get or set command parameter. If a command parameter not found, 
        /// a new parameter with the given field identifier is created and returned.
        /// </summary>
        /// <param name="fieldIdentifier">Field identifier without '@' as prefix.</param>
        /// <returns>Returns a command parameter associated with the field identifier. If not found, 
        /// a new parameter with the given field identifier is created and returned.</returns>
        DbParameter this[string fieldIdentifier]
        {
            get;set;
        }
    }
}
