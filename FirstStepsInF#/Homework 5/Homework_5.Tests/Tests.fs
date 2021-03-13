module Homework_5.Tests

open NUnit.Framework
open OperatingSystemTypes
open VirusNetworkGame
open Computer
open FsUnit

[<Test>]
let ``Test correct work with 100% probability`` () =
    let testComputers =
        [| new Computer(0, new NoSecurityOs(), true)
           new Computer(1, new NoSecurityOs(), false)
           new Computer(2, new NoSecurityOs(), false)
           new Computer(3, new NoSecurityOs(), false)
           new Computer(4, new NoSecurityOs(), false)
           new Computer(5, new NoSecurityOs(), false)
           new Computer(6, new NoSecurityOs(), false)
           new Computer(7, new NoSecurityOs(), false) |]

    let testConnections =
        [| [ 1 ]
           [ 0; 2 ]
           [ 1; 3 ]
           [ 4; 2 ]
           [ 3; 5 ]
           [ 4; 6; 7 ]
           [ 5 ]
           [ 5 ] |]

    let game =
        VirusNetworkGame(testComputers, testConnections)

    game.Start(100) |> ignore

    for computer in testComputers do
        computer.Status |> should be True

[<Test>]
let ``Test correct work with probability 0.0`` () =
    let testComputers =
        [| new Computer(0, new OsLinux(), true)
           new Computer(1, new InvincibleOs(), false)
           new Computer(2, new InvincibleOs(), false)
           new Computer(3, new InvincibleOs(), false)
           new Computer(4, new InvincibleOs(), false)
           new Computer(5, new InvincibleOs(), false)
           new Computer(6, new InvincibleOs(), false)
           new Computer(7, new InvincibleOs(), false) |]

    let testConnections =
        [| [ 1 ]
           [ 0; 2 ]
           [ 1; 3 ]
           [ 4; 2 ]
           [ 3; 5 ]
           [ 4; 6; 7 ]
           [ 5 ]
           [ 5 ] |]

    let game =
        VirusNetworkGame(testComputers, testConnections)

    game.Start(100) |> ignore

    for i = 1 to testComputers.Length - 1 do
        testComputers.[i].Status |> should be False


