open System

[<EntryPoint>]
let main argv =

    let factorial n =
        if n < 0 then raise (InvalidOperationException())
        let rec countFactorial acc x =
            match x with
            | 0 -> acc
            | _ -> countFactorial (acc * x) (x - 1)
        countFactorial 1 n

    let fibonacci n =
        if n < 1 then raise (InvalidOperationException())
        let rec countFibonacci accFirst accSecond x =
            match x with
            | 1 -> accFirst
            | _ -> countFibonacci accSecond (accFirst + accSecond) (x - 1)
        countFibonacci 0 1 n

    let reverse ls =
        let rec doReverse newList oldList =
            match oldList with
            | [] -> newList
            | head::tail -> doReverse (head::newList) tail
        doReverse [] ls

    let exponentiationNumberTwo n m =
        if n < 0 || m < 0 then raise (InvalidOperationException())
        let rec exp acc x = 
            match x with
            | 1 -> acc
            | _ -> exp (acc * 2) (x - 1)
        let rec doExponentiationNumberTwo list x m =
            match m with
            | -1 -> list
            | _ -> doExponentiationNumberTwo (x::list) (x / 2) (m - 1)
        doExponentiationNumberTwo [] (exp 2 (n + m)) m

    let getIndex number list =
        let rec doGetIndex number index list =
            match list with
            | [] -> None
            | head::tail -> if number = head then Some(index) else doGetIndex number (index + 1) tail
        doGetIndex number 0 list

    let array = exponentiationNumberTwo 3 6
    printfn "%A" (array)
    printfn "%A" (getIndex 2 array)
    printfn "%A" (getIndex 512 array)
    printfn "%A" (getIndex 8 array)
    0