CREATE TABLE [dbo].[SystemConfiguration]
(
	[Key] NVARCHAR(100) NOT NULL PRIMARY KEY,
	[Value] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(500) NULL,
	[DataType] NVARCHAR(20) NOT NULL DEFAULT 'String',
	[Modified] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[ModifiedBy] INT NULL,
	CONSTRAINT FK_Config_User FOREIGN KEY ([ModifiedBy])
		REFERENCES [dbo].[User] ([Id]),
	CONSTRAINT CK_Config_DataType CHECK ([DataType] IN ('String', 'Int', 'Bool', 'DateTime', 'Decimal'))
)