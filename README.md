
# Mark.AspNet.Identity
##### ASP.NET Identity 2.x implementations/providers for Entity Framework (Any driver that supports EF), MySQL (ADO.NET) and MS SQL Server (ADO.NET)

## Introduction
For user profile, authentication and related management, ASP.NET 5 provided a new way to do these things named as **ASP.NET Identity** which was entirely re-written from scratch and this one system can be used with all of the ASP.NET frameworks, such as ASP.NET MVC, Web Forms, Web Pages, Web API, and SignalR. It provides local login like username/password as well as external/remote logins using providers like Google, Facebook, Microsoft and others using OpenID/OAuth. The core ASP.NET Identity interface/system is defined in the **Microsoft.AspNet.Identity.Core** package. The system uses database or any other persistent (implemented by the provider) backend to store user informations like profile, logins, roles and claims. 

But, Microsoft only implemented a single provider for the system using only Entity Framework (**Microsoft.AspNet.Identity.EntityFramework**) which internally uses Microsoft SQL Server style SQL for some queries, specially validation. So, that provider cannot be used with other persistent mechanism.

This project implemented provider for Entity Framework that can be used by any persistent system that implemented Entity Framework provider like MySQL, SQL Server and Oracle. It also implemented providers for MySQL and MS SQL Server using pure ADO.NET. Provider for NHibernate will be implemented in the future.

## Features
- Supports ASP.NET Identity v2.x (Currently 2.2.1)
- Supports same features in ASP.NET Identity V2.x like same domain/entity properties.
- Used the same names for classes used by Microsoft implementation like IdentityUser, IdentityRole, UserStore, RoleStore etc.
- Supports for custom properites are similar to Microsoft implementation. To allow custom properties into consideration, you need to update entity configuration class similar to EF mapping so that it can do dynamic read/write of entities. 

## Instruction




