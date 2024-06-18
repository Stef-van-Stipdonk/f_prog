namespace Rommulbad.Domain

open Thoth.Json.Net
open System

type Candidate =
    { Name: string
      DateOfBirth: DateTime
      GuardianId: string
      Diploma: string }

module Candidate =
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

type Session =
    {
      Name: string
      Deep: bool
      Date: DateTime
      Minutes: int }

module Session =
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

type Guardian =
    { Id: string
      Name: string
      Candidates: List<Candidate> }
