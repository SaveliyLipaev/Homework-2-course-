module Homework_5.Computer

open IOpSystem

type Computer(computerId: int, OpSys: IOpSystem, initialStatus) =
    let mutable status = initialStatus

    member this.Id = computerId

    member this.Os = OpSys

    member this.Status = status

    member this.Risk = this.Os.InfectionRisk

    member this.Infect() = status <- true

    member this.PrintStatus() =
        printfn "| Computer:\t%d\t| OS:\t%s\t| Infected:\t%b\t|" computerId OpSys.Name status
