module LambdaCalculus.Tests

open Xunit
open FsUnit.Xunit
open LambdaCalculus.LambdaInterpreter

type MyTests() =
    let tempExpression = Application(Lambda("x", Variable("x")), Variable("y"))

    [<Fact>]
    member this.``betaReduction (l.x.y) ((l.x.x x x) (l.x.x x x)) should equal y``() =
        let tempTerm = Lambda("x", Application(Variable("x"), Application(Variable("x"), Variable("x"))))
        let term = Application(Lambda("x", Variable("y")), Application(tempTerm, tempTerm))
        betaReduction term |> should equal (Variable("y"))

    [<Fact>]
    member this.``betaReduction K I should equal K*``() =
        let term = Application(Lambda("x", Lambda("y", Variable("x"))), Lambda("x", Variable("x")))
        betaReduction term |> should equal (Lambda("y", Lambda("x", Variable("x"))))

    [<Fact>]
    member this.``findNewName {"x", "a"} should equal "b"``() =
        let free = findNewName (Set.empty.Add("x").Add("a"))
        free |> should equal "b"

    [<Fact>]
    member this.``findFreeVars tempExpression should equal {"y"}``() =
        let free = findFreeVars tempExpression
        free |> should equal (Set.empty.Add("y"))

    [<Fact>]
    member this.``findVars tempExpression should equal {"x", "y"}``() =
        let free = findVars tempExpression
        free |> should equal (Set.empty.Add("y").Add("x"))

    [<Fact>]
    member this.``Term substitution of one should equal x``() =
        let one =
            substitution "y" (Variable("x"))
                (Application(Lambda("x", Application(Variable("a"), Variable("y"))), Variable("y")))
        one |> should equal (Variable("x"))

    [<Fact>]
    member this.``Id id should equal id``() =
        let term = Lambda("x", Application(Lambda("x", Variable("x")), Variable("x")))
        betaReduction term |> should equal (Lambda("x", Variable("x")))