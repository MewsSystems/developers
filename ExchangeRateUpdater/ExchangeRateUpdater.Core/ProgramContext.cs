namespace ExchangeRateUpdater.Core {
	using System;
	using System.Collections.Generic;
	using System.Dynamic;
	using System.Linq;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Core.Dynamic;
	using ExchangeRateUpdater.Core.Unity.Extensions;
	using ExchangeRateUpdater.Diagnostics;
	using global::Unity;

	public class ProgramContext : IDisposable {
		private IList<string> _stepNames;

		public ProgramContext(IUnityContainer container) {
			Container = Ensure.IsNotNull(container);
		}

		public bool IsInitialized { get; private set; }
		public IUnityContainer Container { get; private set; }
		public bool Continue { get; private set; } = true;
		public KeyValuePair<string, IProgramStep> CurrentStep { get; private set; }
		public dynamic TempData { get; } = new TempData();

		public bool ValidateInput(ConsoleKey key, IEnumerable<ConsoleKey> allowedKeys) {
			var result = Ensure.IsNotNull(allowedKeys).Any(ak => ak == key);

			return result;
		}

		public ConsoleKey ReadKey() {
			var key = Console.ReadKey();

			return key.Key;
		}

		public void Initialize() {
			_stepNames = Container
				.GetRegistrationsForType<IProgramStep>(true)
				.Select(r => r.Name)
				.OrderBy(n => n)
				.ToList();

			if(_stepNames.Count < 1) {
				Quit();				
			} else {
				SetCurrentStep(_stepNames.First());
				IsInitialized = true;
			}
		}

		public async Task<bool> StartAsync() {
			return await CurrentStep.Value.RunAsync(this);
		}

		public void Next() {
			var nextStep = Container
				.GetRegistrationsForType<IProgramStep>(true)
				.SkipWhile(r => r.Name == CurrentStep.Key)
				.FirstOrDefault();

			if(nextStep == null) {
				Quit();
			} else {
				SetCurrentStep(nextStep.Name);
			}
		}

		public void Quit() {
			Continue = false;
		}

		private void SetCurrentStep(string stepName) {
			CurrentStep = new KeyValuePair<string, IProgramStep>(stepName, Container.Resolve<IProgramStep>(stepName));
		}

		public void WriteMessage(string message, bool prependEmptyLine = true, bool appendEmptyLine = true) {
			if (prependEmptyLine)
				Console.WriteLine();

			Console.WriteLine(message);

			if (appendEmptyLine)
				Console.WriteLine();
		}

		public void WriteMessageRange(IEnumerable<string> messages) {
			Console.WriteLine();

			foreach (var message in messages) {
				WriteMessage(message, false, false);
			}

			Console.WriteLine();
		}

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
