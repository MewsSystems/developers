I refactored the provider into a real implementation by introducing a clear separation 
between domain models and API responses. The provider is now injected via the .NET Generic Host using AddHttpClient, 
which centralizes HTTP configuration (base address, timeout, headers) and improves maintainability. 

Exchange rates are fetched asynchronously from the CNB daily endpoint, deserialized with System.Text.Json, 
and normalized for currencies quoted per multiple units (rate divided by amount). 

Finally, results are filtered to only include currencies requested by the caller from what the source provides.