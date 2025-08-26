using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeRateApi.Models
{
	/// <summary>
	/// Standard error response returned by the API for 4xx and 5xx outcomes.
	/// </summary>
	/// <remarks>
	/// The <see cref="Error"/> field contains a human‑readable description of the problem.
	/// Additional diagnostic data (trace id, etc.) can be appended in future without breaking compatibility.
	/// </remarks>
	[SwaggerSchema(Description = "Standard error response returned by the API for unsuccessful requests.")]
	public class ErrorResponse
	{
		/// <summary>
		/// Human readable error message describing why the request failed.
		/// </summary>
		/// <example>Currency codes must be in XXX,YYY,ZZZ format with 3-letter codes</example>
		[SwaggerSchema(Description = "Human readable error message describing why the request failed.")]
		public required string Error { get; set; }
	}
}
