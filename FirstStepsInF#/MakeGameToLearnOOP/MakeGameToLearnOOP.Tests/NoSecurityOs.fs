namespace MakeGameToLearnOOP

open IOpSys

/// Represents OS with NO SECURITY at all; Guaranteed infection
type NoSecurityOs() =
    interface IOpSys with
        member this.OsName = "Sieve"
        member this.InfectionRisk = 1.0