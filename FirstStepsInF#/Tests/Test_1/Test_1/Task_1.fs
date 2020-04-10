module Task_1
    type typeInt = int
    let findMin =
        List.fold (fun acc x -> min acc x) typeInt.MaxValue 