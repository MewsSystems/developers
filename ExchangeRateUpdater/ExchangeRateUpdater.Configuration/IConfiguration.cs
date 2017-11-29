namespace ExchangeRateUpdater.Configuration {
	public interface IConfiguration<TTarget> {
		TTarget Configure(TTarget target);
	}
}
