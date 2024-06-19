module Rommulbad.Service.Candidate

open Domain.Candidate
open Giraffe
open Rommulbad.Application.CandidateService
open Thoth.Json.Giraffe
open Rommulbad.Service.Serialization.Candidates

let addCandidate () : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ICandidateService>()
            
            let! body = ThothSerializer.ReadBody ctx decode
            match body with
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            | Ok candidate ->
                match service.AddCandidate(candidate) with
                | Some message -> return! text message next ctx
                | None -> return! RequestErrors.BAD_REQUEST "Failed to add candidate" next ctx
        }
            
let getCandidates : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ICandidateService>()

            let candidates = service.GetCandidates()
            let json = serializeCandidates candidates
            return! text json next ctx
        }

let getCandidate (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let service = ctx.GetService<ICandidateService>()
            match Name.ofRaw name with
            | Ok name ->
                match service.GetCandidate(name) with
                | None -> return! RequestErrors.NOT_FOUND "Employee not found!" next ctx
                | Some candidate ->
                    let json = serializeCandidate candidate
                    return! text json next ctx
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
        }
        
let routes: HttpHandler =
    choose [ 
             GET >=> route "/candidate" >=> getCandidates
             GET >=> routef "/candidate/%s" getCandidate
             ]
