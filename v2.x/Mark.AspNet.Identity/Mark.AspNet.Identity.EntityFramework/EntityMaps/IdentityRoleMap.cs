// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents mapping configuration for 'Role' table.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class IdentityRoleMap<TRole, TKey, TUserRole>
        : EntityMap<TRole>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserRole : IdentityUserRole<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        public IdentityRoleMap(string tableName) : base(tableName)
        {
        }

        /// <summary>
        /// Map primary key.
        /// </summary>
        protected override void MapPrimaryKey()
        {
            HasKey(p => p.Id);
            Property(p => p.Id)
                .HasColumnName("Id");
        }

        /// <summary>
        /// Map all non-key fields.
        /// </summary>
        protected override void MapFields()
        {
            Property(p => p.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UK_Role_Name") { IsUnique = true }));
        }

        /// <summary>
        /// Map relationship among entities.
        /// </summary>
        protected override void MapRelationships()
        {
            HasMany(p => p.Users)
                .WithRequired()
                .HasForeignKey(p => p.RoleId);
        }
    }

    /// <summary>
    /// Represents mapping configuration for 'Role' table.
    /// </summary>
    /// <typeparam name="TRole">Role type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityRoleMap<TRole, TKey>
        : IdentityRoleMap<TRole, TKey, IdentityUserRole<TKey>>
        where TRole : IdentityRole<TKey, IdentityUserRole<TKey>>
        where TKey : struct
    {
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="tableName">Table name this entity type is mappped to.</param>
        public IdentityRoleMap(string tableName) : base(tableName)
        {
        }
    }
}
