module CnbInfra

open FSharp.Data
open Domain
    
[<Literal>]
let sample = 
    """15.03.2022 #52
    země|měna|množství|kód|kurz
    Austrálie|dolar|1|AUD|16,320
    Brazílie|real|1|BRL|4,410"""
type private CnbRatesType = CsvProvider<Sample = sample,Separators = "|",SkipRows=1, Culture = "cs-CZ", Schema = "množství = decimal">  
    
let czk = {Code = "CZK"}
let private toDomain (row:CnbRatesType.Row) = {Source = czk; Target = {Code = row.Kód}; Rate = row.Kurz / row.Množství}
let loadCnbRatesFrom (u:System.Uri) =
    async{
        printfn $"Going to load CNB rates from {u.AbsoluteUri}"
        let! rates = CnbRatesType.AsyncLoad(u.AbsoluteUri) 
        printfn $"CNB rates fetched successfully"
        return rates.Rows |> Seq.map toDomain |> List.ofSeq }
    |> Async.CatchIntoResult
