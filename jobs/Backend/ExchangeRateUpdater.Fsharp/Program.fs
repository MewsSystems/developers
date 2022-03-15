open FSharp.Configuration
open Domain
type Settings = AppSettings<"app.config">

let testRequest = 
     [ "USD"
       "EUR"
       "CZK"
       "JPY"
       "KES"
       "RUB"
       "BRL"
       "TRY"
       "AUD" ] 
     |> List.map (fun c -> {Code = c})

let runTest rates = 
    // We do not have lifetime in a console app, so refresh rate of the rates (based on historical valuation date etc.) depends on the usage
    // As an example, this could be daily-refreshed singleton instance in a long-running app, but could also be a standalone batch file (.exe) triggered once a day...
    // (oh, and with deployments 2x / day, I guess Mews does not have long running apps :)) )
    let provider = ExchangeRateProvider rates
    provider.GetExchangeRates testRequest         
    |> Seq.iter (fun r -> printfn $"{r.Source.Code}/{r.Target.Code}={r.Rate}")

async{
    let! rates = CnbInfra.loadCnbRatesFrom Settings.CnbRatesUrl
    match rates with 
    | Ok r -> runTest r        
    | Error e -> printfn $"Fetching of CNB rates failed with: {e}"    
} |> Async.RunSynchronously