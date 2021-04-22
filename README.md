NOTE: This respoitory is archived and no longer active.

# Mark.AspNet.Identity
##### ASP.NET Identity 2.x implementations/providers for Entity Framework (Any driver that supports EF), MySQL (ADO.NET) and MS SQL Server (ADO.NET)

## Introduction
For user profile, authentication and related management, ASP.NET 5 provided a new way to do these things named as **ASP.NET Identity** which was entirely re-written from scratch and this one system can be used with all of the ASP.NET frameworks, such as ASP.NET MVC, Web Forms, Web Pages, Web API, and SignalR. It provides local login like username/password as well as external/remote logins using providers like Google, Facebook, Microsoft and others using OpenID/OAuth. The core ASP.NET Identity interface/system is defined in the **Microsoft.AspNet.Identity.Core** package. The system uses database or any other persistent (implemented by the provider) back-end to store user information like profile, logins, roles and claims. 

But, Microsoft only implemented a single provider for the system using only Entity Framework (**Microsoft.AspNet.Identity.EntityFramework**) which internally uses Microsoft SQL Server style SQL for some queries, specially validation. So, that provider cannot be used with other persistent mechanism.

This project implemented provider for Entity Framework that can be used by any persistent system that implemented Entity Framework provider like MySQL, SQL Server and Oracle. It also implemented providers for MySQL and MS SQL Server using pure ADO.NET. **Provider for NHibernate will be implemented in the future**.

## Features
- Supports ASP.NET Identity v2.x (Currently 2.2.1)
- Supports same features in ASP.NET Identity V2.x like same domain/entity properties.
- Used the same names for classes used by Microsoft implementation like IdentityUser, IdentityRole, UserStore, RoleStore etc.
- Supports for custom properties are similar to Microsoft implementation. To allow custom properties into consideration, you need to update entity configuration class similar to EF mapping so that it can do dynamic read/write of entities.

## Instruction

Here is the list of things you need to do to use the provider(s) implemented for ASP.NET Identity-

1. For database creation, you need to execute [SQL script](v2.x/sql) on your SQL Server/MySQL database to create tables required by the ASP.NET Identity provider. EF provider can create table automatically like Microsoft EF implementation using default mapping implementation. You can change the names of tables and columns by providing custom configuration that will override default configuration on all providers (EF, ADO.NET).
2. Create a new ASP.NET MVC 5 project using **Individual User Accounts** authentication type.
3. Remove Entity Framework and Microsoft ASP.NET Identity Entity Framework packages-
    - Uninstall-Package Microsoft.AspNet.Identity.EntityFramework
    - Uninstall-Package EntityFramework (unless you will be using EF Identity provider)
4. 	Build the project solution and you will find necessary assemblies inside the respective module's **/Bin** directory. All these assemblies can be collectively found in the [/build](v2.x/build) directory as they are copied when each project is built. The following assemblies are needed depending on provider-
    - **Microsoft.AspNet.Identity.Core.dll** (which is the core module for ASP.NET Identity system and is already installed as a package.)
    - **Mark.AspNet.Identity.Core.dll** (which is a base module for all Identity providers.)
    - **Mark.AspNet.Identity.EntityFramework.dll** (Add if you want to use EF Identity provider. You will also need EF driver package for each respective database.)
    - **Mark.AspNet.Identity.MySql.dll** (Add if you want to use MySQL ADO.NET Identity  provider. You will also need to install *MySql.Data* package for MySQL ADO.NET driver.)
    - **Mark.AspNet.Identity.SqlServer.dll** (Add if you want to use SQL Server ADO.NET Identity provider. The ADO.NET driver already provided by the .NET Framework.)
5. 	Now, update code in the created MVC project to reflect new Identity provider. Follow instruction document for each Identity provider type-
    - For EF Identity provider, see [ASP.NET MVC 5 source update instruction for EF Identity provider](v2.x/docs/mvc5-source-update-entity-framework.md).
    - For SQL Server (ADO.NET) Identity provider, see [ASP.NET MVC 5 source update instruction for SQL Server Identity provider](v2.x/docs/mvc5-source-update-sqlserver.md).
    - For MySQL (ADO.NET) Identity provider, see [ASP.NET MVC 5 source update instruction for MySQL Identity provider](v2.x/docs/mvc5-source-update-mysql.md).

## License
[Apache License 2.0](LICENSE.txt)
