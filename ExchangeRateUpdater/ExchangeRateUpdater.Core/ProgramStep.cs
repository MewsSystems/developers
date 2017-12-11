using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core {
	public abstract class ProgramStep : IProgramStep {
		public abstract Task<bool> RunAsync(ProgramContext context);
	}
}
