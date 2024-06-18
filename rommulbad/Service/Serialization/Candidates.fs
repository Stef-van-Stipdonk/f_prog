module Rommulbad.Service.Serialization.Candidates

open Domain.Candidate
open Thoth.Json.Net

let encode: Encoder<Candidate> =
    fun candidate ->
        Encode.object
            [ "name", Encode.string candidate.Name
              "guardian_id", Encode.string candidate.GuardianId
              "diploma", Encode.string candidate.Diploma ]

let decode: Decoder<Candidate> =
    Decode.object (fun get ->
        { Name = get.Required.Field "name" Decode.string
          DateOfBirth = get.Required.Field "date_of_birth" Decode.datetimeUtc 
          GuardianId = get.Required.Field "guardian_id" Decode.string
          Diploma = get.Required.Field "diploma" Decode.string })

let serializeCandidate (candidate: Candidate) =
    candidate |> encode |> Encode.toString 0

let serializeCandidates (candidates: seq<Candidate>) =
    candidates
    |> Seq.map encode
    |> Seq.toList
    |> Encode.list
    |> Encode.toString 0
    
    