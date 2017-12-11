namespace ExchangeRateUpdater.Diagnostics {

	using System;

	public partial class Throw {
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="paramName">Optional parameter with parameter name used to describe ArgumentNullException paramName argument</param>
		/// <returns>Returns passed object if not null, otherwise throws ArgumentNullException</returns>
		public static void IfNull<T>(T value, string paramName = null) {
			if (Check.IsNull(value)) {
				Throw.ArgumentNullException(paramName);
			}
		}

		internal static void ArgumentNullException(string parameterName = "(name not provided)") {
			throw new ArgumentNullException(parameterName, String.Format(ExceptionMessageResource.GenericArgumentNullExceptionMessageFormat, parameterName));
		}

		internal static void StringEmptyOrWhiteSpaceArgumentException(string paramName) {
			throw new ArgumentException(String.Format(ExceptionMessageResource.StringCannotBeNullEmptyOrWhiteSpaceArgumentExceptionMessageFormat, paramName), paramName);
		} 
	}
}
