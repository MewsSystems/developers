module Async
    // This would be in a common Utils package, because nobody really likes the built-in Choice* types
    let CatchIntoResult a = 
        async{
            let! result = a |> Async.Catch        
            return 
                match result with
                | Choice1Of2 success -> Ok success
                | Choice2Of2 failure -> Error failure }
