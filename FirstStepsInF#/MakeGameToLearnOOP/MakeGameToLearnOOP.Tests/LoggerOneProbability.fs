namespace MakeGameToLearnOOP

open ILogger

type LoggerOneProbability() =
    let mutable stepN = 0
    let mutable someComputersIsNotInfected = true
    member this.IsSomeComputersAlive() = someComputersIsNotInfected

    interface ILogger with
        member this.LogState state =
            let rec prettyPrintList number list =
                match list with
                | [] -> printf "\n"
                | head :: tail ->
                    match head with
                    | true -> printfn "Computer #%d: (x_x)" number
                    | false -> printfn "Computer #%d: (◕‿◕)" number

                    prettyPrintList (number + 1) tail

            printfn "Step %d: " stepN
            prettyPrintList 1 (state |> List.ofArray)

            if stepN >= 500 then
                failwith "GAME has not stopped, but probability == 1!"
            else
                someComputersIsNotInfected <- Array.exists ((=) false) state
                stepN <- stepN + 1

            stepN
