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
using System.Data.Common;
using Mark.DotNet.Data.ModelConfiguration;

namespace Mark.DotNet.Data
{
    /// <summary>
    /// Represents command context interface for ADO.NET style storage context.
    /// </summary>
    public interface IDbCommandContext : IDisposable 
    {
        /// <summary>
        /// Set command parameters for each entity in the given entity collection.
        /// </summary>
        /// <param name="setAction">Action that will execute for each entity in the collection.</param>
        /// <param name="idPropertyConfig">Optional property configuration for 
        /// entity id property (database generated) that will be populated.</param>
        void SetParametersForEach<TEntity>(Action<IDbParameterCollection, TEntity> setAction,
            PropertyConfiguration idPropertyConfig = null)
            where TEntity : IEntity;

        /// <summary>
        /// Get the underlying command.
        /// </summary>
        DbCommand Command
        {
            get;
        }

        /// <summary>
        /// Get command parameter collection.
        /// </summary>
        IDbParameterCollection Parameters
        {
            get;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <returns>Returns the number of rows affected.</returns>
        int Execute();

        /// <summary>
        /// Execute the command and returns a data reader.
        /// </summary>
        /// <returns>Returns a data reader.</returns>
        DbDataReader ExecuteReader();
    }
}
