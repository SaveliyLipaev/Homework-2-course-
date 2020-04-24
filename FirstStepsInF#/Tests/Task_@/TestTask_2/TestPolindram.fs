module TestTask_2

open NUnit.Framework
open FsUnit
open Task_2.ThreeDigitPalindrome

[<TestFixture>]
type ``TestsForMaxPalindrome`` () =

    [<Test>]
    member this.``The function should return "906609" ``() =
        getMaxPalindrome()  |> should equal "906609"
