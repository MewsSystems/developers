using System.Reflection;
using Microsoft.Extensions.Localization;

namespace ExchangeRateUpdater.Localization;

public class Localizer
{
  private readonly Dictionary<string, IStringLocalizer> _localizers;
  private const string DefaultLocalizer = "Translations";
  private const string TranslatedResourceNotFound = "Translated resource not found.";
  private readonly IStringLocalizerFactory _localizerFactory;
  private static readonly object Lock = new object();

  public Localizer(IStringLocalizerFactory localizerFactory)
  {
    _localizerFactory = localizerFactory;
    _localizers = new Dictionary<string, IStringLocalizer>();
  }
  
  public string this[string name, params object[] arguments] => Get(name, arguments);

  public string Get(string name, params object[] arguments)
  {
    IStringLocalizer localizer = GetLocalizer(DefaultLocalizer);
    LocalizedString localizedString = localizer[name, arguments];
    return localizedString.ResourceNotFound ? TranslatedResourceNotFound + $" - ({name})" : localizedString.Value;
  }

  private IStringLocalizer GetLocalizer(string resourceFileName)
  {
    if (!string.IsNullOrWhiteSpace(resourceFileName))
    {
      lock(Lock)
      {
        if (!_localizers.ContainsKey(resourceFileName))
        {
          string? assemblyFullName = GetType()?.GetTypeInfo()?.Assembly?.FullName;
          if (!string.IsNullOrWhiteSpace(assemblyFullName))
          {
            IStringLocalizer newLocalizer = _localizerFactory.Create("Translations", new AssemblyName(assemblyFullName).Name ?? throw new InvalidOperationException());
            _localizers.Add(resourceFileName, newLocalizer);
          }
        }
      }

      return _localizers[resourceFileName];
    }

    throw new Exception("Resource file not found.");
  }

  public bool IsTranslationFound(string translation)
  {
    return !string.IsNullOrWhiteSpace(translation) && !translation.Equals(TranslatedResourceNotFound, StringComparison.InvariantCultureIgnoreCase);
  }
}