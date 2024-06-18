namespace Rommulbad.Application.SessionService

open System
open Rommulbad.Domain

type ISessionService =
    abstract member AddSession: string * Session -> Result<unit, string>
    abstract member GetSessions: string -> seq<Session>
    abstract member GetTotalMinutes: string -> int
    abstract member GetEligibleSessions: string * string -> seq<Session>
    abstract member GetTotalEligibleMinutes: string * string -> int

