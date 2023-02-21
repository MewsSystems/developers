using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Sources;

public sealed class FileSource : ISource
{
    private readonly string _path;

    public FileSource(string path)
    {
        _path = path;
    }

    public Task<string> GetContent()
    {
        return File.ReadAllTextAsync(_path);
    }

    public override string ToString()
    {
        return $"File: {_path}";
    }
}
