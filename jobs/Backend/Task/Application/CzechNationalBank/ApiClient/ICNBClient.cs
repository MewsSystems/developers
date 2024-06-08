using Application.CzechNationalBank.ApiClient.Dtos;

namespace Application.CzechNationalBank.ApiClient
{
    public interface ICNBClient : IDisposable
    {
        Task<CNBExRateDailyResponseDto?> GetExRateDailies();
    }
}