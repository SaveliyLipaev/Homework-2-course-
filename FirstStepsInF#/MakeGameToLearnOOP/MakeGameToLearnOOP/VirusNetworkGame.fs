module MakeGameToLearnOOP.VirusNetworkGame

open IOpSys
open ILogger

/// Represents game state
type VirusNetworkGame(computers: IOpSys array, graph: int list array, logger: ILogger) =
    let rnd = System.Random()

    /// Starts the game
    member this.Start() =
        /// Carries out the next step
        let rec makeStep (handleQueue: int list) (used: bool array) =
            match handleQueue with
            | head :: _ when not used.[head]
                             && (rnd.NextDouble() < computers.[head].InfectionRisk) ->
                used.[head] <- true
                used |> logger.LogState |> ignore

                makeStep (handleQueue @ graph.[head]) used
            | head :: _ when not used.[head] ->
                used |> logger.LogState |> ignore
                makeStep (handleQueue @ [ head ]) used
            | _ :: tail -> makeStep tail used
            | [] -> used

        /// Running infection process in each computer
        let rec makeStepForEach i (state: bool array) =
            if i = computers.Length then
                true
            else
                match state.[i] with
                | true -> makeStepForEach (i + 1) state
                | false ->
                    let newState = makeStep [ i ] state
                    makeStepForEach (i + 1) newState

        let startState =
            [| for _index in 1 .. computers.Length -> false |]

        makeStepForEach 0 startState
