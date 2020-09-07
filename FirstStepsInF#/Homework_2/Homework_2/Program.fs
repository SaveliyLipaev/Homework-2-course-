open System
open CountNumbers

[<EntryPoint>]
let main argv =
    printfn "%A" (countEvenNumberWithFold [ 1; 1; 1 ])
    printfn "%A" (countEvenNumberWithFold [ 1; 2; 1 ])
    printfn "%A" (countEvenNumberWithFold [ 1; 4; 6 ])
    printfn "%A" (countEvenNumberWithFold [ 1; 1; 1; 2; 3; 4; 5; 6 ])
    0
