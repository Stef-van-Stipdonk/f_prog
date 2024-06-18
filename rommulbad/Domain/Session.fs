module Domain.Session

open System

type Name = private | Name of string
type Date = private | Date of DateTime
type Deep = private | Deep of bool
type Minutes = private | Minutes of int

type Session =
    {
      Name: string
      Deep: bool
      Date: DateTime
      Minutes: int }
    
module Name =
    let ofRaw (name: string) : Result<Name, string> =
        if name.Length > 0 then
            Ok <| Name name
        else
            Error "Name cannot be empty"

    let raw (Name n) = n

module Date =
    let ofRaw (date: DateTime) : Result<Date, string> =
        if date < DateTime.Now then
            Ok <| Date date
        else
            Error "Date cannot be in the future"

    let raw (Date d) = d
    
module Deep =
    let ofRaw (deep: bool) : Result<Deep, string> =
        Ok <| Deep deep

    let raw (Deep d) = d
    
module Minutes =
    let ofRaw (minutes: int) : Result<Minutes, string> =
        if minutes <= 0 then
            Error "Minutes must be greater than 0"
        else if minutes > 30 then
            Error "Minutes must be less than 60"
        else
            Ok <| Minutes minutes
   