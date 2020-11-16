namespace MakeGameToLearnOOP

open IOpSys

/// Represents "Invincible" OS that could not be infected with virus
type InvincibleOs() =
    interface IOpSys with
        member this.OsName = "Invincible"
        member this.InfectionRisk = 0.0
