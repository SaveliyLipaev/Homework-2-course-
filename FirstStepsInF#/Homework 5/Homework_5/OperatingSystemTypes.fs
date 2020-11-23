module Homework_5.OperatingSystemTypes

open IOpSystem

type OsLinux() =
    interface IOpSystem with
        member this.Name = "Linux"
        member this.InfectionRisk = 0.3

type OsWindows() =
    interface IOpSystem with
        member this.Name = "Windows"
        member this.InfectionRisk = 0.8

type NoSecurityOs() =
    interface IOpSystem with
        member this.Name = "Sieve"
        member this.InfectionRisk = 1.0

type InvincibleOs() =
    interface IOpSystem with
        member this.Name = "Invincible"
        member this.InfectionRisk = 0.0
