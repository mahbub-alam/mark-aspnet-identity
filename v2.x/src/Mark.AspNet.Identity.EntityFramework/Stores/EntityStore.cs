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
using System.Data.Entity;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents generic entity store.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    internal class EntityStore<TEntity, TKey> : IDisposable
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        private DbContext _context;

        /// <summary>
        /// Get database context.
        /// </summary>
        public DbContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Get the underlying entity set.
        /// </summary>
        public DbSet<TEntity> EntitySet
        {
            get;
            private set;
        }

        /// <summary>
        /// Initialize a new instance of the class with the database context.
        /// </summary>
        /// <param name="context">Database context.</param>
        public EntityStore(DbContext context)
        {
            _context = context;
            EntitySet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Create entity by adding to the entity set.
        /// </summary>
        /// <param name="entity">Entity to be added.</param>
        public void Create(TEntity entity)
        {
            ThrowIfDisposed();
            this.EntitySet.Add(entity);
        }

        /// <summary>
        /// Delete entity by removing it from the entity set.
        /// </summary>
        /// <param name="entity">Entity to be deleted.</param>
        public void Delete(TEntity entity)
        {
            ThrowIfDisposed();
            this.EntitySet.Remove(entity);
        }

        /// <summary>
        /// Update entity by marking it changed in the database context.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            ThrowIfDisposed();
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Find entity by it's id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Returns the entity if found; otherwise, returns null.</returns>
        public async Task<TEntity> FindByIdAsync(TKey id)
        {
            ThrowIfDisposed();
            return await this.EntitySet.FindAsync(new object[] { id });
        }

        #region IDisposable Support

        protected bool _isDisposed = false; // To detect redundant calls

        /// <summary>
        /// Throw exception if the object is already disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose managed resources. Set large fields to null inside <see cref="DisposeExtra()"/> method since, 
        /// that method will be called whether the <see cref="Dispose()"/> method is called by the finalizer or 
        /// your code.
        /// </summary>
        protected virtual void DisposeManaged()
        {
            // DbContext will be disposed by parent class.
        }

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields (managed/unmanaged) to null. This method 
        /// will be called whether the <see cref="Dispose()"/> method is called by the finalizer or your code.
        /// </summary>
        protected virtual void DisposeExtra()
        {
            this.EntitySet = null;
            _context = null;
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    DisposeManaged();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                DisposeExtra();

                _isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~EntityStore()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Dispose managed or unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion End of - IDisposable Support
    }
}
