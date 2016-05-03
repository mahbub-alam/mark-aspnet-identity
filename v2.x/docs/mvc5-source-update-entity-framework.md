## How to update ASP.NET MVC 5 project to use Mark.AspNet.Identity.EntityFramework Identity provider

**NOTE:** You can see **changes visually** by comparing your created project with the sample project named **~/samples/MvcAspNetIdentityEFSample** using **diff tool**.

By default, the project use `string` **(which is GUID)** as the primary key for identity provider. Follow the instruction provided below and make necessary changes-

1. In **App\_Start/IdentityConfig.cs** file-
    1. Change **using statement** to add *Mark.AspNet.Identity.EntityFramework* namespace.
    2. If you want to use another type of primary key like `int` other than `string`, add `int` as generic key parameter  after `ApplicationUser` which is used as generic parameter in all the places of the file (replace key type `string` to `int` where needed).
2. In **App\_Start/Startup.Auth.cs** file-
    1. If you will be using `string` as primary key, no change is needed in the file. 
    2. Otherwise, add `int` as generic key parameter  after `ApplicationUser` in `SecurityStampValidator.OnValidateIdentity` method that is being used to set the  `OnValidateIdentity` parameter of the `CookieAuthenticationProvider` class constructor.
    3. `SecurityStampValidator.OnValidateIdentity` method has a parameter named `regenerateIdentity` which need to be renamed to `regenerateIdentityCallback` because we will be using the overloading method that has another parameter named `getUserIdCallback` which we will set to retrieve **User Id** by setting a lambda callback (the default callback returns User Id as string). The lambda callback will be `getUserIdCallback: identity => identity.GetUserId<int>()`.
3. In **Models/IdentityModels.cs** file-
    1. Add **using statement** to add *Mark.AspNet.Identity* namespace.
    2. Change **using statement** to add *Mark.AspNet.Identity.EntityFramework* namespace.
    3. For `int` key type, update the class definition of `ApplicationUser : IdentityUser` to add `int` as generic key parameter like  `IdentityUser<int>`.
    4. Remove `throwIfV1Schema: false` parameter from constructor of the `ApplicationDbContext` class.
    5. For `int` key type, update the definition of the `ApplicationDbContext` class by  changing the base class definition section to add `int` as generic key parameter after `ApplicationUser` generic parameter like `: IdentityDbContext<ApplicationUser, int>`.
4. In **Controllers/AccountController.cs** file, if you use `int` as key type-
    1. In `ConfirmEmail(string userId, string code)` method, change the `userId` parameter type to `int` and change the `if` condition from `userId == null`to `userId == default(int)`.
    2. In `SendCode(string returnUrl, bool rememberMe)` method, change the `if` condition from `userId == null`to `userId == default(int)`.
5. In **Controllers/ManageController.cs** file, if you use `int` as key type-
    1. Change all `GetUserId()` method calls to `GetUserId<int>()` so that it returns **User Id** as `int`.
    2. Some of the places, **User Id** need to be passed as `string`; so, add `.ToString()` where needed.
6. Now, build the project and test it.
