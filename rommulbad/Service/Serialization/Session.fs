module Rommulbad.Service.Serialization.Session

open Domain.Session
open Thoth.Json.Net

let encode: Encoder<Session> =
    fun session ->
        Encode.object
            [ "deep", Encode.bool (SwamInDeepPool.raw session.SwamInDeepPool)
              "date", Encode.datetime (DateOfSession.raw session.DateOfSession)
              "minutes", Encode.int (MinutesSwam.raw session.MinutesSwam) ]

let decode: Decoder<Session> =
    Decode.object (fun get ->
        
            {
                NameOfCandidate = get.Required.Field "name" (Decode.string
                |> Decode.andThen (fun string ->
                    match NameOfCandidate.ofRaw string with
                    | Ok name -> Decode.succeed name
                    | Error msg -> Decode.fail msg
                ))  
                SwamInDeepPool = get.Required.Field "deep" (Decode.bool
                |> Decode.andThen (fun boolean ->
                    match SwamInDeepPool.ofRaw boolean with
                    | Ok deep -> Decode.succeed deep
                    | Error msg -> Decode.fail msg
                ))  
                DateOfSession = get.Required.Field "date" (Decode.datetimeUtc
                |> Decode.andThen (fun datetime ->
                    match DateOfSession.ofRaw datetime with
                    | Ok date -> Decode.succeed date
                    | Error msg -> Decode.fail msg
                ))
                MinutesSwam = get.Required.Field "minutes" (Decode.int
                |> Decode.andThen (fun int ->
                    match MinutesSwam.ofRaw int with
                    | Ok minutes -> Decode.succeed minutes
                    | Error msg -> Decode.fail msg
                ))
            
            }
        )

let serializeSession (session: Session) =
    session |> encode |> Encode.toString 0

let serializeSessions (sessions: seq<Session>) =
    sessions
    |> Seq.map encode
    |> Seq.toList
    |> Encode.list
    |> Encode.toString 0

let deserializeSession (json: string) =
    Decode.fromString decode json

