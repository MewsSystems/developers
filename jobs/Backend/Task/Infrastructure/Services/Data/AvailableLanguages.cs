using Domain.Abstractions.Data;

namespace Infrastructure.Services.Data;

public class AvailableLanguages : IAvailableLangauges
{
    private IEnumerable<string> Languages =
    [
        "EN",
        "CZ"
    ];

    public IEnumerable<string> GetLanguages()
        => Languages;
}
