module Number_3

type Expression =
    | Add of Expression * Expression
    | Mul of Expression * Expression
    | Div of Expression * Expression
    | Sub of Expression * Expression
    | Value of int

let rec compute = function
    | Add(e1, e2) -> compute e1 + compute e2
    | Sub(e1, e2) -> compute e1 - compute e2
    | Mul(e1, e2) -> compute e1 * compute e2
    | Div(e1, e2) -> compute e1 / compute e2
    | Value(x) -> x 