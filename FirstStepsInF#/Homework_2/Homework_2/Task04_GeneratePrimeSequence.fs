module GeneratePrimeSequence

let primeGenerator =
    let isPrime n =
        not ({ 2 .. n - 1 } |> Seq.exists (fun x -> n % x = 0))

    Seq.initInfinite (fun x -> x + 2) |> Seq.filter isPrime