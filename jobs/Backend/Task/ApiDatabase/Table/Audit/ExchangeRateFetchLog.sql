CREATE TABLE [dbo].[ExchangeRateFetchLog]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProviderId] INT NOT NULL,
	[FetchStarted] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[FetchCompleted] DATETIMEOFFSET NULL,
	[Status] NVARCHAR(20) NOT NULL DEFAULT 'Running',
	[RatesImported] INT NULL,
	[RatesUpdated] INT NULL,
	[ErrorMessage] NVARCHAR(MAX) NULL,
	[RequestedBy] INT NULL,
	[DurationMs] AS DATEDIFF(MILLISECOND, [FetchStarted], [FetchCompleted]) PERSISTED,
	CONSTRAINT FK_FetchLog_Provider FOREIGN KEY ([ProviderId])
		REFERENCES [dbo].[ExchangeRateProvider] ([Id]),
	CONSTRAINT FK_FetchLog_User FOREIGN KEY ([RequestedBy])
		REFERENCES [dbo].[User] ([Id]),
	CONSTRAINT CK_FetchLog_Status CHECK ([Status] IN ('Running', 'Success', 'Failed', 'PartialSuccess'))
)