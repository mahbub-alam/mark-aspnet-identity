// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Mark.AspNet.Identity.Common
{
    /// <summary>
    /// Represents command context for ADO.NET style storage context.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public class DbCommandContext<TEntity> : IDisposable 
        where TEntity : IEntity
    {
        private DbCommand _command;
        private ParameterCollection _parameters;
        private ICollection<TEntity> _list;
        private Action<ParameterCollection, TEntity> _setForEach;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="command">ADO.NET database command.</param>
        public DbCommandContext(DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("Command parameter null");
            }

            _command = command;
            _parameters = new ParameterCollection(_command);
            _list = null;
        }

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="command">ADO.NET database command.</param>
        /// <param name="list">A list of entity objects to be used with the command.</param>
        public DbCommandContext(DbCommand command, ICollection<TEntity> list)
        {
            if (command == null)
            {
                throw new ArgumentNullException("Command parameter null");
            }

            if (list == null || !list.Any())
            {
                throw new ArgumentNullException("List parameter null/empty");
            }

            _command = command;
            _parameters = new ParameterCollection(_command);
            _list = list;
        }

        /// <summary>
        /// Set command parameters for each entity in the given entity collection.
        /// </summary>
        /// <param name="setAction">Action that will execute for each entity in the collection.</param>
        public void SetParametersForEach(Action<ParameterCollection, TEntity> setAction)
        {
            _setForEach = setAction;
        }

        /// <summary>
        /// Get command parameter collection.
        /// </summary>
        public ParameterCollection Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <returns>Returns the number of rows affected.</returns>
        public int Execute()
        {
            int retValue = 0;

            if (_list != null)
            {
                foreach (TEntity entity in _list)
                {
                    _setForEach(_parameters, entity);
                    retValue += _command.ExecuteNonQuery();
                }
            }
            else
            {
                retValue = _command.ExecuteNonQuery();
            }

            return retValue;
        }

        /// <summary>
        /// Execute the command and returns a data reader.
        /// </summary>
        /// <returns>Returns a data reader.</returns>
        public DbDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        /// <summary>
        /// Dispose managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _command.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _command = null;
                _parameters = null;
                _list = null;
                _setForEach = null;
                _disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        /// <summary>
        /// Finalizer.
        /// </summary>
        ~DbCommandContext()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Dispose managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Represents command parameter collection.
        /// </summary>
        public class ParameterCollection
        {
            private DbCommand _command;
            private DbParameterCollection _parameters;

            /// <summary>
            /// Initialize a new instance of the class.
            /// </summary>
            /// <param name="command">Database command.</param>
            public ParameterCollection(DbCommand command)
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

            ///// <summary>
            ///// Add command parameter with value.
            ///// </summary>
            ///// <param name="fieldIdentifier">Field identifier without '@' as prefix.</param>
            ///// <param name="value">Value to be set.</param>
            ///// <returns>Returns the added command parameter.</returns>
            //public DbParameter Add(string fieldIdentifier, object value)
            //{
            //    DbParameter parameter = this[fieldIdentifier];
            //    parameter.Value = value;
            //    return parameter;
            //}
        }
    }
}
