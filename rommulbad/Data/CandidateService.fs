module Rommulbad.Data.CandidateService
open System
open Domain.Candidate
open Rommulbad.Application.CandidateService
open Rommulbad.Data.Database
 open Rommulbad.Data.Store

type CandidateService(store: Store) =
    interface ICandidateService with
        member _.GetCandidates() =
            store.candidates
            |> InMemoryDatabase.filter (fun _ -> true)

        member _.GetCandidate(name: string) =
            store.candidates
            |> InMemoryDatabase.lookup name
            
        member _.AddCandidate(candidate: Candidate) =
            let key = candidate.Name
            match InMemoryDatabase.insert key {
                Name = candidate.Name
                DateOfBirth = candidate.DateOfBirth
                GuardianId = candidate.GuardianId
                Diploma = candidate.Diploma
            } store.candidates with
            | Ok() -> Some("Candidate added")
            | Error e -> Some(sprintf "Failed to add candidate: %s" (e.ToString()))
