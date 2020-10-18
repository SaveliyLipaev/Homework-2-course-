module StringCalculatorUsingWorkflows.StringCalculatorTests

open NUnit.Framework
open FsUnit
open StringCalculator

[<Test>]
let ``Каждое из строковых значений является числом - результат вычислен`` () =
    let calculate = new StringCalculatorBuilder()
    calculate {
        let! a = "1"
        let! b = "21"
        let c = a * b
        let f = c * c
        let! g = "441"
        let h = g / f
        return h
    }
    |> should equal (Some(1))

[<Test>]
let ``Вычисления в целых числах не вызывает ошибку`` () =
    let calculate = new StringCalculatorBuilder()
    calculate {
        let! a = "1"
        let! b = "22"
        let c = a * b
        let f = c * c
        let! g = "800"
        let h = g / f
        return h
    }
    |> should equal (Some(1))

[<Test>]
let ``Строки которые невозможно конвертировать в числа - возвращают значение указ. на отсутствие результата`` () =
    let calculate = new StringCalculatorBuilder()
    calculate {
        let! a = "Якого дідька ти тут шумиш?"
        let b = a * 1
        let! c = "कृपया कटौती न करें"
        let d = a + b + c
        return d
    }
    |> should equal None