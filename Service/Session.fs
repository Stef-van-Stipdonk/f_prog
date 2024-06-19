module Rommulbad.Service.Session

open Domain.Candidate
open Domain.Session
open Giraffe.HttpStatusCodeHandlers
open Rommulbad.Application.SessionService
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Giraffe
open Rommulbad.Service.Serialization.Session

let addSession (nameStr: string) : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ISessionService>()
            let! body = ThothSerializer.ReadBody ctx decode
            match NameOfCandidate.ofRaw nameStr, body with
            | Ok name, Ok session ->
                match service.AddSession(name, session) with
                | Ok () -> return! text "OK" next ctx
                | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
            | Error errorMessage, _ -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            | _, Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
        }

let getSessions (nameStr: string) : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ISessionService>()
            match NameOfCandidate.ofRaw nameStr with
            | Ok name ->
                let sessions = service.GetSessions(name)
                let json = serializeSessions sessions
                return! text json next ctx
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
        }

let getTotalMinutes (nameStr: string) : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ISessionService>()
            match NameOfCandidate.ofRaw nameStr with
            | Ok name ->
                let total = service.GetTotalMinutes(name)
                let json = Encode.int total |> Encode.toString 0
                return! text json next ctx
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
        }

let getEligibleSessions (nameStr: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ISessionService>()
            match NameOfCandidate.ofRaw nameStr with
            | Ok name ->
                match Diploma.ofRaw diploma with
                | Ok diploma ->
                    let sessions = service.GetEligibleSessions(name, diploma)
                    let json = serializeSessions sessions
                    return! text json next ctx
                | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
        }

let getTotalEligibleMinutes (nameStr: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ISessionService>()
            match NameOfCandidate.ofRaw nameStr with
            | Ok name ->
                match Diploma.ofRaw diploma with
                | Ok diploma ->
                    let total = service.GetTotalEligibleMinutes(name, diploma)
                    let json = Encode.int total |> Encode.toString 0
                    return! text json next ctx
                | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
        }
        
let awardDiploma (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ISessionService>()
            match NameOfCandidate.ofRaw name with
            | Ok name ->
                match service.AwardDiploma(name) with
                | Ok resultValue -> return! text resultValue next ctx
                | Error errorValue -> return! RequestErrors.BAD_REQUEST errorValue next ctx
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
        }

let routes: HttpHandler =
    choose
        [ POST >=> routef "/candidate/%s/session" addSession
          GET >=> routef "/candidate/%s/session" getSessions
          GET >=> routef "/candidate/%s/session/total" getTotalMinutes
          GET >=> routef "/candidate/%s/session/%s" getEligibleSessions
          GET >=> routef "/candidate/%s/award" awardDiploma
          GET >=> routef "/candidate/%s/session/%s/total" getTotalEligibleMinutes
        ]
