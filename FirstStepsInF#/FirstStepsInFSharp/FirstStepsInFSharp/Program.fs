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
            | _ -> countFibonacci (accSecond) (accFirst + accSecond) (x - 1)
        countFibonacci 0 1 n

    let reverse ls =
        let rec doReverse newList oldList =
            match oldList with
            | [] -> newList
            | _ -> doReverse (oldList.Head::newList) (oldList.Tail)
        doReverse [] ls

    let exponentiationNumberTwo n m =
        if n < 0 || m < 0 then raise (InvalidOperationException())
        let rec exp acc x = 
            match x with
            | 1 -> acc
            | _ -> exp (acc * 2) (x - 1)
        let rec doExponentiationNumberTwo list n m =
            match m with
            | -1 -> list
            | _ -> doExponentiationNumberTwo (exp (2) (n + m)::list) (n) (m - 1)
        doExponentiationNumberTwo [] n m

    let getIndex number list =
        let rec doGetIndex number index list =
            match list with
            | [] -> raise (InvalidOperationException())
            | _ -> if number = list.Head then index else doGetIndex (number) (index + 1) (list.Tail)
        doGetIndex number 0 list

    0