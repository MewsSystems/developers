using Common.Exceptions;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Common.Logs
{
	/// <summary>
	/// Log4Net implementation class.
	/// </summary>
	public class Log
	{
		#region Fields
		private static ILog? _log;
		private static RollingFileAppender? _roller;
		#endregion

		#region Singleton

		private static Lazy<Log> _instance = new Lazy<Log>(() => new Log());
		public static Log Instance => _instance.Value;

		private Log()
		{
			InitAppenders();
			_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
			if (_log == null)
				throw new LogInitializationException();

			_log.Logger.Repository.Threshold = Level.All;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Logs with info level.
		/// </summary>
		/// <param name="message">Message to be logged.</param>
		public void Info(string message)
		{
			if (_log?.IsInfoEnabled ?? false)
				_log.Info(message);
		}

		/// <summary>
		/// Logs with error level.
		/// </summary>
		/// <param name="message">Message to be logged.</param>
		public void Error(string message)
		{
			if (_log?.IsErrorEnabled ?? false)
				_log.Error(message);
		}

		/// <summary>
		/// Logs with warning level.
		/// </summary>
		/// <param name="message">Message to be logged.</param>
		public void Warning(string message)
		{
			if (_log?.IsWarnEnabled ?? false)
				_log.Warn(message);
		}

		/// <summary>
		/// Logs with debug level.
		/// </summary>
		/// <param name="message">Message to be logged.</param>
		public void Debug(string message)
		{
			if (_log?.IsDebugEnabled ?? false)
				_log.Debug(message);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Appenders initialization - TXT file appender.
		/// </summary>
		private static void InitAppenders()
		{
			PatternLayout patternLayout = new PatternLayout();
			patternLayout.ConversionPattern = LogSettings.ConversionPattern;
			patternLayout.ActivateOptions();

			_roller = new RollingFileAppender
			{
				File = LogSettings.LogFileName,
				AppendToFile = true,
				ImmediateFlush = true,
				RollingStyle = RollingFileAppender.RollingMode.Date,
				Layout = patternLayout,
				MaxSizeRollBackups = LogSettings.MaxSiteRollBackups,
				MaximumFileSize = LogSettings.MaximumFileSize,
				StaticLogFileName = true,
				Threshold = Level.All
			};

			_roller.ActivateOptions();

			Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
			hierarchy.Root.AddAppender(_roller);
			hierarchy.Configured = true;
		}

		#endregion

		#region Private class.

		/// <summary>
		/// Setting file for Log4net logs.
		/// </summary>
		private class LogSettings
		{
			// Log file name.
			public const string LogFileName = "Log.txt";
			// Log line entry pattern.
			public const string ConversionPattern = "%date %level: %message%newline";
			// Max. number of log files, which are stored on file system.
			public const int MaxSiteRollBackups = 5;
			// Max. log file size.
			public const string MaximumFileSize = "20MB";
		}

		#endregion
	}
}
