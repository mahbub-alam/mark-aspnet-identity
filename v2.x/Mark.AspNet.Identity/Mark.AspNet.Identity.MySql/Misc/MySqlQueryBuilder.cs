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
using Mark.Data;
using Mark.Data.Common;
using Mark.Data.ModelConfiguration;

namespace Mark.AspNet.Identity.MySql
{
    /// <summary>
    /// Represents MySQL query builder that generates SQL query from entity configuration.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MySqlQueryBuilder<TEntity> : DbQueryBuilder<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="configuration">Entity configuration.</param>
        public MySqlQueryBuilder(EntityConfiguration<TEntity> configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Get the quoted identifier of the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier to be quoted.</param>
        /// <returns>Returns quoted identifier.</returns>
        public override string GetQuotedIdentifier(string identifier)
        {
            return String.Format("`{0}`", identifier);
        }
    }
}
