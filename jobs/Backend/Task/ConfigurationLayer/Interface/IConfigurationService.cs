using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLayer.Interface
{
    public interface IConfigurationService
    {
        Task<string> GetValueAsync(string key, string defaultValue = "");
        Task<T?> GetValueAsync<T>(string key, T? defaultValue = default);
    }
}
