module DigitCalculator

open System

(*  RoundingCalculator is a workflow builder
    for calculation with specified precision *)
type RoundingCalculator(precision: int) =
    member this.Bind(x: float, f: float -> float) = Math.Round(f x, precision)
    member this.Return(x: float) = Math.Round(x, precision)