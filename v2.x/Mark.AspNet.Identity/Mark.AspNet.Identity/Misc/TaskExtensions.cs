// Compiled by: MAB (from MS source reference)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity
{
    /// <summary>
    /// Represents extensions for task classes.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Get an awaiter with thread's cultures preservation support.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="task">Target task.</param>
        /// <returns>Returns an awaiter.</returns>
        public static CultureAwaiter<TResult> WithCurrentCulture<TResult>(this Task<TResult> task)
        {
            return new CultureAwaiter<TResult>(task);
        }

        /// <summary>
        /// Get an awaiter with thread's cultures preservation support.
        /// </summary>
        /// <param name="task">Target task.</param>
        /// <returns>Returns an awaiter.</returns>
        public static CultureAwaiter WithCurrentCulture(this Task task)
        {
            return new CultureAwaiter(task);
        }
    }
}
