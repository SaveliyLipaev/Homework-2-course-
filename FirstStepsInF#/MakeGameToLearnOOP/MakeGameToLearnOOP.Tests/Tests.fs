module MakeGameToLearnOOP.Tests

open IOpSys
open NUnit.Framework
open FsUnit
open VirusNetworkGame

[<Test>]
let ``Test correct work with 100% probability`` () =
    let computers =
        [| for _i in 1 .. 6 -> (NoSecurityOs() :> IOpSys) |]

    let graph =
        [| [ 1 ]
           [ 0 ]
           [ 3 ]
           [ 2; 4; 5 ]
           [ 3; 5 ]
           [ 3; 4 ] |]

    let logger = LoggerOneProbability()

    let simulator =
        VirusNetworkGame(computers, graph, logger)

    simulator.Start() |> ignore

    logger.IsSomeComputersAlive() |> should be False

[<Test>]
let ``Test correct work with probability = 0`` () =
    let computers =
        [| for _i in 1 .. 6 -> (InvincibleOs() :> IOpSys) |]

    let graph =
        [| [ 1 ]
           [ 0 ]
           [ 3 ]
           [ 2; 4; 5 ]
           [ 3; 5 ]
           [ 3; 4 ] |]

    let expectedState = [| for _i in 1 .. 6 -> false |]

    let simulator =
        VirusNetworkGame(computers, graph, LoggerImpossibleProbability(6))

    try
        simulator.Start() |> should be True
    with
    | Failure ("Success!") -> Assert.Pass()
    | Failure ("Failure!") -> Assert.Fail("Probability is 0, but some computers were infected; HOW?!")

    Assert.Fail("The simulation stopped, but it should not!")