// Learn more about F# at http://fsharp.org

open System
open Taks_1
open Taks_2

[<EntryPoint>]
let main argv =
    printfn "%A" (findMin [132155552;2353;155535;1342354235])
    let sq = printableAsciiSquare(4)
    printfn "%A" (sq)
    0