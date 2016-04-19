// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents disposable support.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        #region IDisposable Support

        private bool _isDisposed = false; // To detect redundant calls

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
        /// Dispose managed resources. Set large fields to null inside 
        /// <see cref="DisposeExtra()"/> method since, that method will 
        /// be called whether the <see cref="Disposable.Dispose()"/> 
        /// method is called by the finalizer or your code.
        /// </summary>
        protected abstract void DisposeManaged();

        /// <summary>
        /// Dispose unmanaged resources and/or set large fields 
        /// (managed/unmanaged) to null. This method will be called whether 
        /// the <see cref="Disposable.Dispose()"/> method is called by the 
        /// finalizer or your code.
        /// </summary>
        protected abstract void DisposeExtra();

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
        /// <summary>
        /// Finializer. 
        /// </summary>
        ~Disposable()
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

        #endregion End of - IDisposable Support
    }
}
