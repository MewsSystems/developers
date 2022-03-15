module Domain

/// Iso 4217 code
type Currency = {Code : string}
type ExchangeRate = {Source: Currency; Target : Currency ; Rate : decimal}    
type ExchangeRateProvider( prefetchedRates) = 
    let rateLookup = 
        prefetchedRates 
        |> List.map (fun r -> r.Target, r) 
        |> Map.ofList

    /// Returns exchange rates among the specified currencies that are defined by the source. Missing currencies are skipped
    /// I personally do not like the strategy of "If the source does not provide some of the currencies, ignore them" , but OK
    member _.GetExchangeRates currencies  =
        currencies 
        |> Seq.map (fun c -> Map.tryFind c rateLookup)
        |> Seq.choose id  
