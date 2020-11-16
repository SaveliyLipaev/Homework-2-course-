module MakeGameToLearnOOP.AvailableOsTypes

open IOpSys

type OsLinux() =
    interface IOpSys with
        member this.OsName = "Linux"
        member this.InfectionRisk = 0.3

type OsWindows() =
    interface IOpSys with
        member this.OsName = "Windows"
        member this.InfectionRisk = 0.8