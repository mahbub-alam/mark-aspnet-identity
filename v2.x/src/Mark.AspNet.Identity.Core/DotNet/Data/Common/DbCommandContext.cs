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
using System.Data.Common;
using System.Reflection;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.DotNet.Data.Common
{
    /// <summary>
    /// Represents command context for ADO.NET style storage context.
    /// </summary>
    public sealed class DbCommandContext : Disposable, IDbCommandContext
    {
        private DbCommand _command;
        private DbParameterCollection _parameters;
        private ICollection<IEntity> _list;
        private Action<IDbParameterCollection, IEntity> _setForEach;
        private PropertyInfo _idPropInfo = null;
        private bool _readLastInsertedId = false;
        
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

        private void SetIdValue(IEntity entity, object value)
        {
            // Means, no id value was generated in the database
            if (value.GetType() == typeof(DBNull))
            {
                return;
            }

            value = Convert.ChangeType(value, _idPropInfo.PropertyType);
            _idPropInfo.SetValue(entity, value);
        }

        /// <summary>
        /// Set command parameters for each entity in the given entity collection.
        /// </summary>
        /// <param name="setAction">Action that will execute for each entity in the collection.</param>
        /// <param name="idPropertyConfig">Optional property configuration for 
        /// entity id property (database generated) that will be populated.</param>
        public void SetParametersForEach<TEntity>(Action<IDbParameterCollection, TEntity> setAction,
            PropertyConfiguration idPropertyConfig = null)
            where TEntity : IEntity
        {
            ThrowIfDisposed();

            _setForEach = new Action<IDbParameterCollection, IEntity>((collection, entity) =>
            {
                setAction(collection, (TEntity)entity);
            });

            // For last inserted id
            if (idPropertyConfig != null)
            {
                if (!idPropertyConfig.IsKey)
                {
                    throw new ArgumentException(
                        "Passed property configuration is not the configuration of a key property");
                }

                _idPropInfo = typeof(TEntity).GetProperty(idPropertyConfig.PropertyName);

                _readLastInsertedId = _command.CommandText.IndexOf("INSERT", 
                    StringComparison.OrdinalIgnoreCase) >= 0 &&
                    idPropertyConfig.IsIntegerKey;
            }
        }

        /// <summary>
        /// Get the underlying command.
        /// </summary>
        public DbCommand Command
        {
            get
            {
                ThrowIfDisposed();
                return _command;
            }
        }

        /// <summary>
        /// Get command parameter collection.
        /// </summary>
        public IDbParameterCollection Parameters
        {
            get
            {
                ThrowIfDisposed();
                return _parameters;
            }
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <returns>Returns the number of rows affected.</returns>
        public int Execute()
        {
            ThrowIfDisposed();

            int retValue = 0;

            if (_list != null)
            {
                if (_readLastInsertedId)
                {
                    object idValue;

                    foreach (IEntity entity in _list)
                    {
                        _setForEach(_parameters, entity);
                        idValue = _command.ExecuteScalar();
                        SetIdValue(entity, idValue);
                        ++retValue;
                    }
                }
                else
                {
                    foreach (IEntity entity in _list)
                    {
                        _setForEach(_parameters, entity);
                        retValue += _command.ExecuteNonQuery();
                    }
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
            ThrowIfDisposed();

            return _command.ExecuteReader();
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected override void DisposeManaged()
        {
            _command.Dispose();
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected override void DisposeExtra()
        {
            _command = null;
            _parameters = null;
            _list = null;
            _setForEach = null;
        }

    }
}
