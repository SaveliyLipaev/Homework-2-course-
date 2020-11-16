namespace MakeGameToLearnOOP

open ILogger

type LoggerImpossibleProbability(compNumber) =
    let mutable stepNumber = 0

    interface ILogger with
        member this.LogState state =
            if stepNumber >= 500 then
                let expectedState = [| for i in 1 .. compNumber -> false |]

                if state = expectedState then failwith "Success!" else failwith "Failure!"
            else
                stepNumber <- stepNumber + 1

            stepNumber
