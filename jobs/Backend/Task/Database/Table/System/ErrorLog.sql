CREATE TABLE [dbo].[ErrorLog]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Timestamp] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[Severity] NVARCHAR(20) NOT NULL,
	[Source] NVARCHAR(200) NOT NULL,
	[Message] NVARCHAR(MAX) NOT NULL,
	[Exception] NVARCHAR(MAX) NULL,
	[StackTrace] NVARCHAR(MAX) NULL,
	[UserId] INT NULL,
	[AdditionalData] NVARCHAR(MAX) NULL,
	CONSTRAINT FK_ErrorLog_User FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User] ([Id]),
	CONSTRAINT CK_ErrorLog_Severity CHECK ([Severity] IN ('Info', 'Warning', 'Error', 'Critical'))
)