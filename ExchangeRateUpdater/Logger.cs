using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	/// <summary>
	/// Simple logger
	/// </summary>
	public static class Logger
	{
		private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

		public static void Trace(string message)
		{
			_logger.Trace(message);
		}
	}
}
