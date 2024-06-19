namespace Rommulbad.Application.SessionService

open System
open Domain.Candidate
open Domain.Session

type ISessionService =
    abstract member AddSession: NameOfCandidate * Session -> Result<unit, string>
    abstract member GetSessions: NameOfCandidate -> seq<Session>
    abstract member GetTotalMinutes: NameOfCandidate -> int
    abstract member GetEligibleSessions: NameOfCandidate * Diploma -> seq<Session>
    abstract member GetTotalEligibleMinutes: NameOfCandidate * Diploma -> int
    abstract member AwardDiploma: NameOfCandidate -> Result<string, string>

