namespace Rommulbad.Application.CandidateService

open System
open Domain.Candidate

    type ICandidateService =
        abstract member GetCandidates: unit -> seq<Candidate>
        abstract member GetCandidate: Name -> Option<Candidate>
        abstract member AddCandidate: Candidate-> Option<string>