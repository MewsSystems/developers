namespace Common.Logs
{
	/// <summary>
	/// Setting file for Log4net logs.
	/// </summary>
	public static class LogSettings
	{
		// Log file name.
		public static readonly string LogFileName = "Log.txt";
		// Log line entry pattern.
		public const string ConversionPattern = "%date %level: %message%newline";
		// Max. number of log files, which are stored on file system.
		public static int MaxSiteRollBackups = 5;
		// Max. log file size.
		public const string MaximumFileSize = "20MB";
	}
}