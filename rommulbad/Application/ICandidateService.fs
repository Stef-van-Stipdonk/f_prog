namespace Rommulbad.Application.CandidateService

open System
open Rommulbad.Domain

    type ICandidateService =
        abstract member GetCandidates: unit -> seq<Candidate>
        abstract member GetCandidate: string -> Option<Candidate>
        abstract member AddCandidate: Candidate-> Option<string>
