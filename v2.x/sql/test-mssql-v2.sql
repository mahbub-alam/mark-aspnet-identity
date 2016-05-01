USE aspnet_identity_test;

-- Drop tables
DROP TABLE
UserLogin,
UserRole,
UserClaim,
Role,
[User];

-- Create tables
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
	EmailConfirmed BIT NOT NULL DEFAULT 0, 
	PhoneNumber NVARCHAR (16) NULL, 
	PhoneNumberConfirmed BIT NOT NULL DEFAULT 0, 
	TwoFactorEnabled BIT NOT NULL DEFAULT 0, 
	LockoutEnabled BIT NOT NULL DEFAULT 0, 
	LockoutEndDateUtc DATETIME NULL, 
	AccessFailedCount INT NOT NULL DEFAULT 0, 
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

CREATE TABLE UserRole
(
	UserId INT NOT NULL, 
	RoleId INT NOT NULL,
	CONSTRAINT FK_UserRole_UserId FOREIGN KEY (UserId) REFERENCES [User] (Id), 
	CONSTRAINT FK_UserRole_RoleId FOREIGN KEY (RoleId) REFERENCES Role (Id), 
	CONSTRAINT PK_UserRole_CompId PRIMARY KEY (UserId, RoleId)
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

-- Initial data
INSERT INTO Role (Name) VALUES ('System Administrator');
INSERT INTO Role (Name) VALUES ('Application Admin');
INSERT INTO Role (Name) VALUES ('General User');
INSERT INTO Role (Name) VALUES ('Report User');
INSERT INTO Role (Name) VALUES ('Test User');

INSERT INTO [User] (UserName, PasswordHash, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEnabled, LockoutEndDateUtc, SecurityStamp) 
			VALUES ('john_doe', '10b8b801768fc884a43f5f319313dd8e', 'john_doe@exmaple.com', 1, '+12142521234', 1, 2, 1, '2016-04-20 00:00', 'fake_security_stamp');
INSERT INTO [User] (UserName) VALUES ('james_blonde');
INSERT INTO [User] (UserName) VALUES ('hannibal_lecter');

INSERT INTO UserLogin (LoginProvider, ProviderKey, UserId) VALUES ('Microsoft', 'A516261B-0AE5-4D7F-926F-910C1A2BE51A', 1);
INSERT INTO UserLogin (LoginProvider, ProviderKey, UserId) VALUES ('Github', '7584027F-D8EA-4EFB-9852-0B447B98C752', 1);
INSERT INTO UserLogin (LoginProvider, ProviderKey, UserId) VALUES ('Yahoo', '341113DF-9091-4750-A2DD-9A1B56A8FCDC', 1);
INSERT INTO UserClaim (UserId, ClaimType, ClaimValue) VALUES (1, 'Edit Log', 'Yes');
INSERT INTO UserClaim (UserId, ClaimType, ClaimValue) VALUES (1, 'Full Edit', 'Yes');
INSERT INTO UserClaim (UserId, ClaimType, ClaimValue) VALUES (1, 'Delete All', 'Yes');
INSERT INTO UserRole (UserId, RoleId) VALUES (1, 1);
INSERT INTO UserRole (UserId, RoleId) VALUES (1, 2);
INSERT INTO UserRole (UserId, RoleId) VALUES (1, 3);
