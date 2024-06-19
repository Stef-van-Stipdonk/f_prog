module Domain.Candidate

open System
open System.Text.RegularExpressions

type Name = private | Name of string
type DateOfBirth = private | DateOfBirth of DateTime
type GuardianId = private | GuardianId of string
type Diploma = private | Diploma of string

type Candidate =
    { Name: Name
      DateOfBirth: DateOfBirth
      GuardianId: GuardianId
      Diploma: Diploma }

module Name =
    let ofRaw (name: string) : Result<Name, string> =
        if Regex.IsMatch(name, @"^[a-zA-Z]+(?: *[a-zA-Z]+)*$") then
            Ok <| Name name
        else
            Error "Name cannot contain numbers or special characters, and can have 0 or more spaces"
            
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
        | _ -> Error "Diploma must be A, B, C, or empty"

    let raw (Diploma d) = d