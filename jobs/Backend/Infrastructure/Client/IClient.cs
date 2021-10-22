using System.Threading.Tasks;

namespace Infrastructure.Client
{
    public interface IClient
    {
        public void InitializeClient();
        public Task<string> GetAsStringAsync(string url);
    }
}