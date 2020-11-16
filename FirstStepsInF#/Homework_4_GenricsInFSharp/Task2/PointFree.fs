module Homework4.PointFree

// Условие задачи:
//    Записать в point-free стиле ```func x l = List.map (fun y -> y * x) l.```
//    Выписать шаги вывода и проверить с помощью FsCheck корректность результата

/// Simple function that multiplies
/// each element of the list by the passed argument
let func x xs = List.map (fun elem -> elem * x) xs

let func'1 x = List.map (fun elem -> elem * x)

let func'2 x = List.map (fun elem -> (*) x elem)

let func'3 x = List.map ((*) x)

/// Final reimplementation of multiplying
/// function rewritten in 'point-free style'
let func'4 = List.map << (*)

[<EntryPoint>]
let main _ =
    printfn "_____________"
    printfn "\n[Simple test]"
    let xs = [1; 2; 3; 4; 5]
    printfn "Condition: %b (should be true)" (func 2 xs = func'4 2 xs)
    printfn "\t-> test list: %A" xs
    printfn "\t-> result: %A" (func'4 2 xs)
    0