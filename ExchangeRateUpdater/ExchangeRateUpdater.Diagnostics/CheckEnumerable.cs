using System.Collections;

namespace ExchangeRateUpdater.Diagnostics {
	public partial class Check {
		public static bool IsNullOrEmpty(IEnumerable value) {
			return Check.IsNull(value) || !value.GetEnumerator().MoveNext();
		}
	}
}
