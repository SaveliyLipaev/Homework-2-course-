namespace LambdaCalculus

/// <summary>
/// Implements normal‐order β-reduction interpreter of λ-expressions.
/// </summary>
module LambdaInterpreter =

    /// <summary> Type represents a λ-term.</summary>
    type Term =
        | Variable of string
        | Application of Term * Term
        | Lambda of string * Term

    /// <summary> Finds variables for a λ-term.</summary>
    let rec findVars T =
        match T with
        | Variable (x) -> Set.singleton x
        | Application (S, T) -> Set.union (findVars S) (findVars T)
        | Lambda (_, S) -> (findVars S)

    /// <summary> Finds free variables.</summary>
    let rec findFreeVars T =
        match T with
        | Variable (x) -> Set.singleton x
        | Application (S, T) -> Set.union (findFreeVars S) (findFreeVars T)
        | Lambda (str, T) -> Set.difference (findFreeVars T) (Set.singleton str)

    /// <summary>
    /// Finds new name which not contained in set of used names.
    /// </summary>
    let findNewName usedNames =
        let letters = List.map (fun x -> x.ToString()) [ 'a' .. 'z' ]

        let rec find count =
            let newSet =
                letters
                |> List.map (fun x ->
                    if (count = 0) then x else x + count.ToString())

            let newSet = List.filter (fun x -> not (Set.contains x usedNames)) newSet
            if (List.isEmpty newSet) then find (count + 1) else newSet.[0]
        find 0

    /// <summary>
    /// Performs term substitution on given term expression
    /// </summary>
    let rec substitution varName currT newT =
        match (currT, newT) with
        | (Lambda (y, term), Variable (_)) when y = varName -> Lambda(y, term)
        | (Variable (x), _) when varName = x -> newT
        | (Variable (x), _) -> currT
        | (Application (firstT, secondT), _) ->
            Application(substitution varName firstT newT, substitution varName secondT newT)
        | (Lambda (x, term), _) ->
            let termFV = findFreeVars term
            let newTermFV = findFreeVars newT
            let cond = (Set.contains x newTermFV) && (Set.contains varName termFV)
            if (not cond) then
                Lambda(x, substitution varName term newT)
            else
                let newName = findNewName (Set.union termFV newTermFV)
                let firstT = substitution x term (Variable(newName))
                let secondT = substitution varName firstT newT
                Lambda(newName, secondT)

    /// <summary>
    /// Performs β-reduction according normal‐order strategy.
    /// </summary>
    let betaReduction T =
        let rec betaReduce term =
            match term with
            | Variable (x) -> Variable(x)
            | Application (Lambda (x, firstT), secondT) -> substitution x firstT secondT
            | Application (firstT, secondT) ->
                let newT = betaReduce firstT
                match newT with
                | Lambda (x, currentTerm) -> betaReduce (substitution x currentTerm secondT)
                | _ -> Application(newT, betaReduce secondT)
            | Lambda (x, term) ->
                Lambda(x, betaReduce term)
        betaReduce T