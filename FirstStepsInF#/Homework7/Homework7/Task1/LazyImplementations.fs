module Homework7.Task1.LazyImplementations

open Homework7.Task1.ILazy
open System
open System.Threading

/// Represents lazy calculator with single thread work guarantee
type SingleThreadedLazy<'a>(supplier: unit -> 'a) =
    let mutable result = None

    interface ILazy<'a> with
        /// Launches the calculation and returns the result
        member this.Get() =
            match result with
            | None ->
                let value = supplier ()
                result <- Some value
                value
            | Some value -> value

/// Multi-threaded version of Lazy with a guarantee of correct operation in multi-threaded mode
type MultiThreadedLazy<'a>(supplier: unit -> 'a) =
    let mutable result = None
    let lockObject = Object()

    interface ILazy<'a> with
        member this.Get() =
            match result with
            | None ->
                lock lockObject (fun () ->
                    match result with
                    | Some x -> x
                    | None ->
                        let value = supplier ()
                        result <- Some value
                        value)
            | Some value -> value

/// Lock-free lazy with multi-threaded mode work guarantee
type LockFreeLazy<'a>(supplier: unit -> 'a) =
    let mutable result = None
    let emptyResult = result

    interface ILazy<'a> with
        member this.Get() =
            match result with
            | None ->
                let value = supplier ()
                Interlocked.CompareExchange(&result, Some value, emptyResult)
                |> ignore

                match result with
                | Some x -> x
                | None -> value
            | Some value -> value