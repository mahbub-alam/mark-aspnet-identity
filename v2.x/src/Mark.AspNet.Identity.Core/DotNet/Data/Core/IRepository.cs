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

namespace Mark.DotNet.Data
{
    /// <summary>
    /// Represents base repository interface.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface IRepository<TEntity> 
        where TEntity : IEntity
    {
        /// <summary>
        /// Set unit of work reference to be used by the repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work reference.</param>
        void SetUnitOfWork(IUnitOfWork unitOfWork);

        /// <summary>
        /// Add entity to the repository.
        /// </summary>
        /// <param name="item">entity to be added.</param>
        void Add(TEntity item);

        /// <summary>
        /// Remove entity from the repository.
        /// </summary>
        /// <param name="item">entity to be removed.</param>
        void Remove(TEntity item);

        /// <summary>
        /// Update change of entity to the repository.
        /// </summary>
        /// <param name="item">entity to be updated.</param>
        void Change(TEntity item);
    }
}
