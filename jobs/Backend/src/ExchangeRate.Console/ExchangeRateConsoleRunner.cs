using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using ExchangeRate.Client.Cnb;
using ExchangeRate.Console.Abstraction;
using ExchangeRate.Console.Models.Enums;
using ExchangeRate.Service.Abstract;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Console
{
	public class ExchangeRateConsoleRunner : IExchangeRateConsoleRunner
	{
		private readonly IExchangeRateService _exchangeRateService;
		private readonly ILogger<ExchangeRateConsoleRunner> _logger;

		public ExchangeRateConsoleRunner(IExchangeRateService exchangeRateService, ILogger<ExchangeRateConsoleRunner> logger)
		{
			_exchangeRateService = exchangeRateService;
			_logger = logger;
		}

		public async Task<ExitCode> ExecuteGetExchangeRates(string[] args)
		{
			try
			{
				ShowResult(await GetExchangeRates(GetApiType(args)));
				return ExitCode.Success;
			}
			catch (ArgumentNullException e)
			{
				_logger.LogCritical(e, "Argument is null or empty");
			}
			catch (EmptyResultSetException e)
			{
				_logger.LogCritical(e, "Empty result");
			}
			catch (HttpRequestException e)
			{
				_logger.LogCritical(e, "Http request problem");
			}
			catch (ParsingException e)
			{
				_logger.LogCritical(e, "XML parsing problem");
			}
			catch (ValidationException e)
			{
				_logger.LogWarning(e, "Validation problem");
			}
			catch (Exception e)
			{
				_logger.LogCritical(e, "Something went wrong");
			}

			return ExitCode.Error;
		}

		#region Private members


		private async Task<List<string>?> GetExchangeRates(CnbConstants.ApiType apiType)
		{
			Stopwatch sw = Stopwatch.StartNew();
			var result = await _exchangeRateService.GetExchangeRates(apiType);
			sw.Stop();
			_logger.LogDebug("Data fetched: {Elapsed} ms", sw.ElapsedMilliseconds);

			return result;
		}

		private CnbConstants.ApiType GetApiType(string[] args)
		{
			string stringApiType = string.Empty;
			if (args.Length > 0)
			{
				stringApiType = args[0];
			}

			return GetCnbApiType(stringApiType);
		}

		private void ShowResult(IEnumerable<string>? result)
		{
			if (result != null)
			{
				StringBuilder stringBuilderResult = new();
				foreach (var item in result)
				{
					stringBuilderResult.AppendLine(item);
				}
				System.Console.WriteLine("**********\n* Result *\n**********");
				System.Console.WriteLine(stringBuilderResult.ToString());
			}
			else
			{
				_logger.LogWarning("Null result set");
			}
		}

		private CnbConstants.ApiType GetCnbApiType(string inputApiType)
		{
			CnbConstants.ApiType apiType = CnbConstants.ApiType.CnbXml;
			if (!string.IsNullOrWhiteSpace(inputApiType))
			{
				if (!Enum.TryParse(inputApiType, false, out apiType))
				{
					_logger.LogWarning("Wrong parameter {InputApiType} . Using default api: {ApiType} . Try calling application with parameter : {EnumValues}", inputApiType, apiType, Enum.GetNames(typeof(CnbConstants.ApiType)).ToList());
				}
				else
				{
					_logger.LogInformation("Using api: {ApiType} . ", apiType);
				}
			}
			else
			{
				_logger.LogInformation("Using default api: {ApiType} . ", apiType);
			}
			return apiType;
		}

		#endregion privatemembers
	}
}
