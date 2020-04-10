module Taks_3

type ThreadsafeStack<'T>() =
    let mutable _stack: List<'T> = []

    member this.Push value = lock _stack (fun () -> _stack <- value :: _stack)

    member this.TryPop() =
        lock _stack (fun () ->
            match _stack with
            | result :: myNewStack ->
                _stack <- myNewStack
                result |> Some
            | [] -> None)