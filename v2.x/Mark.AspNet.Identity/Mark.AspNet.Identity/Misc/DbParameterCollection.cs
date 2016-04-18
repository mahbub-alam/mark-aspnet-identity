// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Mark.AspNet.Identity.Common;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents command parameter collection.
    /// </summary>
    public class DbParameterCollection : IDbParameterCollection
    {
        private DbCommand _command;
        private System.Data.Common.DbParameterCollection _parameters;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="command">Database command.</param>
        public DbParameterCollection(DbCommand command)
        {
            _command = command;
            _parameters = _command.Parameters;
        }

        /// <summary>
        /// Get or set command parameter. If a command parameter not found, 
        /// a new parameter with the given field identifier is created and returned.
        /// </summary>
        /// <param name="fieldIdentifier">Field identifier without '@' as prefix.</param>
        /// <returns>Returns a command parameter associated with the field identifier. If not found, 
        /// a new parameter with the given field identifier is created and returned.</returns>
        public DbParameter this[string fieldIdentifier]
        {
            get
            {
                if (_parameters.Contains("@" + fieldIdentifier))
                {
                    return _parameters["@" + fieldIdentifier];
                }
                else
                {
                    DbParameter parameter = _command.CreateParameter();
                    parameter.ParameterName = "@" + fieldIdentifier;
                    _parameters.Add(parameter);
                    return parameter;
                }
            }

            set
            {
                _parameters["@" + fieldIdentifier] = value;
            }
        }
    }
}
