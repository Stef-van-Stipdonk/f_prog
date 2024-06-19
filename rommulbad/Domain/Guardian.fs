module Domain.Guardian

open System.Text.RegularExpressions

type Id = private | Id of string
type Name = private | Name of string

type Guardian =
    {
      Id: Id
      Name: Name
    }
    
module Id =
    let ofRaw (id: string) : Result<Id, string> =
        if not (Regex.IsMatch(id, @"^[0-9]{3}-[a-zA-Z]{4}")) then
            Error "Value does not have the correct format"
        else
            Ok <| Id id
            
    let raw (Id i) = i
    
module Name =
    let ofRaw (name: string) : Result<Name, string> =
        if Regex.IsMatch(name, @"^[a-zA-Z]+(?: *[a-zA-Z]+)*$") then
            Ok <| Name name
        else
            Error "Name cannot contain numbers or special characters, and can have 0 or more spaces"
            
    let raw (Name n) = n