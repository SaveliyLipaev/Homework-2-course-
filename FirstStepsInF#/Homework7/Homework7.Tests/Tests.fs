module Homework7.Tests2

open Homework7.Task2.AsyncDownloader
open NUnit.Framework

[<Test>]
let ``Download three pages`` () =
    let result = downloadAsync("http://hwproj.me/courses/34")
    Assert.AreEqual(result.Length, 29)

    for res in result do
        match res with 
        | (_, Error(x)) -> Assert.Fail()
        | _ -> Assert.IsTrue(true)

        let html = match result.Head with | _, Success(x) -> x | _ -> "Something got horribly wrong"
        Assert.AreEqual(html.Length, 84964)

[<Test>]
let ``Wrong address`` () =
        let result = downloadAsync("http://d")
        Assert.AreEqual(result.Length, 1)

        let (_, isError) = result.Head
        match isError with 
        | Success(x) -> Assert.Fail() 

        | _ -> Assert.IsTrue(true)