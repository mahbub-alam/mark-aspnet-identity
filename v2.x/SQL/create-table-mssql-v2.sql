-- ASP.NET Identity v2.x MSSQL Database Script

--USE master;
--IF EXISTS(SELECT * FROM sys.databases WHERE name='aspnet_identity_test')
--DROP DATABASE aspnet_identity_test;

--CREATE DATABASE aspnet_identity_test;

USE aspnet_identity_test;

CREATE TABLE Role
(
	Id INT NOT NULL IDENTITY,
	Name NVARCHAR (64) NOT NULL,
	CONSTRAINT UK_Role_Name UNIQUE (Name),
	CONSTRAINT PK_Role_Id PRIMARY KEY (Id)
);

CREATE TABLE [User]
(
	Id INT NOT NULL IDENTITY,
	UserName NVARCHAR (64) NOT NULL,
	SecurityStamp NVARCHAR (255) NULL, 
	PasswordHash NVARCHAR (255) NULL, 
	Email NVARCHAR (64) NULL, 
	EmailConfirmed BIT NOT NULL, 
	PhoneNumber NVARCHAR (16) NULL, 
	PhoneNumberConfirmed BIT NOT NULL, 
	TwoFactorEnabled BIT NOT NULL, 
	LockoutEnabled BIT NOT NULL, 
	LockoutEndDateUtc DATETIME NULL, 
	AccessFailedCount INT NOT NULL, 
	CONSTRAINT UK_User_UserName UNIQUE (UserName),
	CONSTRAINT PK_User_Id PRIMARY KEY (Id)
);

CREATE TABLE UserLogin
(
	LoginProvider NVARCHAR (128) NOT NULL,
	ProviderKey NVARCHAR (128) NOT NULL, 
	UserId INT NOT NULL, 
	CONSTRAINT FK_UserLogin_UserId FOREIGN KEY (UserId) REFERENCES [User] (Id),
	CONSTRAINT PK_UserLogin_CompId PRIMARY KEY (LoginProvider, ProviderKey, UserId)
);

CREATE TABLE UserClaim
(
	Id INT NOT NULL IDENTITY, 
	UserId INT NOT NULL, 
	ClaimType NVARCHAR (255) NULL,
	ClaimValue NVARCHAR (255) NULL, 
	CONSTRAINT FK_UserClaim_UserId FOREIGN KEY (UserId) REFERENCES [User] (Id), 
	CONSTRAINT PK_UserClaim_Id PRIMARY KEY (Id)
);