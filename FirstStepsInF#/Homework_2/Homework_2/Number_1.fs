module Number_1

let countEvenNumberWithMap =
    List.map(fun elem -> 1 - abs(elem % 2)) >> List.sum

let countEvenNumberWithFilter =
    List.filter(fun elem -> elem % 2 = 0) >> List.length

let countEvenNumberWithFold =
    List.fold(fun acc elem -> acc + 1 - abs(elem % 2)) 0 