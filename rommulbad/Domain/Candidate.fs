module Domain.Candidate

open System

type Name = private | Name of string
type DateOfBirth = private | DateOfBirth of DateTime
type GuardianId = private | GuardianId of string
type Diploma = private | Diploma of string

type Candidate =
    { Name: string
      DateOfBirth: DateTime
      GuardianId: string
      Diploma: string }

module Name =
    let ofRaw (name: string) : Result<Name, string> =
        if name.Length > 0 then
            Ok <| Name name
        else
            Error "Name cannot be empty"
            
    let raw (Name n) = n
    
module DateOfBirth =
    let ofRaw (dateOfBirth: DateTime) : Result<DateOfBirth, string> =
        if dateOfBirth < DateTime.Now then
            Ok <| DateOfBirth dateOfBirth
        else
            Error "Date of birth cannot be in the future"
            
    let raw (DateOfBirth d) = d
    
module GuardianId =
    let ofRaw (guardianId: string) : Result<GuardianId, string> =
        if guardianId.Length > 0 then
            Ok <| GuardianId guardianId
        else
            Error "GuardianId cannot be empty"
            
    let raw (GuardianId g) = g
    
module Diploma =
    let ofRaw (diploma: string) : Result<Diploma, string> =
        match diploma with
        | "A" -> Ok <| Diploma diploma
        | "B" -> Ok <| Diploma diploma
        | "C" -> Ok <| Diploma diploma
        | "" -> Ok <| Diploma diploma
        | " " -> Ok <| Diploma diploma
        | _ -> Error "Diploma must be A, B or C"

    let raw (Diploma d) = d