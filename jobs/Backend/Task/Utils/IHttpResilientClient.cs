using System.Net.Http;
using System.Threading.Tasks;

public interface IHttpResilientClient
{
    public Task<HttpResponseMessage> DoGet(string url);
}