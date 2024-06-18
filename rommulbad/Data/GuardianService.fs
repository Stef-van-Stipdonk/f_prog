module Rommulbad.Data.GuardianService

open System
open Rommulbad.Application.GuardianService
open Rommulbad.Data.CandidateService
open Rommulbad.Data.Store

type GuardianService(store: Store) =
    interface IGuardianService with            
        member _.AddGuardian(id: String, name) =
            Some("Not implemented")
