module MakeGameToLearnOOP.Main

open IOpSys
open AvailableOsTypes
open ConsoleLogger
open System
open VirusNetworkGame

module Main =
    /// Reads description of computers from the console
    let rec readComputers number accComputers =
        if number <= 0 then
            accComputers
        else
            printf "Enter #%d computer type [Windows, Linux]: " number
            let computerType = Console.ReadLine()

            match (computerType.ToLower()) with
            | "windows" -> readComputers (number - 1) ((OsWindows() :> IOpSys) :: accComputers)
            | _ -> readComputers (number - 1) ((OsLinux() :> IOpSys) :: accComputers)

    /// Reads initial state of local network from the console
    let rec readGraph number accGraph =
        if number <= 0 then
            accGraph
        else
            printf "Enter #%d neighbors (press Enter if 0 neighbors): " number
            let neighborsString = Console.ReadLine()
            if neighborsString.Length > 0 then
                let neighbors =
                    neighborsString.Split [| ' ' |]
                    |> List.ofArray
                    |> List.map (fun x -> (int x) - 1)

                readGraph (number - 1) (neighbors :: accGraph)
            else
                readGraph (number - 1) ([] :: accGraph)


    printf "Enter computer number: "
    let computerNumber = Console.ReadLine() |> int

    if computerNumber <= 0 then
        printfn "Computer number shouldn't be <= 0"
    else
        let computers =
            readComputers computerNumber [] |> Array.ofList
        let graph =
            readGraph computerNumber [] |> Array.ofList
        let logger = ConsoleLogger()
        let game = VirusNetworkGame(computers, graph, logger)

        game.Start() |> ignore
