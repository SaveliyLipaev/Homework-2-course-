module Homework4.BracketChecker

// Условие задачи:
//    Реализовать функцию, которая по произвольной строке
//    проверяет корректность скобочной последовательности в этой строке.
//    Скобки бывают трёх видов.

/// Structure represent bracket pair
type Bracket =
    struct
        val opening: char
        val closing: char
        new(op: char, cl: char) = { opening = op; closing = cl }
    end

/// Creates matching brackets from defined symbols
let makeBracket bracket =
    match bracket with
    | ')' | '(' -> Some(Bracket('(', ')'))
    | '>' | '<' -> Some(Bracket('<', '>'))
    | '}' | '{' -> Some(Bracket('{', '}'))
    | _ -> None

/// Checks if string is balanced
let checkBalance (s: string) =
    let rec balance chars buffer =
        match chars with
        | [] -> List.empty = buffer
        | head :: tail ->
            match makeBracket head with
            | None -> balance tail buffer
            | Some (x) when head = x.opening -> balance tail (head :: buffer)
            | Some (x) -> if buffer.Head = x.opening then balance tail buffer.Tail else false

    balance (s.ToCharArray() |> List.ofArray) []


[<EntryPoint>]
let main _ =
    printfn "_____________"
    printfn "[Simple test]\n"

    printfn
        "\"%s\" is balanced: %b"
        "([(other < |symbols > do not matter )])"
        ("([(hidden < |symbols > do not matter )])" |> checkBalance)

    printfn
        "\"%s\" is balanced: %b"
        "(<(  ͡• ͜ʖ ͡• )--<)"
        ("(<(  ͡• ͜ʖ ͡• )--<)" |> checkBalance)
    0