// Learn more about F# at http://fsharp.org

open System
open Number_1

[<EntryPoint>]
let main argv =
    printfn "%A" (countEvenNumberWithFold [1;1;1])
    printfn "%A" (countEvenNumberWithFold [1;2;1])
    printfn "%A" (countEvenNumberWithFold [1;4;6])
    printfn "%A" (countEvenNumberWithFold [1;1;1;2;3;4;5;6])
    0 // return an integer exit code
