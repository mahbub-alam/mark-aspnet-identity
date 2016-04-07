// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents mapping configuration for 'UserLogin' table.
    /// </summary>
    /// <typeparam name="TUserLogin">User login type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityUserLoginMap<TUserLogin, TKey> : EntityMap<TUserLogin> 
        where TUserLogin : IdentityUserLogin<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        public IdentityUserLoginMap(string tableName) : base(tableName)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => new
            {
               p.LoginProvider,
               p.ProviderKey,
               p.UserId
            });

            Property(p => p.LoginProvider)
                .HasColumnName("LoginProvider");

            Property(p => p.ProviderKey)
                .HasColumnName("ProviderKey");

            Property(p => p.UserId)
                .HasColumnName("UserId");
        }
    }
}
