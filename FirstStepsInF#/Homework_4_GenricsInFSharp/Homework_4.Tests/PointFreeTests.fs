module Homework4.Tests.PointFree

open Homework4.PointFree
open NUnit.Framework
open FsCheck

// Test if original and point-free functions all return same result
[<Test>]
let ``Check func'1`` () =
    Check.QuickThrowOnFailure(fun x xs -> (func x xs) = (func'1 x xs))

[<Test>]
let ``Check func'2`` () =
    Check.QuickThrowOnFailure(fun x xs -> (func x xs) = (func'2 x xs))

[<Test>]
let ``Check func'3`` () =
    Check.QuickThrowOnFailure(fun x xs -> (func x xs) = (func'3 x xs))

[<Test>]
let ``Check func'4`` () =
    Check.QuickThrowOnFailure(fun x xs -> (func x xs) = (func'4 x xs))