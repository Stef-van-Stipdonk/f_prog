module Rommulbad.Service.Serialization.Session

open Domain.Session
open Thoth.Json.Net

let encode: Encoder<Session> =
    fun session ->
        Encode.object
            [ "deep", Encode.bool session.Deep
              "date", Encode.datetime session.Date
              "minutes", Encode.int session.Minutes ]

let decode: Decoder<Session> =
    Decode.object (fun get ->
        {
          Name = get.Required.Field "name" Decode.string
          Deep = get.Required.Field "deep" Decode.bool
          Date = get.Required.Field "date" Decode.datetimeUtc
          Minutes = get.Required.Field "minutes" Decode.int })

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

