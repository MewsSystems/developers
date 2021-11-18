using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IApiClientService
    {
        Task<string> ConsumeEndpoint(string url);
    }
}
