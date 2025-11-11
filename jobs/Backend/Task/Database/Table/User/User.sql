CREATE TABLE [dbo].[User]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(512) NOT NULL,
    [FirstName] NVARCHAR(255) NOT NULL,
    [LastName] NVARCHAR(255) NOT NULL,
    [Role] NVARCHAR(255) NOT NULL,
    CONSTRAINT CK_User_Role CHECK ([Role] IN ('Admin', 'Consumer'))
)
