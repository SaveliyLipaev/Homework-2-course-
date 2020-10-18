namespace StringCalculatorUsingWorkflows

module StringCalculator =
    
    open System
    
    /// <summary>
    /// Converts string to int option
    /// </summary>
    let toInt (str: string) =
        let isSucceeded, value = Int32.TryParse(str)
        match isSucceeded with
        | true -> Some(value)
        | _ -> None
    
    /// <summary>
    ///  Implements workflow builder
    ///  which performs calculations on numbers specified as strings values.
    /// </summary>
    type StringCalculatorBuilder() =

        member this.Bind(x: string, f: Int32 -> 'b option) =
            let converted = toInt x
            match converted with
            | None -> None
            | Some value -> f value

        member this.Return(x) = Some(x)
