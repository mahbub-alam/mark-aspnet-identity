USE aspnet_identity_test;

-- Drop tables
DROP TABLE IF EXISTS
UserLogin,
UserRole,
UserClaim,
Role,
`User`;

-- Create tables
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
	EmailConfirmed TINYINT(1) NOT NULL DEFAULT 0, 
	PhoneNumber VARCHAR (16) NULL, 
	PhoneNumberConfirmed TINYINT(1) NOT NULL DEFAULT 0, 
	TwoFactorEnabled TINYINT(1) NOT NULL DEFAULT 0, 
	LockoutEnabled TINYINT(1) NOT NULL DEFAULT 0, 
	LockoutEndDateUtc DATETIME NULL, 
	AccessFailedCount INT(10) NOT NULL DEFAULT 0, 
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

-- Initial data
INSERT INTO Role (Name) VALUES ('System Administrator');
INSERT INTO Role (Name) VALUES ('Application Admin');
INSERT INTO Role (Name) VALUES ('General User');
INSERT INTO Role (Name) VALUES ('Report User');
INSERT INTO Role (Name) VALUES ('Test User');

INSERT INTO `User` (UserName, PasswordHash, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, AccessFailedCount, LockoutEnabled, LockoutEndDateUtc, SecurityStamp) 
			VALUES ('john_doe', '10b8b801768fc884a43f5f319313dd8e', 'john_doe@exmaple.com', 1, '+12142521234', 1, 2, 1, '2016-04-20 00:00', 'fake_security_stamp');
INSERT INTO `User` (UserName) VALUES ('james_blonde');
INSERT INTO `User` (UserName) VALUES ('hannibal_lecter');

INSERT INTO UserLogin (LoginProvider, ProviderKey, UserId) VALUES ('Microsoft', 'A516261B-0AE5-4D7F-926F-910C1A2BE51A', 1);
INSERT INTO UserLogin (LoginProvider, ProviderKey, UserId) VALUES ('Github', '7584027F-D8EA-4EFB-9852-0B447B98C752', 1);
INSERT INTO UserLogin (LoginProvider, ProviderKey, UserId) VALUES ('Yahoo', '341113DF-9091-4750-A2DD-9A1B56A8FCDC', 1);
INSERT INTO UserClaim (UserId, ClaimType, ClaimValue) VALUES (1, 'Edit Log', 'Yes');
INSERT INTO UserClaim (UserId, ClaimType, ClaimValue) VALUES (1, 'Full Edit', 'Yes');
INSERT INTO UserClaim (UserId, ClaimType, ClaimValue) VALUES (1, 'Delete All', 'Yes');
INSERT INTO UserRole (UserId, RoleId) VALUES (1, 1);
INSERT INTO UserRole (UserId, RoleId) VALUES (1, 2);
INSERT INTO UserRole (UserId, RoleId) VALUES (1, 3);
