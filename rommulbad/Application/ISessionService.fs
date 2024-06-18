namespace Rommulbad.Application.SessionService

open System
open Domain.Candidate
open Domain.Session

type ISessionService =
    abstract member AddSession: Name * Session -> Result<unit, string>
    abstract member GetSessions: Name -> seq<Session>
    abstract member GetTotalMinutes: Name -> int
    abstract member GetEligibleSessions: Name * Diploma -> seq<Session>
    abstract member GetTotalEligibleMinutes: Name * Diploma -> int

