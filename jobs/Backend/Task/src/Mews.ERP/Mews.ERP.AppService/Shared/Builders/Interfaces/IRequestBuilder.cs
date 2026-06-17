using System.Collections.Specialized;

namespace Mews.ERP.AppService.Shared.Builders.Interfaces;

public interface IRequestBuilder<out T> where T : class
{
    T Build(string path, NameValueCollection parameters);
}