// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using Mark.AspNet.Identity.ModelConfiguration;

namespace Mark.AspNet.Identity.EntityFramework
{
    /// <summary>
    /// Represents database context that uses custom entities.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TRole">Role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserLogin">User login entity type.</typeparam>
    /// <typeparam name="TUserRole">User role entity type.</typeparam>
    /// <typeparam name="TUserClaim">User claim entity type.</typeparam>
    public class IdentityDbContext<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> : DbContext
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        private IdentityUserMap<TUser, TKey, TUserLogin, TUserRole, TUserClaim> _userMap;
        private IdentityRoleMap<TRole, TKey, TUserRole> _roleMap;
        private IdentityUserLoginMap<TUserLogin, TKey> _userLoginMap;
        private IdentityUserRoleMap<TUserRole, TKey> _userRoleMap;
        private IdentityUserClaimMap<TUserClaim, TKey> _userClaimMap;

        /// <summary>
        /// Represents user entities.
        /// </summary>
        public virtual IDbSet<TUser> Users
        {
            get; set;
        }

        /// <summary>
        /// Represents role entities.
        /// </summary>
        public virtual IDbSet<TRole> Roles
        {
            get; set;
        }

        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString.
        /// </summary>
        public IdentityDbContext() : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or name of the connection string.</param>
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a database, and initializes it from
        /// the given model. The connection will not be disposed when the context is disposed if contextOwnsConnection is
        /// false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model.  The connection will not be disposed when the context is
        /// disposed if contextOwnsConnection is false.
        /// </param>
        public IdentityDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of
        /// the database to which a connection will be made, and initializes it from
        /// the given model.  The by-convention name is the full name (namespace + class
        /// name) of the derived context class.  See the class remarks for how this is
        /// used to create a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(DbCompiledModel model) : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect
        /// to a database. The connection will not be disposed when the context is disposed
        /// if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">If set to true the connection is disposed when the context is disposed,
        /// otherwise the caller must dispose the connection.</param>
        public IdentityDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes
        /// it from the given model.  See the class remarks for how this is used to create
        /// a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized,
        /// but before the model has been locked down and used to initialize the context.
        /// The default implementation of this method does default initialization for default 
        /// ASP.NET identity entities, but it can be overridden in a derived class such that 
        /// the model can be further configured before it is locked down. 
        /// NOTE: If only default ASP.NET identity entities need to be customeized, instead of 
        /// overriding this method, override <see cref="SetIdentityEntityMappings()"/> method.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("'modelBuilder' parameter null");
            }

            SetIdentityEntityMappings();

            modelBuilder.Configurations.Add(_userMap);
            modelBuilder.Configurations.Add(_roleMap);
            modelBuilder.Configurations.Add(_userLoginMap);
            modelBuilder.Configurations.Add(_userRoleMap);
            modelBuilder.Configurations.Add(_userClaimMap);

            // Mapping set. Not needed now.
            _userMap = null;
            _roleMap = null;
            _userLoginMap = null;
            _userRoleMap = null;
            _userClaimMap = null;
        }

        /// <summary>
        /// Override this method to change all identity entities mapping instead of OnModelCreating() method.
        /// Call 'SetIdentityXXXMap(...)' named methods inside to set entities mapping.
        /// </summary>
        protected virtual void SetIdentityEntityMappings()
        {
            SetIdentiyUserMap(null);
            SetIdentiyRoleMap(null);
            SetIdentiyUserLoginMap(null);
            SetIdentiyUserRoleMap(null);
            SetIdentiyUserClaimMap(null);
        }

        /// <summary>
        /// Set mapping of identity user.
        /// </summary>
        /// <param name="map">Mapping. If null, the default mapping will be used.</param>
        protected void SetIdentiyUserMap(IdentityUserMap<TUser, TKey, TUserLogin, TUserRole, TUserClaim> map)
        {
            if (map != null)
            {
                _userMap = map;
            }
            else
            {
                _userMap = new IdentityUserMap<TUser, TKey, TUserLogin, TUserRole, TUserClaim>(
                    new UserConfiguration());
            }
        }

        /// <summary>
        /// Set mapping of identity role.
        /// </summary>
        /// <param name="map">Mapping. If null, the default mapping will be used.</param>
        protected void SetIdentiyRoleMap(IdentityRoleMap<TRole, TKey, TUserRole> map)
        {
            if (map != null)
            {
                _roleMap = map;
            }
            else
            {
                _roleMap = new IdentityRoleMap<TRole, TKey, TUserRole>(new RoleConfiguration());
            }
        }

        /// <summary>
        /// Set mapping of identity user login.
        /// </summary>
        /// <param name="map">Mapping. If null, the default mapping will be used.</param>
        protected void SetIdentiyUserLoginMap(IdentityUserLoginMap<TUserLogin, TKey> map)
        {
            if (map != null)
            {
                _userLoginMap = map;
            }
            else
            {
                _userLoginMap = new IdentityUserLoginMap<TUserLogin, TKey>(new UserLoginConfiguration());
            }
        }

        /// <summary>
        /// Set mapping of identity user role.
        /// </summary>
        /// <param name="map">Mapping. If null, the default mapping will be used.</param>
        protected void SetIdentiyUserRoleMap(IdentityUserRoleMap<TUserRole, TKey> map)
        {
            if (map != null)
            {
                _userRoleMap = map;
            }
            else
            {
                _userRoleMap = new IdentityUserRoleMap<TUserRole, TKey>(new UserRoleConfiguration());
            }
        }

        /// <summary>
        /// Set mapping of identity user claim.
        /// </summary>
        /// <param name="map">Mapping. If null, the default mapping will be used.</param>
        protected void SetIdentiyUserClaimMap(IdentityUserClaimMap<TUserClaim, TKey> map)
        {
            if (map != null)
            {
                _userClaimMap = map;
            }
            else
            {
                _userClaimMap = new IdentityUserClaimMap<TUserClaim, TKey>(new UserClaimConfiguration());
            }
        }
    }

    /// <summary>
    /// Represents database context that uses default entities except custom entity for user.
    /// </summary>
    /// <typeparam name="TUser">User entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityDbContext<TUser, TKey> 
        : IdentityDbContext<
            TUser, 
            IdentityRole<TKey>, 
            TKey, 
            IdentityUserLogin<TKey>, 
            IdentityUserRole<TKey>, 
            IdentityUserClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString.
        /// </summary>
        public IdentityDbContext() : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or name of the connection string.</param>
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a database, and initializes it from
        /// the given model. The connection will not be disposed when the context is disposed if contextOwnsConnection is
        /// false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model.  The connection will not be disposed when the context is
        /// disposed if contextOwnsConnection is false.
        /// </param>
        public IdentityDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of
        /// the database to which a connection will be made, and initializes it from
        /// the given model.  The by-convention name is the full name (namespace + class
        /// name) of the derived context class.  See the class remarks for how this is
        /// used to create a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(DbCompiledModel model) : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect
        /// to a database. The connection will not be disposed when the context is disposed
        /// if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">If set to true the connection is disposed when the context is disposed,
        /// otherwise the caller must dispose the connection.</param>
        public IdentityDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes
        /// it from the given model.  See the class remarks for how this is used to create
        /// a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

    }

    /// <summary>
    /// Represents database context that uses default entities except custom entity for role.
    /// </summary>
    /// <typeparam name="TRole">Role entity type.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <typeparam name="TUserRole">User role type.</typeparam>
    public class IdentityDbContext<TRole, TKey, TUserRole>
        : IdentityDbContext<
            IdentityUser<TKey, IdentityUserLogin<TKey>, TUserRole, IdentityUserClaim<TKey>>,
            TRole,
            TKey,
            IdentityUserLogin<TKey>,
            TUserRole,
            IdentityUserClaim<TKey>>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserRole : IdentityUserRole<TKey> 
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString.
        /// </summary>
        public IdentityDbContext() : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or name of the connection string.</param>
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a database, and initializes it from
        /// the given model. The connection will not be disposed when the context is disposed if contextOwnsConnection is
        /// false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model.  The connection will not be disposed when the context is
        /// disposed if contextOwnsConnection is false.
        /// </param>
        public IdentityDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of
        /// the database to which a connection will be made, and initializes it from
        /// the given model.  The by-convention name is the full name (namespace + class
        /// name) of the derived context class.  See the class remarks for how this is
        /// used to create a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(DbCompiledModel model) : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect
        /// to a database. The connection will not be disposed when the context is disposed
        /// if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">If set to true the connection is disposed when the context is disposed,
        /// otherwise the caller must dispose the connection.</param>
        public IdentityDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes
        /// it from the given model.  See the class remarks for how this is used to create
        /// a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

    }

    /// <summary>
    /// Represents database context that uses default entities.
    /// </summary>
    /// <typeparam name="TKey">Id type.</typeparam>
    public class IdentityDbContext<TKey>
        : IdentityDbContext<
            IdentityUser<TKey>,
            IdentityRole<TKey>,
            TKey,
            IdentityUserLogin<TKey>,
            IdentityUserRole<TKey>,
            IdentityUserClaim<TKey>>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString.
        /// </summary>
        public IdentityDbContext() : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or name of the connection string.</param>
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a database, and initializes it from
        /// the given model. The connection will not be disposed when the context is disposed if contextOwnsConnection is
        /// false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model.  The connection will not be disposed when the context is
        /// disposed if contextOwnsConnection is false.
        /// </param>
        public IdentityDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of
        /// the database to which a connection will be made, and initializes it from
        /// the given model.  The by-convention name is the full name (namespace + class
        /// name) of the derived context class.  See the class remarks for how this is
        /// used to create a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(DbCompiledModel model) : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect
        /// to a database. The connection will not be disposed when the context is disposed
        /// if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">If set to true the connection is disposed when the context is disposed,
        /// otherwise the caller must dispose the connection.</param>
        public IdentityDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes
        /// it from the given model.  See the class remarks for how this is used to create
        /// a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public IdentityDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

    }
}
