module MakeGameToLearnOOP.ConsoleLogger

open ILogger

/// Logs infection state to console
type ConsoleLogger() =
    let mutable stepNumber = 0

    interface ILogger with
        member this.LogState state =
            /// Prints given list of current computer states to stdin
            let rec prettyPrintList number list =
                match list with
                | [] -> printf "\n"
                | head :: tail ->
                    match head with
                    | true -> printfn "Computer #%d: (x_x)" number
                    | false -> printfn "Computer #%d: (◕‿◕)" number

                    prettyPrintList (number + 1) tail

            printfn "Step %d: " stepNumber
            prettyPrintList 1 (state |> List.ofArray)

            stepNumber <- stepNumber + 1
            stepNumber
