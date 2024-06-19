module Rommulbad.Service.Serialization.Candidates

open Domain
open Domain.Candidate
open Thoth.Json.Net

let encode: Encoder<Candidate> =
    fun candidate ->
        Encode.object
            [ "name", Encode.string (Name.raw candidate.Name)
              "guardian_id", Encode.string (GuardianId.raw candidate.GuardianId)
              "diploma", Encode.string (Diploma.raw candidate.Diploma) ]

let decode: Decoder<Candidate> =
    Decode.object (fun get ->
        {
          Name = get.Required.Field "name" (Decode.string
              |> Decode.andThen (fun string ->
                  match Name.ofRaw string with
                  | Ok name -> Decode.succeed name
                  | Error msg -> Decode.fail msg
              )
          )
          DateOfBirth = get.Required.Field "date_of_birth" (Decode.datetimeUtc
              |> Decode.andThen (fun datetime ->
                  match DateOfBirth.ofRaw datetime with
                  | Ok dateOfBirth -> Decode.succeed dateOfBirth
                  | Error msg -> Decode.fail msg
              )
          )
          GuardianId = get.Required.Field "guardian_id" (Decode.string
              |> Decode.andThen (fun string ->
                  match GuardianId.ofRaw string with
                  | Ok guardianId -> Decode.succeed guardianId
                  | Error msg -> Decode.fail msg
              )
          )
          Diploma = get.Required.Field "diploma" (Decode.string
              |> Decode.andThen (fun string ->
                  match Diploma.ofRaw string with
                  | Ok diploma -> Decode.succeed diploma
                  | Error msg -> Decode.fail msg
              )
          )
        }
        )

let serializeCandidate (candidate: Candidate) =
    candidate |> encode |> Encode.toString 0

let serializeCandidates (candidates: seq<Candidate>) =
    candidates
    |> Seq.map encode
    |> Seq.toList
    |> Encode.list
    |> Encode.toString 0
    
    