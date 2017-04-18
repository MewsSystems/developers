using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.BankConnection
{
	class BankConnector: IBankConnector, IExchangeDataProvider
	{
		private bool m_Connected = false;
		private static string URL = "http://api.fixer.io/";

		/// <summary>
		/// Connect to API
		/// </summary>
		/// <returns>True if connected</returns>
		private void SetRequestData(HttpWebRequest request, string data)
		{
			var encoding = new ASCIIEncoding();
			byte[] byte1 = encoding.GetBytes(data);
			request.ContentLength = data.Length;
			var stream = request.GetRequestStream();
			stream.Write(byte1, 0, byte1.Length);
			stream.Close();
		}

		public bool Connect()
		{
			var url = URL;
			var request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = "GET";
			request.Accept = "application/json";
			request.ContentType = "application/json";
			var response = (HttpWebResponse)request.GetResponse();
			m_Connected = response.ContentLength > 0;
			return m_Connected;
		}

		/// <summary>
		/// Close connection
		/// </summary>
		public void Disconnect()
		{
			Console.WriteLine("nope");
		}

		/// <summary>
		/// Get exchange rate for currency pair
		/// </summary>
		/// <param name="code1">First currency code</param>
		/// <param name="code2">Second currency code</param>
		/// <returns>Exchange rate</returns>
		public string GetPairValue(string code1, string code2)
		{
			if (m_Connected)
			{
				string value = "";
				try
				{
					var url = URL + string.Format("latest?base={0}&symbols={1}", code1, code2);
					var request = (HttpWebRequest)HttpWebRequest.Create(url);
					request.Method = "GET";
					var response = (HttpWebResponse)request.GetResponse();
					Stream receiveStream = response.GetResponseStream();
					Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
					StreamReader readStream = new StreamReader(receiveStream, encode);
					string responseString = "";
					StringBuilder sb = new StringBuilder();
					Char[] read = new Char[256];
					int count = readStream.Read(read, 0, 256);
					while (count > 0)
					{
						String str = new String(read, 0, count);
						sb.Append(str);
						count = readStream.Read(read, 0, 256);
					}
					responseString = sb.ToString();
					response.Close();
					readStream.Close();

					string marker = "rates\":{\"" + code2 + "\":";
					if (responseString.Contains(marker))
					{
						value = responseString.Substring(responseString.IndexOf(marker) + marker.Length);
						value = value.TrimEnd('}');
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error @ pair [{0}/{1}]:\r\n", code1, code2, ex.Message);
				}
				return value;
			}
			else
				throw new Exception("Disconnected!");
		}
	}
}
