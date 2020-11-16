module Homework7.Tests2

open Homework7.Task2.AsyncDownloader
open NUnit.Framework

[<Test>]
let ``Download three pages``=
    let result = downloadAsync("https://www.google.ru/")
    Assert.AreEqual(result.Length, 3)

    for res in result do
        match res with 
        | (_, Error(x)) -> Assert.Fail()
        | _ -> Assert.IsTrue(true)

        let html = match result.Head with | _, Success(x) -> x | _ -> "Something got horribly wrong"
        Assert.AreEqual(html.Length, 62477)

[<Test>]
let ``Wrong address`` () =
        let result = downloadAsync("http://d")
        Assert.AreEqual(result.Length, 1)

        let (_, isError) = result.Head
        match isError with 
        | Success(x) -> Assert.Fail() 

        | _ -> Assert.IsTrue(true)