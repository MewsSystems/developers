using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExchangeRateUpdater.Api.Binders;

public class CommaSeparatedQueryBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

        if (string.IsNullOrWhiteSpace(value))
        {
            bindingContext.Result = ModelBindingResult.Success(new List<string>());
            return Task.CompletedTask;
        }

        var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(v => v.Trim().ToUpperInvariant())
                          .ToList();

        bindingContext.Result = ModelBindingResult.Success(values);
        return Task.CompletedTask;
    }
}
