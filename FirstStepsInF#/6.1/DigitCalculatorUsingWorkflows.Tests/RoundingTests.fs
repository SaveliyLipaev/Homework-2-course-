module DigitCalculatorUsingWorkflows.RounderTests

open NUnit.Framework
open FsUnit
open DigitCalculator

[<Test>]
let ``Буквально пример из условия задачи`` () =
    let rounding = new RoundingCalculator(3)
    rounding {
        let! a = 2.0 / 12.0
        let! b = 3.5
        return a / b } |> should (equalWithin 0.001) 0.048

[<Test>]
let ``Простой тест`` () =
    let rounding = new RoundingCalculator(3)
    rounding {
        let! a = 14.5 / 12.
        let! b = 2.3
        return a / b } |> should (equalWithin 0.001) 0.525

[<Test>]
let ``Деление двух отрицательных чисел даёт положительный результат`` () =
    let rounding = new RoundingCalculator(3)
    rounding {
        let! a = -2.0 / 0.328
        let! b = -2.1515
        return a / b } |> should (equalWithin 0.001) 2.834

[<Test>]
let ``Расчет нулевой мантиссы работает без ошибок`` () =
    let rounding = new RoundingCalculator(3)
    rounding {
        let! a = 0. / -1.5
        let! b = 2.61
        return a / b } |> should (equalWithin 0.001) 0.

[<Test>]
let ``Проверка разных арифметических операций применённых последовательно`` () =
    let rounding = new RoundingCalculator(4)
    rounding {
        let! a = -0.5674 / -0.2567
        let! b = -0.3451 - 12.
        let! c = b * -1.0
        return a / (c - 12.) } |> should (equalWithin 0.0001) 6.405

[<Test>]
let ``Изменение точности в workflow builder влияет на точность результата`` () =
    let rounding = new RoundingCalculator(6)
    rounding {
        let! a = -0.5674 / -0.2567
        let! b = -0.3451
        return a / b } |> should (equalWithin 0.000001) -6.404991

[<Test>]
let ``Деление на 0 возвращает бесконечность`` () =
    let rounding = new RoundingCalculator(4)
    rounding {
        let! a = 123.4567 / 0.24
        let! b = 0.
        return a / b } |> should equal infinity

[<Test>]
let ``Проверка вычисления с большей точностью`` () =
    let rounding = new RoundingCalculator(9)
    rounding {
        let! a = 0.001 / 5.2
        let! b = 0.5
        return a / b } |> should equal 0.000384615
