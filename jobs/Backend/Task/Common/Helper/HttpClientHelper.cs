namespace Common.Helper
{
	/// <summary>
	/// The purpose of this HttpClientHelper class is to provide always the instance of HttpClient object - this is according to MS documentation.
	/// </summary>
	internal class HttpClientHelper
	{
		#region Properties

		public HttpClient HttpClient { get; }

		#endregion

		#region Singleton

		private static Lazy<HttpClientHelper> _instance = new Lazy<HttpClientHelper>(() => new HttpClientHelper());
		public static HttpClientHelper Instance => _instance.Value;

		private HttpClientHelper()
		{
			HttpClient = new HttpClient();
		}

		#endregion
	}
}
