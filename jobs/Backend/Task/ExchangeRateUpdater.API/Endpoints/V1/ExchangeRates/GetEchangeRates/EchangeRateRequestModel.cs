using ExchangeRateUpdater.API.Validation;
using ExchangeRateUpdater.Domain.Core.UseCases.Queries.GetExchangeRates;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.API.Endpoints.V1.ExchangeRates.GetExchangeRate
{
	public class ExchangeRateRequestModel
	{
		/// <summary>
		/// Target currency
		/// </summary>
		[Required]
		[FromQuery]
		[ValidCurrency]
		public string TargetCurrency { get; set; }

		/// <summary>
		/// Year-Month of the exchange value
		/// </summary>
		[FromQuery]
		public DateTime? Date { get; set; } 

		public GetExchangeRateRequest ToQueryRequest()
		{
			return new GetExchangeRateRequest
			{
				TargetCurrency = TargetCurrency.ToUpper(),
				Date = Date,
			};
		}
	}
}
