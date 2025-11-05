CREATE TABLE [dbo].[ExchangeRateProviderConfiguration]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProviderId] INT NOT NULL,
	[SettingKey] NVARCHAR(100) NOT NULL,
	[SettingValue] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(500) NULL,
	[Created] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[Modified] DATETIMEOFFSET NULL,
	CONSTRAINT FK_ProviderConfig_Provider FOREIGN KEY ([ProviderId])
		REFERENCES [dbo].[ExchangeRateProvider] ([Id]) ON DELETE CASCADE,
	CONSTRAINT UQ_Provider_Setting UNIQUE ([ProviderId], [SettingKey])
)
