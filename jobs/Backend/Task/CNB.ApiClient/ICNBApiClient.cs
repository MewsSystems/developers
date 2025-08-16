namespace CNB.ApiClient;

public interface ICNBApiClient
{
    Task<ExratesDailyResponse> GetDailyExrates(DateOnly date, CancellationToken cancellationToken);
}
