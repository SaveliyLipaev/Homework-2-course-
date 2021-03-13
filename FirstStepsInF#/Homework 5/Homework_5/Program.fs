module Homework_5.Main

open OperatingSystemTypes
open Computer
open VirusNetworkGame

[<EntryPoint>]
let main argv =
    let testComputers =
        [| new Computer(0, new OsWindows(), true)
           new Computer(1, new OsLinux(), false)
           new Computer(2, new OsWindows(), false)
           new Computer(3, new OsLinux(), false)
           new Computer(4, new OsWindows(), false)
           new Computer(5, new OsWindows(), false)
           new Computer(6, new OsLinux(), false)
           new Computer(7, new OsWindows(), false) |]

    let testConnections =
        [| [ 1 ]
           [ 0; 2 ]
           [ 1 ]
           [ 4 ]
           [ 3; 5 ]
           [ 4; 6; 7 ]
           [ 5 ]
           [ 5 ] |]

    let game =
        VirusNetworkGame(testComputers, testConnections)

    game.Start(200) |> ignore
    printfn "Test Ended"
    0
