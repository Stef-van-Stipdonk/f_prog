namespace Rommulbad.Service

module Serialization = 

    open Rommulbad.Domain
    open Thoth.Json.Net

    let serializeCandidate (candidate: Candidate) =
        candidate |> Candidate.encode |> Encode.toString 0

    let serializeCandidates (candidates: seq<Candidate>) =
        candidates
        |> Seq.map Candidate.encode
        |> Seq.toList
        |> Encode.list
        |> Encode.toString 0

    let serializeSession (session: Session) =
        session |> Session.encode |> Encode.toString 0

    let serializeSessions (sessions: seq<Session>) =
        sessions
        |> Seq.map Session.encode
        |> Seq.toList
        |> Encode.list
        |> Encode.toString 0

    let deserializeSession (json: string) =
        Decode.fromString Session.decode json
