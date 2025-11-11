/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- BCrypt hash for password "simple" (cost factor 11)
-- This is a REAL BCrypt hash generated for "simple"
DECLARE @PasswordHash NVARCHAR(512) = '$2a$11$i0UZQX8ZlJ2yY6DuZ4Y0HO0efKgQjvFMeMFv7wYJUVSIqmuzKSBJ.';

-- Insert Admin User
IF NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE Email = 'admin@example.com')
BEGIN
    INSERT INTO [dbo].[User] ([Email], [PasswordHash], [FirstName], [LastName], [Role])
    VALUES (
        'admin@example.com',
        @PasswordHash,
        'Admin',
        'User',
        'Admin'
    );
    PRINT '  ✓ Admin user created';
END
ELSE
BEGIN
    PRINT '  ⚠ Admin user already exists';
END

-- Insert Consumer User  
IF NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE Email = 'consumer@example.com')
BEGIN
    INSERT INTO [dbo].[User] ([Email], [PasswordHash], [FirstName], [LastName], [Role])
    VALUES (
        'consumer@example.com',
        @PasswordHash,
        'Consumer',
        'User',
        'Consumer'
    );
    PRINT '  ✓ Consumer user created';
END
ELSE
BEGIN
    PRINT '  ⚠ Consumer user already exists';
END