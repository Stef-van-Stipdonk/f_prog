module Rommulbad.Data.GuardianService

open System
open Domain
open Domain.Candidate
open Domain.Guardian
open Rommulbad.Application.GuardianService
open Rommulbad.Data.Database
open Rommulbad.Data.Store

type GuardianService(store: Store) =
    interface IGuardianService with            
        member _.AddGuardian(guardian: Guardian) =
            match InMemoryDatabase.lookup (Id.raw guardian.Id) store.guardians with
            | Some _ -> Error "Guardian with id already exists"
            | None ->
                match InMemoryDatabase.insert (Id.raw guardian.Id) guardian store.guardians with
                    | Ok() -> Ok()
                    | Error e -> Error $"Failed to add Guardian: %s{e.ToString()}"

        member this.AddCandidateForGuardian(guardianId: Id, candidate: Candidate) =
            match InMemoryDatabase.lookup (Id.raw guardianId) store.guardians with
            | Some _ ->
                match InMemoryDatabase.lookup candidate.Name store.candidates with
                    | Some candidate ->
                        match GuardianId.raw candidate.GuardianId = Id.raw guardianId with
                        | true -> Error "Guardian already has this candidate with the same name"
                        | false ->
                            match InMemoryDatabase.insert candidate.Name candidate store.candidates with
                            | Error e -> Error $"Failed to add Candidate: %s{e.ToString()}"
                            | Ok() -> Ok()
                    | None ->
                            match InMemoryDatabase.insert candidate.Name candidate store.candidates with
                            | Error e -> Error $"Failed to add Candidate: %s{e.ToString()}"
                            | Ok() -> Ok()
            | None -> Error "Guardian with id does not exist"
                            
                            
                        
                        
                        
                
            
