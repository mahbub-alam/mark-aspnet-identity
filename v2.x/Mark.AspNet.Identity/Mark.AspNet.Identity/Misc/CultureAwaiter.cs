// Compiled by: MAB (from MS source reference)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Threading;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents an awaiter like ConfiguredTaskAwaiter but with thread's cultures preservation support.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    public struct CultureAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly Task<TResult> _task;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="task">Target task.</param>
        public CultureAwaiter(Task<TResult> task)
        {
            _task = task;
        }

        /// <summary>
        /// Gets a value that specifies whether the task being awaited is completed.  
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return _task.IsCompleted;
            }
        }

        /// <summary>
        /// Returns an awaiter for this awaitable object.
        /// </summary>
        /// <returns>Returns the awaiter.</returns>
        public CultureAwaiter<TResult> GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Ends the await on the completed task.
        /// </summary>
        /// <returns>Returns the computed result.</returns>
        public TResult GetResult()
        {
            return _task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Schedules the continuation action for the task associated with this awaiter.
        /// </summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Schedules the continuation action for the task associated with this awaiter.
        /// </summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            // Save culture info to be used with the new continuation thread
            CultureInfo savedThreadCultureInfo = Thread.CurrentThread.CurrentCulture;
            CultureInfo savedThreadUICultureInfo = Thread.CurrentThread.CurrentUICulture;

            ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter awaiter = _task.ConfigureAwait(false).GetAwaiter();

            awaiter.UnsafeOnCompleted(() =>
            {
                // Save culture info of THIS thread so that it can be restored after executing continuation
                CultureInfo currentThreadCultureInfo = Thread.CurrentThread.CurrentCulture;
                CultureInfo currentThreadUICultureInfo = Thread.CurrentThread.CurrentUICulture;

                // Use previous thread's culture info
                Thread.CurrentThread.CurrentCulture = savedThreadCultureInfo;
                Thread.CurrentThread.CurrentUICulture = savedThreadUICultureInfo;

                try
                {
                    continuation();
                }
                finally
                {
                    // Restore the culture info of the current thread
                    Thread.CurrentThread.CurrentCulture = currentThreadCultureInfo;
                    Thread.CurrentThread.CurrentUICulture = currentThreadUICultureInfo;
                }
            });
        }
    }

    /// <summary>
    /// Represents an awaiter like ConfiguredTaskAwaiter but with thread's cultures preservation support.
    /// </summary>
    public struct CultureAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly Task _task;

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="task">Target task.</param>
        public CultureAwaiter(Task task)
        {
            _task = task;
        }

        /// <summary>
        /// Gets a value that specifies whether the task being awaited is completed.  
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return _task.IsCompleted;
            }
        }

        /// <summary>
        /// Returns an awaiter for this awaitable object.
        /// </summary>
        /// <returns>Returns the awaiter.</returns>
        public CultureAwaiter GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Ends the await on the completed task.
        /// </summary>
        public void GetResult()
        {
            _task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Schedules the continuation action for the task associated with this awaiter.
        /// </summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Schedules the continuation action for the task associated with this awaiter.
        /// </summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            // Save culture info to be used with the new continuation thread
            CultureInfo savedThreadCultureInfo = Thread.CurrentThread.CurrentCulture;
            CultureInfo savedThreadUICultureInfo = Thread.CurrentThread.CurrentUICulture;

            ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter = _task.ConfigureAwait(false).GetAwaiter();

            awaiter.UnsafeOnCompleted(() =>
            {
                // Save culture info of THIS thread so that it can be restored after executing continuation
                CultureInfo currentThreadCultureInfo = Thread.CurrentThread.CurrentCulture;
                CultureInfo currentThreadUICultureInfo = Thread.CurrentThread.CurrentUICulture;

                // Use previous thread's culture info
                Thread.CurrentThread.CurrentCulture = savedThreadCultureInfo;
                Thread.CurrentThread.CurrentUICulture = savedThreadUICultureInfo;

                try
                {
                    continuation();
                }
                finally
                {
                    // Restore the culture info of the current thread
                    Thread.CurrentThread.CurrentCulture = currentThreadCultureInfo;
                    Thread.CurrentThread.CurrentUICulture = currentThreadUICultureInfo;
                }
            });
        }
    }

}
