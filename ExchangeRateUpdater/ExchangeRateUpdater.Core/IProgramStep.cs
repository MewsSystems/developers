namespace ExchangeRateUpdater.Core {
	using System.Threading.Tasks;

	public interface IProgramStep {
		Task<bool> RunAsync(ProgramContext context);
	}
}