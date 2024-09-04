using Application.CzechNationalBank.ApiClient.Dtos;

namespace Application.CzechNationalBank.ApiClient
{
    public interface ICNCApiClient : IDisposable
    {
        Task<CNBExRateDailyResponseDto?> GetExRateDailies();
    }
}