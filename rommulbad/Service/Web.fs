namespace Rommulbad.Service

open Giraffe
open Rommulbad.Application.CandidateService
open Rommulbad.Application.GuardianService
open Rommulbad.Application.SessionService
open Rommulbad.Data
open Rommulbad.Data.GuardianService
open Rommulbad.Domain
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Rommulbad.Service.Serialization

module Web =
    let getCandidates (service: ICandidateService) : HttpHandler =
        fun next ctx ->
            task {
                let candidates = service.GetCandidates()
                let json = serializeCandidates candidates
                return! text json next ctx
            }

    let getCandidate (service: ICandidateService) (name: string) : HttpHandler =
        fun next ctx ->
            task {
                match service.GetCandidate(name) with
                | None -> return! RequestErrors.NOT_FOUND "Employee not found!" next ctx
                | Some candidate ->
                    let json = serializeCandidate candidate
                    return! text json next ctx
            }

    let addSession (service: ISessionService) (name: string) : HttpHandler =
        fun next ctx ->
            task {
                let! body = ThothSerializer.ReadBody ctx Session.decode
                match body with
                | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
                | Ok session ->
                    match service.AddSession(name, session) with
                    | Ok() -> return! text "OK" next ctx
                    | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
            }

    let getSessions (service: ISessionService) (name: string) : HttpHandler =
        fun next ctx ->
            task {
                let sessions = service.GetSessions(name)
                let json = serializeSessions sessions
                return! text json next ctx
            }

    let getTotalMinutes (service: ISessionService) (name: string) : HttpHandler =
        fun next ctx ->
            task {
                let total = service.GetTotalMinutes(name)
                let json = Encode.int total |> Encode.toString 0
                return! text json next ctx
            }

    let getEligibleSessions (service: ISessionService) (name: string, diploma: string) : HttpHandler =
        fun next ctx ->
            task {
                let sessions = service.GetEligibleSessions(name, diploma)
                let json = serializeSessions sessions
                return! text json next ctx
            }

    let getTotalEligibleMinutes (service: ISessionService) (name: string, diploma: string) : HttpHandler =
        fun next ctx ->
            task {
                let total = service.GetTotalEligibleMinutes(name, diploma)
                let json = Encode.int total |> Encode.toString 0
                return! text json next ctx
            }
            
    let addCandidate (service: ICandidateService) : HttpHandler =
        fun next ctx ->
            task {
                let! body = ThothSerializer.ReadBody ctx Candidate.decode
                match body with
                | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
                | Ok candidate ->
                    match service.AddCandidate(candidate) with
                    | Some message -> return! text message next ctx
                    | None -> return! RequestErrors.BAD_REQUEST "Failed to add candidate" next ctx
            }

    let routes (candidateService: ICandidateService) (sessionService: ISessionService) (guardianService: IGuardianService) : HttpHandler =
        choose
            [ GET >=> route "/candidate" >=> getCandidates candidateService
              GET >=> routef "/candidate/%s" (getCandidate candidateService)
              POST >=> routef "/candidate/%s/session" (addSession sessionService)
              GET >=> routef "/candidate/%s/session" (getSessions sessionService)
              GET >=> routef "/candidate/%s/session/total" (getTotalMinutes sessionService)
              GET >=> routef "/candidate/%s/session/%s" (getEligibleSessions sessionService)
              GET >=> routef "/candidate/%s/session/%s/total" (getTotalEligibleMinutes sessionService) ]
