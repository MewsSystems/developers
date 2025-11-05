CREATE TABLE [dbo].[Currency]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Code] NVARCHAR(3) NOT NULL UNIQUE,
    [Created] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Three-letter ISO 4217 code of the currency.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Currency',
    @level2type = N'COLUMN',
    @level2name = N'Code'