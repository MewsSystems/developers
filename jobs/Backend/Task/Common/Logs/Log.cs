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

		public void Info(string message)
		{
			if (_log?.IsInfoEnabled ?? false)
				_log.Info(message);
		}

		public void Error(string message)
		{
			if (_log?.IsErrorEnabled ?? false)
				_log.Error(message);
		}

		public void Warning(string message)
		{
			if (_log?.IsWarnEnabled ?? false)
				_log.Warn(message);
		}

		public void Debug(string message)
		{
			if (_log?.IsDebugEnabled ?? false)
				_log.Debug(message);
		}

		#endregion

		#region Private methods

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
	}
}
