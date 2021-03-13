module Homework4.Tests.PhonebookTests

open Homework4.Phonebook
open NUnit.Framework
open FsUnit
open FsCheck

let private testDB =
    [ ("8800", "Ilya")
      ("8800", "Dima")
      ("5535", "Mom") ]

[<Test>]
let ``test adding new person to phonebook`` () =
    let func phone name listAsDB =
        List.length (addToDB phone name listAsDB) = (List.length listAsDB)
        + 1

    Check.QuickThrowOnFailure func

[<Test>]
let ``test finding by phone`` () =
    findNameByPhone "8800" testDB
    |> List.sort
    |> should equal [ "Dima"; "Ilya" ]
    findNameByPhone "8800" [] |> should equal []

[<Test>]
let ``test finding by name`` () =
    let list = testDB @ [ ("8899", "Dima") ] // Adding another Dima
    findPhoneByName "Dima" list
    |> List.sort
    |> should equal [ "8800"; "8899" ]
    findNameByPhone "Terekhov" [] |> should equal []

[<Test>]
let ``test saving & loading`` () =
    let func (listAsDB: (string * string) list) =
        saveToFile listAsDB
        let list = readFromFile "DB.dat"
        List.sort list = List.sort listAsDB

    Check.QuickThrowOnFailure func