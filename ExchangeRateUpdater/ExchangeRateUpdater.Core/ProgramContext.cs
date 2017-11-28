namespace ExchangeRateUpdater.Core {
	using System;
	using ExchangeRateUpdater.Diagnostics;
	using global::Unity;

	public class ProgramContext : IDisposable {
		public ProgramContext(IUnityContainer container) {
			Container = Ensure.IsNotNull(container);
		}

		public IUnityContainer Container { get; private set; }

		#region IDisposable implementation
		private bool isDisposed = false;

		void Dispose(bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					Container.Dispose();
					Container = null;
				}

				isDisposed = true;
			}
		}

		public void Dispose() {
			Dispose(true);
		}
		#endregion
	}
}
