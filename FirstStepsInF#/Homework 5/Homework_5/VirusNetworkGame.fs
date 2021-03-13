module Homework_5.VirusNetworkGame

open Computer

/// Represents game state
type VirusNetworkGame(computers: array<Computer>, connected: array<list<int>>) =
    let rnd = System.Random()

    let isConnectedToInfected id =
        connected.[id]
        |> List.exists (fun id -> computers.[id].Status)

    /// Prints status of all computers in the network
    let printGameState () =
        for computer in computers do
            computer.PrintStatus()
        printfn "\n"

    /// Starts the game
    member this.Start(maxSteps) =
        /// Counter of steps in the game
        let mutable stepsCounter = 0

        let rec makeStep (queue: list<int>) =
            let id = queue.Head
            let computer = computers.[id]

            let predicate =
                not computer.Status
                && isConnectedToInfected id
                && (rnd.NextDouble() <= computer.Risk)

            if stepsCounter < maxSteps then
                if predicate then
                    computer.Infect()
                    printGameState ()
                    stepsCounter <- stepsCounter + 1
                    makeStep (queue @ connected.[id])
                elif not computer.Status then
                    printGameState ()
                    stepsCounter <- stepsCounter + 1
                    makeStep (queue @ [ id ])
                elif not queue.Tail.IsEmpty then
                    makeStep queue.Tail

        let rec makeStepForEach index =
            if index = computers.Length then
                true
            else
                match computers.[index].Status with
                | true -> makeStepForEach (index + 1)
                | false ->
                    makeStep [ index ]
                    makeStepForEach (index + 1)

        makeStepForEach 0
