CREATE TABLE [dbo].[ExchangeRate]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProviderId] INT NOT NULL,
	[BaseCurrencyId] INT NOT NULL,
	[TargetCurrencyId] INT NOT NULL,
	[Multiplier] INT NOT NULL,
	[Rate] DECIMAL(19, 6) NOT NULL,
	[ValidDate] DATE NOT NULL,
	[Created] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[Modified] DATETIMEOFFSET NULL,
	CONSTRAINT FK_ExchangeRate_Provider FOREIGN KEY ([ProviderId])
		REFERENCES [dbo].[ExchangeRateProvider] ([Id]),
	CONSTRAINT FK_ExchangeRate_BaseCurrency FOREIGN KEY ([BaseCurrencyId])
        REFERENCES [dbo].[Currency] ([Id]),
	CONSTRAINT FK_ExchangeRate_TargetCurrency FOREIGN KEY ([TargetCurrencyId])
        REFERENCES [dbo].[Currency] ([Id]),
	CONSTRAINT CK_Rate_Positive CHECK ([Rate] > 0),
	CONSTRAINT CK_Multiplier_Positive CHECK ([Multiplier] > 0),
	CONSTRAINT CK_ValidDate_NotFuture CHECK ([ValidDate] <= CAST(GETDATE() AS DATE)),
	CONSTRAINT CK_Different_Currencies CHECK ([BaseCurrencyId] <> [TargetCurrencyId]),
	CONSTRAINT UQ_Rate_Provider_Date UNIQUE ([ProviderId], [BaseCurrencyId], [TargetCurrencyId], [ValidDate])
)
