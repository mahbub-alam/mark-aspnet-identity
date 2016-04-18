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
    public class DbCommandContext : IDbCommandContext
    {
        private DbCommand _command;
        private DbParameterCollection _parameters;
        private ICollection<IEntity> _list;
        private Action<IDbParameterCollection, IEntity> _setForEach;
        
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
            _parameters = new DbParameterCollection(_command);
            _list = null;
        }

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="command">ADO.NET database command.</param>
        /// <param name="list">A list of entity objects to be used with the command.</param>
        public DbCommandContext(DbCommand command, ICollection<IEntity> list)
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
            _parameters = new DbParameterCollection(_command);
            _list = list;
        }

        /// <summary>
        /// Set command parameters for each entity in the given entity collection.
        /// </summary>
        /// <param name="setAction">Action that will execute for each entity in the collection.</param>
        public void SetParametersForEach<TEntity>(Action<IDbParameterCollection, TEntity> setAction) where TEntity : IEntity
        {
            _setForEach = new Action<IDbParameterCollection, IEntity>((collection, entity) =>
            {
                setAction(collection, (TEntity)entity);
            });
        }

        /// <summary>
        /// Get the underlying command.
        /// </summary>
        public DbCommand Command
        {
            get { return _command; }
        }

        /// <summary>
        /// Get command parameter collection.
        /// </summary>
        public IDbParameterCollection Parameters
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
                foreach (IEntity entity in _list)
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
    }
}
