module Homework7.Task1.ILazy

/// Represents lazy calculation
type ILazy<'a> =
    /// Launches the calculation and returns the result
    abstract Get: unit -> 'a