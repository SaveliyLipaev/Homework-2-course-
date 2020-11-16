module Homework7.Tests

open Homework7.Task1.LazyFactory
open System.Threading
open NUnit.Framework
open FsUnit

[<Test>]
let ``Single-threaded lazy works`` () =
    let calculator = LazyFactory.CreateSingleThreadedLazy(fun () -> 42)
    calculator.Get () |> should equal 42

[<Test>]
let ``Multi-threaded lazy calculator works`` () =
    let calculator = LazyFactory.CreateMultiThreadedLazy(fun () -> 888)
    calculator.Get () |> should equal 888

[<Test>]
let ``Multi-threaded lazy calculator calls supplier only one time`` () =
    let mutable counter = ref 0L
    let calculator = LazyFactory.CreateMultiThreadedLazy(fun () -> 
        Interlocked.Increment counter |> ignore
        (Interlocked.Read counter) |> should lessThan 2)

    for i in 1..100 do
        ThreadPool.QueueUserWorkItem (fun obj -> calculator.Get ()) |> ignore

[<Test>]
let ``Single-threaded lazy calculator calls supplier only one time`` () =
    let mutable counter = 0
    let calculator = LazyFactory.CreateSingleThreadedLazy(fun () -> 
        counter <- counter + 1
        counter |> should lessThan 2)

    for i in 1..1000 do
        calculator.Get ()

[<Test>]
let ``Multi-threaded lazy returns the same object on every call`` () =
    let calculator = LazyFactory.CreateMultiThreadedLazy(fun () -> new System.Object())

    let expected = calculator.Get ()
    for i in 1..100 do
        ThreadPool.QueueUserWorkItem (fun obj -> 
            expected |> (calculator.Get ()).Equals |> should be True) |> ignore

[<Test>]
let ``Lock-free lazy returns the same object on every call`` () =
    let calculator = LazyFactory.CreateLockFreeLazy(fun () -> new System.Object())

    let expected = calculator.Get ()
    for i in 1..100 do
        ThreadPool.QueueUserWorkItem (fun obj -> 
            expected |> (calculator.Get ()).Equals |> should be True) |> ignore
