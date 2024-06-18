module Domain.Guardian

open Domain.Candidate

type Id = private | Id of string
type Name = private | Name of string

type Guardian =
    { Id: string
      Name: string
      Candidates: List<Candidate> }
    
module Id =
    let ofRaw (id: string) : Result<Id, string> =
        if id.Length > 0 then
            Ok <| Id id
        else
            Error "Id cannot be empty"
            
    let raw (Id i) = i
    
module Name =
    let ofRaw (name: string) : Result<Name, string> =
        if name.Length > 0 then
            Ok <| Name name
        else
            Error "Name cannot be empty"