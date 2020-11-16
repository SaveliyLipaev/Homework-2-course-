module Homework4.Tests.BracketChecker

open Homework4.BracketChecker
open NUnit.Framework

// Using NUnit test fixture
[<TestFixture>]
module ``Tests for bracket balance checker`` =
    let testData () =
        [ TestCaseData("{( (o･ω･o) ())}").Returns(true)
          TestCaseData("((AA)b%)").Returns(true)
          TestCaseData("{{}[]]").Returns(false)
          // assumed that empty string should return true
          TestCaseData("").Returns(true)
          TestCaseData("{aa((a{{dd}]>").Returns(false)
          TestCaseData("{([< ٩(｡•́‿•̀｡)۶>])}").Returns(true)
          TestCaseData("(").Returns(false)
          TestCaseData("({aaaa} (っಠ‿ಠ)っ }BB&^&)[dd^$#]").Returns(false) ]

    [<TestCaseSource("testData")>]
    let basicTests str = checkBalance str