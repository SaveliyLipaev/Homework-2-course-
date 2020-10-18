module Homework_2.Tests

open NUnit.Framework
open FsUnit
open FsCheck

open CountNumbers
open BinaryTreeMap
open ExpressionTree
open GeneratePrimeSequence

// (Задача 1) Подсчет чётных чисел с map, filter, fold
[<TestFixture>]
module ``Test: Count even numbers with map, filter, fold`` =
    let testData =
        [ [], 0
          [ 37; 49; 15; 1 ], 0
          [ 1; 4; 3; -4; 0 ], 3
          [ 8; 42; 12; -4; 0 ], 5 ]
        |> List.map (fun (xs, expected) -> TestCaseData(xs).Returns expected)

    [<TestCaseSource("testData")>]
    let TestMapImplementation xs =
        countEvenNumberWithMap xs

    [<TestCaseSource("testData")>]
    let TestFilterImplementation xs =
        countEvenNumberWithFilter xs

    [<TestCaseSource("testData")>]
    let TestFoldImplementation xs =
        countEvenNumberWithFold xs

    // Проверяем работу функций на разных данных используя FsCheck
    let funToCheck (xs: list<int>) =
        countEvenNumberWithFold xs = countEvenNumberWithFilter xs && countEvenNumberWithFold xs = countEvenNumberWithMap xs

    [<Test>]
    let ``Check with FsCheck`` () =
        Check.QuickThrowOnFailure funToCheck



// (Задача 2) Map для двоичного дерева
module ``Test: Implement map function for binary trees`` =
    [<Test>]
    let ``Map an empty tree`` () =
        (map Empty id) |> should equal Empty

    [<Test>]
    let ``Square all values in a tree with single node`` () =
        (map (Node(2, Empty, Empty)) (pown 2)) |> should equal (Node(4, Empty, Empty))

    [<Test>]
    let ``Calculate: (value of every node) mod 2`` () =
        map (Node(1, Node(5, Node(4, Empty, Empty), Node(7, Empty, Empty)), Node(9, Empty, Empty))) (fun x -> x % 2)
        |> should equal
        <| Node(1, Node(1, Node(0, Empty, Empty), Node(1, Empty, Empty)), Node(1, Empty, Empty))

// (Задача 3) Дерево разбора арифметического выражения
module ``Test: Evaluate an arithmetic expression tree`` =
    [<Test>]
    let ``Just a value`` () = (compute (Value 42)) |> should equal 42

    [<Test>]
    let ``Compute: (7 + 3) * 2`` () =
        compute (Mul(Add(Value 7, Value 3), Value 2)) |> should equal 20

    [<Test>]
    let ``Compute: (42 - 44) / 2`` () =
        compute (Div(Sub(Value 42, Value 44), Value 2)) |> should equal -1

    [<Test>]
    let ``Division by zero`` () =
        (fun () -> compute (Div(Value 1, Value 0)) |> ignore)
        |> should (throwWithMessage "Attempted to divide by zero.") typeof<System.DivideByZeroException>

// (Задача 4) Простые числа
module ``Test: Generate lazy prime number sequence`` =

    [<Test>]
    let ``Test 0 prime numbers`` () =
        Seq.sum (Seq.take 0 <| primeGenerator) |> should equal 0

    [<Test>]
    let ``Test first prime number`` () =
        Seq.sum (Seq.take 1 <| primeGenerator) |> should equal 2

    [<Test>]
    let ``Sum of first 3 prime numbers 2 + 3 + 5 equals 10`` () =
        Seq.sum (Seq.take 3 <| primeGenerator) |> should equal 10

    [<Test>]
    let ``2 + 3 + 5 + 7 + 11 + 13 + 17 + 19 + 23 equals 10`` () =
        Seq.sum (Seq.take 9 <| primeGenerator) |> should equal (2 + 3 + 5 + 7 + 11 + 13 + 17 + 19 + 23)
