-- ASP.NET Identity v2.x MYSQL Database Script

USE aspnet_identity_test;

CREATE TABLE Role
(
	Id INT(10) NOT NULL AUTO_INCREMENT,
	Name VARCHAR (64) NOT NULL,
	UNIQUE INDEX (Name),
	PRIMARY KEY (Id)
);

CREATE TABLE `User`
(
	Id INT(10) NOT NULL AUTO_INCREMENT,
	UserName VARCHAR (64) NOT NULL,
	SecurityStamp VARCHAR (255) NULL, 
	PasswordHash VARCHAR (255) NULL, 
	Email VARCHAR (64) NULL, 
	EmailConfirmed TINYINT(1) NOT NULL, 
	PhoneNumber VARCHAR (16) NULL, 
	PhoneNumberConfirmed TINYINT(1) NOT NULL, 
	TwoFactorEnabled TINYINT(1) NOT NULL, 
	LockoutEnabled TINYINT(1) NOT NULL, 
	LockoutEndDateUtc DATETIME NULL, 
	AccessFailedCount INT(10) NOT NULL, 
	UNIQUE INDEX (UserName),
	PRIMARY KEY (Id)
);

CREATE TABLE UserLogin
(
	LoginProvider VARCHAR (128) NOT NULL,
	ProviderKey VARCHAR (128) NOT NULL, 
	UserId INT(10) NOT NULL, 
	FOREIGN KEY (UserId) REFERENCES `User` (Id),
	PRIMARY KEY (LoginProvider, ProviderKey, UserId)
);

CREATE TABLE UserRole
(
	UserId INT(10) NOT NULL, 
	RoleId INT(10) NOT NULL,
	FOREIGN KEY (UserId) REFERENCES `User` (Id), 
	FOREIGN KEY (RoleId) REFERENCES Role (Id), 
	PRIMARY KEY (UserId, RoleId)
);

CREATE TABLE UserClaim
(
	Id INT(10) NOT NULL AUTO_INCREMENT, 
	UserId INT(10) NOT NULL, 
	ClaimType VARCHAR (255) NULL,
	ClaimValue VARCHAR (255) NULL, 
	FOREIGN KEY (UserId) REFERENCES `User` (Id), 
	PRIMARY KEY (Id)
);
