module CountNumbers

let countEvenNumberWithMap =
    List.map (fun elem -> (elem % 2) ^^^ 1) >> List.sum

let countEvenNumberWithFilter =
    List.filter (fun elem -> elem % 2 = 0) >> List.length

let countEvenNumberWithFold =
    List.fold (fun acc elem -> acc + 1 - abs (elem % 2)) 0 