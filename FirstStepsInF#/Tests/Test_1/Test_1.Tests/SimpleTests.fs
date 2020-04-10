module Test_1.Tests
open NUnit.Framework
open FsUnit
open Taks_1
open Taks_2
open Taks_3


[<TestFixture>]
type TestfindMin() =

    [<Test>]
    member this.``Dummy`` () =
        let list = [132155552;2353; 155535;1342354235]
        let expected = List.min list
        findMin list |> should equal expected


    [<Test>]
    member this.``With negatives`` () =
        let list = [-256; 2353; 0; 1342354235]
        let expected = List.min list
        findMin list |> should equal expected


[<TestFixture>]
type TestOfMyImmutableQueue () =

    [<Test>]
    member this.``Push 1 to empty stack`` () =
        let stack = ThreadsafeStack<int>()
        let expected = 1
        expected |> stack.Push 
        stack.TryPop |> should equal expected

    [<Test>]
    member this.``Pushg to 1 element  que`` () =
        let stack = ThreadsafeStack<int>()
        let expected = 3
        1 |> stack.Push 
        2 |> stack.Push 
        expected |> stack.Push 
        stack.TryPop |> should equal expected

    