module Rommulbad.Service.Serialization.Guardian

open Thoth.Json.Net
open Domain.Guardian

let encode: Encoder<Guardian> =
    fun guardian ->
        Encode.object
            [
                "id", Encode.string (Id.raw guardian.Id)
                "name", Encode.string (Name.raw guardian.Name)
            ]
            
let decode: Decoder<Guardian> =
    Decode.object (fun get ->
        {
            Id = get.Required.Field "id" (Decode.string
                |> Decode.andThen (fun string ->
                    match Id.ofRaw string with
                    | Ok id -> Decode.succeed id
                    | Error msg -> Decode.fail msg
                )
            )
            Name = get.Required.Field "name" (Decode.string
                |> Decode.andThen (fun string ->
                    match Name.ofRaw string with
                    | Ok name -> Decode.succeed name
                    | Error msg -> Decode.fail msg
                )
            ) 
        }
        )
    
let serializeGuardian (guardian: Guardian) =
    guardian |> encode |> Encode.toString 0
    
let serializeGuardians (guardians: seq<Guardian>) =
    guardians
    |> Seq.map encode
    |> Seq.toList
    |> Encode.list
    |> Encode.toString 0

let deserializeGuardian (json: string) =
    Decode.fromString decode json