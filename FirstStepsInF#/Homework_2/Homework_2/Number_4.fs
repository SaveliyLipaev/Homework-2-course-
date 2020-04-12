module Number_4

let primeGenerator =
    let isPrime number =
        let rec isPrime number sqrtNumber i =
            if number % i = 0 then false else
            if i > sqrtNumber then true else
            isPrime number sqrtNumber (i + 1)
        isPrime number (number |> float |> sqrt |> int) 2
    Seq.initInfinite (fun x -> x + 2) |> Seq.filter isPrime