module Rommulbad.Service.Guardian

open Domain.Guardian
open Giraffe
open Rommulbad.Application.GuardianService
open Rommulbad.Service.Serialization
open Thoth.Json.Giraffe
open Rommulbad.Service.Serialization.Guardian

let addGuardian () : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<IGuardianService>()
            let! body = ThothSerializer.ReadBody ctx decode
            match body with
            | Ok guardian ->
                match service.AddGuardian(guardian) with
                | Ok () -> return! text "OK" next ctx
                | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
            | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
        }

let addCandidateForGuardian (guardianId: string) : HttpHandler =
    fun next ctx ->
        task {
            match Id.ofRaw guardianId with
            | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
            | Ok id ->
            
            let service = ctx.GetService<IGuardianService>()
            let! body = ThothSerializer.ReadBody ctx Candidates.decode
            match body with
            | Ok candidate ->
                match service.AddCandidateForGuardian(id, candidate) with
                | Ok () -> return! text "OK" next ctx
                | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
            | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
        }        

let routes : HttpHandler =
    choose
        [
           POST >=> route "/guardian" >=> addGuardian ()
           POST >=> routef "/guardian/%s/candidate" addCandidateForGuardian
        ]
   