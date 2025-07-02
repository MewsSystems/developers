using ExchangeRateUpdater.API.Validation;
using ExchangeRateUpdater.Core.UseCases.Queries.GetExchangeRates;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.API.Endpoints.V1.ExchangeRates.Models
{
	public class GetExchangeRateModel
	{
		/// <summary>
		/// Currency in ISO format.
		/// </summary>
		[Required]
		[FromQuery]
		[ValidCurrency]
		public string TargetCurrency { get; set; }

		/// <summary>
		/// Month in ISO format (yyyy-MM).
		/// </summary>
		[FromQuery]
		public DateTime? Date { get; set; }

		/// <summary>
		/// Convert the model the query request.
		/// </summary>
		/// <returns></returns>
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
